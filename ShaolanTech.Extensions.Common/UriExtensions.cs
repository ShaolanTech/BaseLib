using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
public static class UriExtensions
{
    /// <summary>
    /// 查询字符串集合
    /// </summary>
    public class QueryStringCollection
    {
        /// <summary>
        /// 当前查询参数总量
        /// </summary>
        public int Count { get; set; }

        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
        public QueryStringCollection(List<KeyValuePair<string, string>>inputParameters)
        {
            this.parameters = inputParameters;
            this.Count = inputParameters.Count;
        }
        /// <summary>
        /// 当前集合中是否包括指定关键词
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return this.parameters.Any(p => p.Key.ToLower() == key.ToLower());
        }
        /// <summary>
        /// 获取指定关键字对应的第一个值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public string GetQueryString(string key)
        {
            if (this.ContainsKey(key))
            {
                var item = this.parameters.FirstOrDefault(p => p.Key.ToLower() == key.ToLower());
                return item.Value;
            }
            return ""; 
        }
        /// <summary>
        /// 获取指定关键字对应的所有个值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<string> GetQueryStrings(string key)
        {
            List<string> result = new List<string>();
            if (this.ContainsKey(key))
            {
                result.AddRange(this.parameters.Where(k => k.Key.ToLower() == key.ToLower()).Select(k => k.Value));
            }
            return result;
        }
        public List<KeyValuePair<string,string>>GetAll()
        {
            return this.parameters;
        }
    }
    /// <summary>
    /// 获取指定链接中的查询字符串
    /// </summary>
    /// <param name="uri">链接</param>
    /// <returns></returns>
    public static QueryStringCollection ParseQueryStrings(this Uri uri)
    {
        
        if (uri.AbsolutePath.IsNullOrEmpty())
        {
            return new QueryStringCollection( new List<KeyValuePair<string, string>>());
        }
        List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
        var tokens = uri.ToString().Split('?');
        if (tokens.Length == 1)
        {
            return new QueryStringCollection(new List<KeyValuePair<string, string>>());
        }
        else
        {
            var queries = tokens[1].Split('&');
         
            foreach (var query in queries)
            {
                var q = query.Split('=');
                if (q.Length == 2)
                {
                    result.Add(new KeyValuePair<string, string>(q[0],q[1])); 
                }
            }
            return new QueryStringCollection(result);
        }
    }
}
