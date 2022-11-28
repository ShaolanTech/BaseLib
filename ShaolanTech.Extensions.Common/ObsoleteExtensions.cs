using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Warensoft.EntLib.Common;
using System.Linq;

/// <summary>
/// 已经过时的扩展函数
/// </summary>
public static class ObsoleteExtensions
{
    [Obsolete("该扩展已经过时，由于指代不明确，请使其他明确方法代替")]
    public static T CastFromJsonString<T>(this object obj)
    {
        if (obj == null)
        {
            return default(T);
        }
        var s = obj.ToString();
        T result = default(T);
        try
        {
            result = s.FromJsonString<T>();
        }
        catch (Exception e)
        {


        }

        return result;
    }
    [Obsolete("该扩展已经过时，请使用GetNumber函数代替")]
    /// <summary>
    /// 获取字符串中的第一个数字
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int GetNumber(this string s)
    {
        var numbers = s.GetIntNumbers();
        if (numbers.Count == 0)
        {
            return -1;
        }
        return numbers[0];
    }
    [Obsolete("该扩展已经过时，请使用SplitList函数代替")]
    public static string[] Split(this string s, string splitter)
    {
        if (s.IsNullOrEmpty())
        {
            return new string[0];
        }
        return s.Split(new string[] { splitter }, StringSplitOptions.None);
    }
    [Obsolete("该扩展已经过时，请使用ClearText函数代替")]
    public static string GetClearedString(this string s, string matchpattern, bool useMD5 = false)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        var chars = s.Where(c => c.ToString().IsChinese() || c.ToString().IsMatch(matchpattern, RegexOptions.IgnoreCase)).ToArray();
        if (useMD5 == false)
        {
            return new string(chars).ToLower();
        }
        else
        {
            return SecurityUtil.GetMD5String(new string(chars).ToLower());
        }
    }
    [Obsolete("该扩展已经过时，请使用ClearText函数代替")]
    /// <summary>
    /// 获取中文及字母字符
    /// </summary>
    /// <returns></returns>
    public static string GetAlphabets(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        char[] c = s.ToCharArray().Where(c1 => (c1 >= 0x4e00 && c1 <= 0x9fbb) || (c1 >= 65 && c1 <= 90) || (c1 >= 97 && c1 <= 122)).ToArray();
        return new string(c);
    }
    [Obsolete("该扩展已经过时，请使用ClearText函数代替")]
    public static string GetClearString(this string input, bool useMD5 = false, bool withNumber = true)
    {
        input = input.Remove("\n", "\r");
        if (input.IsNullOrEmpty())
        {
            return "";
        }
        string exp = "";
        if (input.StartsWith("0"))
        {
            input = input.Substring(1, input.Length - 1);
        }
        if (withNumber)
        {
            exp = @"(;)|(')|(\s)|(/)|(\|)|(\+)|(-)|(:)|(\()|(\))|(\[)|(\])|(>)|(<)|(,)|(\.)|(“)|(”)|(—)|(＋)|(：)|(\?)|(（)|(）)|(《)|(》)|(~)|(\!)|(_)|(#)|(\$)|(\%)|(\^)|(\&)|(\*)(\=)|(\\)|(、)|(。)|(，)|(？)|(，)|(／)";
        }
        else
        {
            exp = @"(\d)|(;)|(')|(\s)|(/)|(\|)|(\+)|(-)|(:)|(\()|(\))|(\[)|(\])|(>)|(<)|(,)|(\.)|(“)|(”)|(—)|(＋)|(：)|(\?)|(（)|(）)|(《)|(》)|(~)|(\!)|(_)|(#)|(\$)|(\%)|(\^)|(\&)|(\*)(\=)|(\\)|(、)|(。)|(，)|(？)|(，)|(／)";

        }
        if (!useMD5)
        {
            var s = Regex.Replace(input, exp, "");
            return s.ToLower();
        }
        else
        {
            var s = Regex.Replace(input, exp, "");
            return SecurityUtil.GetMD5String(s).ToLower();
        }
    }
}

