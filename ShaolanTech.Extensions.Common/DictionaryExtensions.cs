using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
/// <summary>
/// 字典扩展函数
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// 读取字符串，如果没有键，则返回空串
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string TryReadString(this Dictionary<string, string> dic, string key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        return "";
    }
    /// <summary>
    /// 读取字典中的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key">关键字</param>
    /// <returns></returns>
    public static V Read<T, V>(this Dictionary<T, V> dic, T key, object defaultValue = null)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        if (defaultValue == null)
        {
            return default(V);
        }
        else
        {
            return (V)defaultValue;
        }
    }
    public static void AppendPrefixIfNotEmpty(this StringBuilder sb, string prefix, string content)
    {
        if (sb.Length != 0)
        {
            sb.Append($"{prefix}{content}");
        }
        else
        {
            sb.Append(content);
        }
    }
    public static T ReadJson<T>(this Dictionary<string, string> dic, string key)
    {
        T t = default(T);
        if (dic.ContainsKey(key))
        {
            t = dic[key].FromJsonString<T>();
        }
        return t;
    }
    /// <summary>
    /// 读取对象
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key">关键字</param>
    /// <param name="type">对象类型</param>
    /// <returns></returns>
    public static object ReadObject(this Dictionary<string, string> dic, string key, Type type)
    {
        object result = null;
        var value = dic.ReadString(key);
        if (value.IsNotNullOrEmpty())
        {

            return value.FromJsonString(type);
        }
        return result;
    }
    /// <summary>
    /// 读取Boolean类型值 
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key">关键字</param>
    /// <returns></returns>
    public static bool ReadBoolean(this Dictionary<string, string> dic, string key)
    {
        bool result = false;
        var value = dic.ReadString(key);
        if (value.IsNotNullOrEmpty())
        {
            bool.TryParse(value, out result);
        }
        return result;
    }
    /// <summary>
    /// 读取Int32类型值 
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key">关键字</param>
    /// <returns></returns>
    public static int ReadInt(this Dictionary<string, string> dic, string key)
    {
        int result = 0;
        var value = dic.ReadString(key);
        if (value.IsNotNullOrEmpty())
        {
            int.TryParse(value, out result);
        }
        return result;
    }
    /// <summary>
    /// 读取String类型值 
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key">关键字</param>
    /// <returns></returns>
    public static string ReadString(this Dictionary<string, string> dic, string key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        else
        {
            var currentKey = dic.Keys.FirstOrDefault(k => k.ToLower() == key.ToLower());
            if (currentKey != null)
            {
                return dic[currentKey];
            }
            return "";
        }
    }

}

