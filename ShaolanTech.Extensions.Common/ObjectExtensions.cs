using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
//using System.Text.Json;
using Warensoft.EntLib.Common;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
/// <summary>
/// 对象转化扩展函数类
/// </summary>
public static class ObjectExtensions
{

    /// <summary>
    /// 将JSON字符串转化为ExpandoObject对象 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json">JSON字符串</param>
    /// <returns></returns>
    public static System.Dynamic.ExpandoObject CastJsonToExpandoObject(this string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(json);
    }
    /// <summary>
    /// 将JSON字符串转化为Dynamic对象 
    /// </summary>
    /// <param name="json">JSON字符串</param>
    /// <returns></returns>
    public static dynamic CastJsonToDynamicObject(this string json)
    {

        return json.FromJsonString();
    }
    /// <summary>
    /// 将JSON转化为JToken动态对象
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static dynamic FromJsonString(this string s)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject(s);
    }
    /// <summary>
    /// 将JSON转化为JToken动态对象
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static object FromJsonString(this string s, Type type)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject(s, type);
    }
    /// <summary>
    /// 将JSON字符串转化为JTOKEN对象
    /// </summary>
    /// <param name="s">JSON字符串</param>
    /// <returns></returns>
    public static List<JToken> FromJsonStringJToken(this string s)
    {
        List<JToken> json = JValue.Parse(s).ToList();
        return json;
    }

    public static Dictionary<string, object> ToDictionary(this Newtonsoft.Json.Linq.JObject token)
    {
        var result = new Dictionary<string, object>();
        foreach (var item in token)
        {
            result.Add(item.Key, item.Value.ToObject<object>());
        }

        return result;
    }
    public static Dictionary<string, T> GetDictionary<T>(this Newtonsoft.Json.Linq.JObject token)
    {
        var result = new Dictionary<string, T>();
        foreach (var item in token)
        {
            result.Add(item.Key, item.Value.ToObject<T>());
        }

        return result;
    }
    /// <summary>
    /// 对相同类型进行属性拷贝
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="dest">拷贝源</param>
    /// <param name="source">拷贝目标</param>
    /// <param name="notIncludes">用于指定不拷贝哪些属性</param>
    public static void CopyPropertiesFrom<T>(this T dest, T source, params string[] notIncludes)
    {
        TypeInfo info = typeof(T).GetTypeInfo();
        var properties = info.DeclaredProperties;
        foreach (var property in properties)
        {
            if (notIncludes == null || (notIncludes != null && notIncludes.Contains(property.Name) == false))
            {
                var newValue = property.GetValue(source);
                property.SetValue(dest, newValue);
            }

        }
    }
    
    /// <summary>
    /// 将JSON字符串转化为指定对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="s">JSON字符串</param>
    /// <returns></returns>
    public static T FromJsonString<T>(this string s)
    {
        T result = default(T);
        if (s.IsNullOrEmpty())
        {
            return default(T);
        }
        try
        {
            result = JsonConvert.DeserializeObject<T>(s);
        }
        catch (Exception ex)
        {


        } 
        return result;
    }
    public static string ToJsonString(this object obj, bool withOrder = false )
    {

        if (!withOrder)
        {
            StringWriter wr = new StringWriter();
            var jsonWriter = new JsonTextWriter(wr);
            jsonWriter.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            var js = new JsonSerializer();
            js.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            js.NullValueHandling = NullValueHandling.Ignore;
            js.Serialize(jsonWriter, obj);
            var result = wr.ToString().Replace("\\u0000", "").Replace("'", "\\u0027");
            return result;
        }
        else
        {

            TypeInfo info = obj.GetType().GetTypeInfo();
            var properties = info.DeclaredProperties;
            StringBuilder sb = new StringBuilder("{");
            foreach (var property in properties.OrderBy(p => p.Name))
            {
                sb.Append("\"").Append(property.Name).Append("\":");
                sb.Append("\"").Append(property.GetValue(obj)).Append("\",");
            }
            sb.RemoveLast().Append("}");
            return sb.ToString();
        }

    }


    /// <summary>
    /// 为可扩展对象添加属性
    /// </summary>
    /// <param name="expando"></param>
    /// <param name="propertyName">属性名</param>
    /// <param name="propertyValue">属性值</param>
    public static void SetPropery(this ExpandoObject expando, string propertyName, object propertyValue)
    {
        // ExpandoObject supports IDictionary so we can extend it like this
        var expandoDict = expando as IDictionary<string, object>;
        if (expandoDict.ContainsKey(propertyName))
            expandoDict[propertyName] = propertyValue;
        else
            expandoDict.Add(propertyName, propertyValue);
    }

    /// <summary>
    /// 获取可扩展对象的属性值 
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    /// <param name="expando"></param>
    /// <param name="propertyName">属性名</param>
    /// <returns></returns>
    public static T GetPropery<T>(this ExpandoObject expando, string propertyName)
    {
        // ExpandoObject supports IDictionary so we can extend it like this
        var expandoDict = expando as IDictionary<string, object>;
        if (expandoDict.ContainsKey(propertyName))
            return (T)expandoDict[propertyName];
        else
            return default(T);
    }
}
