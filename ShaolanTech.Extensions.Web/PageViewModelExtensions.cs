using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace ShaolanTech.Extensions.Web
{
    public class PageViewModelConfig
    {
        /// <summary>
        /// 设置写入请求日志的回调函数 
        /// </summary>
        public Action<PageViewRequestLog> SetLog { get; set; }
    }
    public static class PageViewModelExtensions
    {
        /// <summary>
        /// 使用基于MVC视图的ViewModel模式插件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config">PageView配置项</param>
        public static void UserPageViewModel(this IApplicationBuilder app, Action<PageViewModelConfig> config = null)
        {
            if (config != null)
            {
                var configModel = new PageViewModelConfig();
                config(configModel);
                if (configModel.SetLog != null)
                {
                    PageViewModelMiddelWare.RequestLog = configModel.SetLog;
                }
            }
            app.UseMiddleware<PageViewModelMiddelWare>();
        }
        /// <summary>
        /// 注册当前页面的PageViewModel实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        public static void UsePageViewModel<T>(this RazorPage page,bool needAuthorization=false) where T : PageViewModel
        {
            var model = $"{page.Context.Request.Path.ToString().ToLower()}?viewapi=";
            PageViewModelMiddelWare.RegisterPageViewModel<T>(model, needAuthorization);
            //PageViewModel.RegisterPageViewModel();
        }
    }
    
    public class PageViewMethodAttribute : Attribute
    {
        /// <summary>
        /// 是否存储请求日志，默认为True
        /// </summary>
        public bool Log { get; set; } = false;
        /// <summary>
        /// 是否强制要求接口授权
        /// </summary>
        public bool Authorized { get; set; } = false;
    }
    /// <summary>
    /// PageView请求日志
    /// </summary>
    public class PageViewRequestLog
    {
        /// <summary>
        /// 请求Form参数
        /// </summary>
        public Dictionary<string, string> Forms { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// PageView类型名
        /// </summary>
        public string PageViewClassType { get; set; }
        /// <summary>
        /// 请求的函数名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }
        /// <summary>
        /// Http请求类型
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 路由IP组
        /// </summary>
        public string XForwardedFor { get; set; }

        public string RequestError { get; set; }

        public string Referer { get; set; }
    }
    /// <summary>
    /// 基于MVC视图的ViewModel模式插件
    /// </summary>
    public class PageViewModelMiddelWare
    {
        internal class PageViewRequestInfo
        {
            public bool NeedAuthorization { get; set; }
            public Type PageViewType { get; set; }

            public PageViewModel CreateInstance() 
            {
                return (PageViewModel)Activator.CreateInstance(this.PageViewType);
            }
        }
        internal static Action<PageViewRequestLog> RequestLog { get; set; }
        private static ConcurrentDictionary<string, PageViewRequestInfo> pageViewModels = new ConcurrentDictionary<string, PageViewRequestInfo>();

        public static void RegisterPageViewModel<T>(string name,bool needAuthorization=false) where T : PageViewModel
        {
            if (pageViewModels.ContainsKey(name) == false)
            {
                pageViewModels.TryAdd(name, new PageViewRequestInfo { NeedAuthorization = needAuthorization, PageViewType=typeof(T) });
            }

        }
        private RequestDelegate _nextDelegate;

        public PageViewModelMiddelWare(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.QueryString("viewapi").IsNotNullOrEmpty() && pageViewModels.ContainsKey($"{httpContext.Request.Path.ToString().ToLower()}?viewapi="))
            {
                PageViewRequestLog log = new PageViewRequestLog();
                log.RequestTime = DateTime.Now;
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    log.XForwardedFor = httpContext.Request.Headers["X-Forwarded-For"];
                }
                var method = httpContext.Request.QueryString("viewapi");
                log.MethodName = method;
                var pageInfo = pageViewModels[$"{httpContext.Request.Path.ToString().ToLower()}?viewapi="];
                
                log.PageViewClassType = pageInfo.PageViewType.FullName;
                object result = null;
                httpContext.Response.ContentType = "application/json";
                log.Url = httpContext.Request.GetDisplayUrl();
                log.HttpMethod = httpContext.Request.Method;
                log.UserName = httpContext.User.Identity.Name;
                var referer = "";
                if (httpContext.Request.Headers.ContainsKey("Referer") != false)
                {
                    if (httpContext.Request.Headers.TryGetValue("Referer", out StringValues values))
                    {
                        if (values.Count != 0)
                        {
                            referer = values[0];
                        }
                    }
                }

                log.Referer = referer;
                if (httpContext.Request.Method == "POST")
                {
                    log.Forms = new Dictionary<string, string>();
                    foreach (var item in httpContext.Request.Form)
                    {
                        log.Forms.Add(item.Key, string.Join(";", item.Value));
                    }
                }

                var methodInfo = pageInfo.PageViewType.GetMethods().FirstOrDefault(m => m.Name.ToLower() == method.ToLower());
                //var attr = methodInfo.GetCustomAttribute<PageViewMethodAttribute>();
                //var typeattr = type.GetCustomAttribute<PageViewModelAttribute>();
                if (methodInfo == null)
                {
                    result = new ResultInfo { OperationDone = false, Message = "方法不存在" };
                    log.RequestError = "方法不存在";
                }
                else
                {
                     
                    if (pageInfo.NeedAuthorization&&  !httpContext.User.Identity.IsAuthenticated)
                    {
                        result = new ResultInfo { OperationDone = false, Message = "请示未授权" };
                        log.RequestError = "请示未授权";
                    }
                    else
                    {
                        using var instance = pageInfo.CreateInstance();
                        instance.CurrentHttpContext = httpContext;

                        result = await instance.GetResult(method);
                        if (result is ResultInfo)
                        {
                            var r = (ResultInfo)result;
                            if (!r.OperationDone)
                            {
                                log.RequestError = r.Message;
                            }
                        }
                    }

                }
                if (RequestLog != null )
                {

                    RequestLog(log);
                }
                var serializer = JsonSerializer.Create(new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver { }
                });

                var stringWriter = new StringWriter();
                var writer = new JsonTextWriter(stringWriter);
                serializer.Serialize(writer, result);
                var serialized = stringWriter.ToString();
                await httpContext.Response.WriteAsync(serialized, Encoding.UTF8);

            }
            else
            {
                await _nextDelegate(httpContext);
            }
        }
    }
}
