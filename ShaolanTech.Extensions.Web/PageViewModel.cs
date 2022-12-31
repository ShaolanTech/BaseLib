using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Reflection;

namespace ShaolanTech.Extensions.Web
{
    public class PageViewModel : IDisposable
    {


        /// <summary>
        /// 从配置文件中读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadConfig(string key)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                 .AddJsonFile("appsettings.json", optional: true);

            var configuration = builder.Build();
            return configuration[key];
        }
        /// <summary>
        /// 从Http请求中读取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">对象关键字</param>
        /// <returns></returns> 
        public T ReadObject<T>(string key)
        {
            return this.CurrentHttpContext.ReadObject<T>(key);
        }
        /// <summary>
        /// 从Http请求中读取对象
        /// </summary>
        /// <param name="key">对象关键字</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public object ReadObject(string key, Type type)
        {
            return this.CurrentHttpContext.ReadObject(key, type);
        }
        /// <summary>
        /// 从Http请求中读取整数
        /// </summary>
        /// <param name="key">对象关键字</param>
        /// <returns></returns>
        public int ReadInt(string key)
        {
            return this.CurrentHttpContext.ReadInt(key);
        }
        public bool ReadBoolean(string key)
        {
            return this.CurrentHttpContext.ReadBoolean(key);
        }
        /// <summary>
        /// 从Http请求中读取字符串
        /// </summary>
        /// <param name="key">对象关键字</param>
        /// <returns></returns>
        public string ReadString(string key)
        {
            return this.CurrentHttpContext.ReadString(key);
        }
        public string ReadStringMatch(string pattern)
        {
            return this.CurrentHttpContext.ReadStringMatch(pattern);
        }
        public List<string> ReadStringsMatch(string pattern)
        {
            return this.CurrentHttpContext.ReadStringsMatch(pattern);
        }
        /// <summary>
        /// 当前实例类型
        /// </summary>
        protected Type Type { get; private set; }
        /// <summary>
        /// 获取当前Request
        /// </summary>
        public HttpRequest Request
        {
            get
            {

                if (this.CurrentHttpContext != null)
                {
                    return this.CurrentHttpContext.Request;
                }

                return null;
            }

        }
        /// <summary>
        /// 获取当前的Response
        /// </summary>
        public HttpResponse Response
        {
            get
            {
                if (this.CurrentHttpContext != null)
                {
                    return this.CurrentHttpContext.Response;
                }

                return null;
            }
        }
        /// <summary>
        /// 获取当前请求中Post的文件列表
        /// </summary>
        public IFormFileCollection UploadedFiles
        {
            get
            {
                return this.Request.Form.Files;
            }
        }

        public HttpContext CurrentHttpContext { get; internal set; }
        /// <summary>
        /// 方法缓存
        /// </summary>
        // private static ConcurrentDictionary<string, Dictionary<string, MethodInfo>> methodsDic = new ConcurrentDictionary<string, Dictionary<string, MethodInfo>>();
        MethodInfo[] methods;
        private string typeName = "";
        public PageViewModel()
        {

            var type = this.GetType();
            this.Type = type;
            this.methods = this.Type.GetMethods();

            this.typeName = type.FullName;


        }
         
        /// <summary>
        /// 指定方法名，并对其进行调用
        /// </summary>
        /// <param name="method">方法名</param>
        /// <returns></returns>
        public async Task<object> GetResult(string method)
        {
            var func = methods.FirstOrDefault(m => m.Name.ToLower() == method.ToLower());

            var args = func.GetParameters();
            var parameters = this.CurrentHttpContext.PostParameters(); 
            var result = (await InvokeInProcess(func, parameters, this ));
            return result;
        }

        private async Task<object> InvokeInProcess(MethodInfo func, Dictionary<string, string> parameters, object instance = null)
        {
            object Result = null;
            var args = func.GetParameters();

            if (instance == null)
            {
                instance = Activator.CreateInstance(func.DeclaringType);
            }
            if (args.Length == 0)
            {
                //异步方法
                if (func.ReturnType.GetTypeInfo().BaseType == typeof(Task))
                {
                    Result = await (dynamic)func.Invoke(instance, null);
                }
                else//同步方法
                {
                    Result = func.Invoke(instance, null);
                }
            }
            else
            {

                List<object> argValues = new List<object>();
                foreach (var arg in args)
                {
                    switch (arg.ParameterType.Name)
                    {
                        case "String":
                            argValues.Add(parameters.ReadString(arg.Name));
                            break;
                        case "Int32":
                            argValues.Add(parameters.ReadInt(arg.Name));
                            break;
                        case "Boolean":
                            argValues.Add(parameters.ReadBoolean(arg.Name));
                            break;
                        default:
                            argValues.Add(parameters.ReadObject(arg.Name, arg.ParameterType));
                            break;
                    }
                }
                //异步方法
                if (func.ReturnType.GetTypeInfo().BaseType == typeof(Task))
                {
                    Result = ((object)(await (dynamic)func.Invoke(instance, argValues.ToArray())));
                }
                else//同步方法
                {
                    Result = func.Invoke(instance, argValues.ToArray());

                }


            }


            return Result;
        }




        public Uri GetRefererUrl()
        {
            if (this.Request == null)
            {
                return null;
            }
            if (this.Request.Headers.ContainsKey("Referer") == false)
            {
                return null;
            }
            if (this.Request.Headers.TryGetValue("Referer", out StringValues values))
            {
                if (values.Count == 0)
                {
                    return null;
                }
                var url = new Uri(values[0]);
                return url;
            }
            return null;
        }
        /// <summary>
        /// 从http头的Referer字段中读取路由路径的最后一段
        /// </summary>
        /// <returns></returns>
        public string ReadRefererUrlLastPath()
        {
            var url = this.GetRefererUrl();
            return url.Segments.Last();
        }
        /// <summary>
        /// http头的Referer字段读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadParameterFromReferer(string key)
        {
            return this.CurrentHttpContext.ReadParameterFromReferer(key);

        }
         
        public void Dispose()
        {
            this.methods = null;
        }
    }
}
