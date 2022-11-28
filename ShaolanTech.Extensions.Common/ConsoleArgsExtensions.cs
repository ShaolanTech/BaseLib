using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

/// <summary>
/// 控制台参数集合
/// </summary>
public class ArgumentCollection
{
    string[] args = null;
    Dictionary<string, string> argDic = new Dictionary<string, string>();
    /// <summary>
    /// 获取参数的数量
    /// </summary>
    public int ArgumentCount
    {
        get
        {
            return this.argDic.Count;
        }
    }
    public ArgumentCollection(string[] args)
    {
        this.args = args;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith("-"))
            {
                if (i + 1 != args.Length && args[i + 1].StartsWith("-") == false)
                {
                    if (this.argDic.ContainsKey(args[i]) == false)
                    {
                        this.argDic.Add(args[i], args[i + 1]);
                    }
                    i++;
                }
                else
                {
                    this.argDic.Add(args[i], "");
                }
            }
        }

    }
    public bool TryGetInt(string key, out int value)
    {
        if (this.HasKey(key))
        {
            value = this.ReadInt(key);
            return true;
        }
        else
        {
            value = -1000;
            return false;
        }
    }
    public bool TryGetStringNotEmpty(string key, out string value)
    {
        if (this.HasKey(key))
        {
            value = this.ReadString(key);
            if (value.IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }
        else
        {
            value = "";
            return false;
        }
    }
    public bool TryGetString(string key, out string value)
    {
        if (this.HasKey(key))
        {
            value = this.ReadString(key);
            return true;
        }
        else
        {
            value = "";
            return false;
        }
    }
    public bool HasKey(string key)
    {
        return this.argDic.ContainsKey(key);
    }
    public string ReadString(string key)
    {
        return this.argDic.ReadString(key);
    }
    public bool ReadBoolean(string key)
    {
        return this.argDic.ReadBoolean(key);
    }
    public int ReadInt(string key)
    {
        return this.argDic.ReadInt(key);
    }
    public T ReadObject<T>(string key)
    {
        return (T)this.argDic.ReadObject(key, typeof(T));
    }
}
public static class ConsoleArgsExtensions
{
    /// <summary>
    /// 获取控制台输入参数的集合
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ArgumentCollection ToArgumentCollection(this string[] source)
    {
        return new ArgumentCollection(source);
    }
}

