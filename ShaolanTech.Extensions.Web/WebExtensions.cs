using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShaolanTech.Extensions.Web
{
    public static class WebExtensions
    {
        /// <summary>
        /// 获取当前页面是否在DEBUG模式下运行
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static bool IsInDebugMode(this RazorPage page)
        {
#if DEBUG
            return true;
#else
        return false;
#endif
        }
        /// <summary>
        /// 获取当前网站的地址
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string HostUrl(this RazorPage page)
        {

            return $"{page.Context.Request.Scheme}://{page.Context.Request.Host}";
        }
        public static string HostUrl(this Controller page)
        {
            return $"{page.Request.Scheme}://{page.Request.Host}";
        }
        /// <summary>
        /// 拼接子地址
        /// </summary>
        /// <param name="page"></param>
        /// <param name="subUrl">地址路径</param>
        /// <returns></returns>
        public static string BuildUrl(this RazorPage page, string subUrl)
        {
            var url = subUrl;
            if (url.StartsWith("/"))
            {
                url = url.Substring(1, url.Length - 1);
            }
            return $"{page.HostUrl()}/{url}";
        }
        /// <summary>
        /// 从CDN加载脚本
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cdn">CDNURL</param>
        /// <param name="scriptPaths">脚本子路径 </param>
        /// <returns></returns>
        public static HtmlString LoadScriptsFromCDN(this RazorPage page, string cdn, params string[] scriptPaths)
        {
            List<string> scripts = new List<string>();
            foreach (var item in scriptPaths.Distinct())
            {
                scripts.Add($"<script src='{cdn}{item}'></script>");
            }
            return new HtmlString(string.Join(" ", scripts));
        }
        /// <summary>
        /// 从CDN加载CSS脚本
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cdn">CDNURL</param>
        /// <param name="scriptPaths">脚本子路径 </param>
        /// <returns></returns>
        public static HtmlString LoadStylesFromCDN(this RazorPage page, string cdn, params string[] scriptPaths)
        {
            List<string> scripts = new List<string>();

            foreach (var item in scriptPaths.Distinct())
            {

                scripts.Add($"<link rel='stylesheet' type='text/css'  href='{cdn}{item}'/>");

            }
            return new HtmlString(string.Join(" ", scripts));
        }
        /// <summary>
        /// 添加JS文件引用
        /// </summary>
        /// <param name="page"></param>
        /// <param name="path">脚本路径</param>
        /// <param name="cache">是否使用缓存，默认为False</param>
        /// <returns></returns>
        public static HtmlString LoadScript(this RazorPage page, string path, bool cache = true)
        {
            var cacheString = "";
            if (!cache)
            {
                cacheString = $"?{DateTime.Now.Ticks}";
            }
            if (path.StartsWith("http"))
            {
                return new HtmlString($"<script src='{path}{cacheString}'></script>");
            }
            else
            {
                return new HtmlString($"<script src='{page.BuildUrl($"{path}{cacheString}")}'></script>");
            }
        }
        /// <summary>
        /// 添加CSS文件引用
        /// </summary>
        /// <param name="page"></param>
        /// <param name="path">脚本路径</param>
        /// <param name="cache">是否使用缓存，默认为False</param>
        /// <returns></returns>
        public static HtmlString LoadStyle(this RazorPage page, string path, bool cache = true)
        {
            var cacheString = "";
            if (!cache)
            {
                cacheString = $"?{DateTime.Now.Ticks}";
            }
            if (path.StartsWith("http"))
            {
                //<link rel="stylesheet" type="text/css" href="static/h-ui/css/H-ui.min.css" />

                return new HtmlString($"<link rel='stylesheet' type='text/css'  href='{path}{cacheString}'/>");
            }
            else
            {
                return new HtmlString($"<link rel='stylesheet' type='text/css' href='{page.BuildUrl($"{path}{cacheString}")}'/>");
            }
        }
        /// <summary>
        /// 拼接子地址
        /// </summary>
        /// <param name="page"></param>
        /// <param name="subUrl">地址路径</param>
        /// <returns></returns>
        public static string BuildUrl(this Controller page, string subUrl)
        {
            var url = subUrl;
            if (url.StartsWith("/"))
            {
                url = url.Substring(1, url.Length - 1);
            }
            return $"{page.HostUrl()}/{url}";
        }
        public static IHtmlContent KnockoutBind(this RazorPage page, string field)
        {
            return new HtmlString($"<!--ko text:{field}--><!--/ko-->");
        }
        /// <summary>
        /// 读取查询字符串的值 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static string QueryString(this RazorPage page, string key)
        {
            if (page.Context.Request.Query.ContainsKey(key) == false)
            {
                return null;
            }
            return page.Context.Request.Query[key][0];
        }
        /// <summary>
        /// 读取查询字符串的值 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static string[] QueryStrings(this RazorPage page, string key)
        {
            if (page.Context.Request.Query.ContainsKey(key) == false)
            {
                return new string[0];
            }
            return page.Context.Request.Query[key];
        }
       
        public static string RenderScripts(string physicalPath, string virtualPath)
        {
            var dir = Directory.GetCurrentDirectory() + physicalPath;
            var files = Directory.GetFiles(dir, "*.js");
            StringBuilder sb = new StringBuilder();
            foreach (var file in files)
            {
                sb.AppendFormat("<script src='{0}/{1}'></script>", virtualPath, Path.GetFileName(file)).AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// 读取查询字符串的值 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static string QueryString(this HttpRequest request, string key)
        {
            if (request.Query.ContainsKey(key) == false)
            {
                return null;
            }
            return request.Query[key][0];
        }
        /// <summary>
        /// 读取查询字符串的值 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static string[] QueryStrings(this HttpRequest request, string key)
        {
            if (request.Query.ContainsKey(key) == false)
            {
                return new string[0];
            }
            return request.Query[key];
        }
        public static void Save(this IFormFile file, string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {


                }
            }
            using (var fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }
        
        public static bool ReadBoolean(this HttpContext controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return false;
            }
            bool v = false;
            bool.TryParse(controller.PostParameters()[name], out v);
            return v;

        }
        public static bool ReadBoolean(this Controller controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return false;
            }
            bool v = false;
            bool.TryParse(controller.PostParameters()[name], out v);
            return v;

        }
        public static int ReadInt(this HttpContext controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return 0;
            }
            int v;
            if (int.TryParse(controller.PostParameters()[name], out v))
            {
                return v;
            }
            return 0;
        }
        public static int ReadInt(this Controller controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return 0;
            }
            int v;
            if (int.TryParse(controller.PostParameters()[name], out v))
            {
                return v;
            }
            return 0;
        }
        public static string ReadString(this HttpContext controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return "";
            }
            return controller.PostParameters()[name];
        }
        public static string ReadString(this Controller controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return "";
            }
            return controller.PostParameters()[name];
        }
        public static string ReadStringMatch(this HttpContext controller, string pattern)
        {
            var matchItems = controller.PostParameters().Where(a => a.Key.IsMatch(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            if (matchItems.Count() != 0)
            {
                return matchItems.First().Value;
            }
            else
            {
                return "";
            }
        }
        public static string ReadStringMatch(this Controller controller, string pattern)
        {
            var matchItems = controller.PostParameters().Where(a => a.Key.IsMatch(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            if (matchItems.Count() != 0)
            {
                return matchItems.First().Value;
            }
            else
            {
                return "";
            }
        }
        public static List<string> ReadStringsMatch(this HttpContext controller, string pattern)
        {
            var matchItems = controller.PostParameters().Where(a => a.Key.IsMatch(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            if (matchItems.Count() != 0)
            {
                return matchItems.Select(item => item.Value).ToList();
            }
            else
            {
                return new List<string>();
            }
        }
        public static List<string> ReadStringsMatch(this Controller controller, string pattern)
        {
            var matchItems = controller.PostParameters().Where(a => a.Key.IsMatch(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            if (matchItems.Count() != 0)
            {
                return matchItems.Select(item => item.Value).ToList();
            }
            else
            {
                return new List<string>();
            }
        }
        public static Dictionary<string, object> ReadJson(this HttpContext controller, string name)
        {
            var json = controller.ReadObject<JObject>(name);
            return json.ToDictionary();
        }
        public static Dictionary<string, object> ReadJson(this Controller controller, string name)
        {
            var json = controller.ReadObject<JObject>(name);
            return json.ToDictionary();
        }
        public static T ReadObject<T>(this HttpContext context, string name)
        {
            name = name.ToLower();
            if (context.PostParameters().ContainsKey(name) == false)
            {
                return default(T);
            }
            var obj = context.PostParameters()[name].FromJsonString<T>();
            return obj;
        }
        public static T ReadObject<T>(this Controller controller, string name)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return default(T);
            }
            var obj = controller.PostParameters()[name].FromJsonString<T>();
            return obj;
        }
        public static object ReadObject(this HttpContext context, string name, Type type)
        {
            name = name.ToLower();
            if (context.PostParameters().ContainsKey(name) == false)
            {
                return null;
            }
            var json = context.PostParameters()[name];
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }
        public static object ReadObject(this Controller controller, string name, Type type)
        {
            name = name.ToLower();
            if (controller.PostParameters().ContainsKey(name) == false)
            {
                return null;
            }
            var json = controller.PostParameters()[name];
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }
        public static Dictionary<string, string> PostParameters(this HttpContext context)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            var query = context.Request.Query.Keys;
            foreach (var key in query)
            {
                postData.Add(key.ToLower(), context.Request.Query[key]);
            }

            if (context.Request.Method == "POST")
            {
                if (context.Request.Form != null)
                {
                    var form = context.Request.Form;
                    foreach (var item in form.Keys)
                    {
                        if (postData.ContainsKey(item) == false)
                        {
                            postData.Add(item.ToLower(), form[item]);
                        }

                    }
                }

            }

            return postData;
        }
        public static Dictionary<string, string> PostParameters(this Controller controller)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            var query = controller.Request.Query.Keys;
            foreach (var key in query)
            {
                postData.Add(key.ToLower(), controller.Request.Query[key]);
            }

            if (controller.Request.Method == "POST")
            {
                if (controller.HttpContext.Request.ContentType != "text/xml" && controller.HttpContext.Request.Form != null)
                {
                    var form = controller.HttpContext.Request.Form;
                    foreach (var item in form.Keys)
                    {
                        if (postData.ContainsKey(item) == false)
                        {
                            postData.Add(item.ToLower(), form[item]);
                        }

                    }
                }

            }

            return postData;
        }
        /// <summary>
        /// http头的Referer字段读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadParameterFromReferer(this HttpContext context, string key)
        {
            if (context == null)
            {
                return "";
            }
            if (context.Request.Headers.ContainsKey("Referer") == false)
            {
                return "";
            }
            if (context.Request.Headers.TryGetValue("Referer", out StringValues values))
            {
                if (values.Count == 0)
                {
                    return "";
                }
                var url = new Uri(values[0]);
                var coll = url.ParseQueryStrings();
                if (coll.ContainsKey(key))
                {
                    return coll.GetQueryString(key);
                }
                return "";
            }
            return "";
        }
    }
}
