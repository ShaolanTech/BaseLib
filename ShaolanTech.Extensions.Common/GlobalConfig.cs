using System;
using System.Collections.Generic;
using System.Text;


public class ExtensionConfig
{ }
public class GlobalConfig
{
    /// <summary>
    /// 扩展配置信息
    /// </summary>
    public static ExtensionConfig Extension { get; set; } = new ExtensionConfig();
    private static Dictionary<string, object> config = new Dictionary<string, object>();
    public static object Get(string key)
    {
        if (config.ContainsKey(key))
        {
            return config[key];
        }
        return null;
    }
    public static T Get<T>(string key)
    {
        if (!config.ContainsKey(key))
        {
            config.Add(key, default(T));
        }
        return (T)config[key];

    }
    public static void Set(string key, object value)
    {
        if (config.ContainsKey(key))
        {
            config[key] = value;
        }
        else
        {
            config.Add(key, value);
        }
    }
}

