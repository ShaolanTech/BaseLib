using ShaolanTech;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 字符串扩展函数
/// </summary>
public static class StringExtensions
{
    public static ReadOnlySpan<byte> ToReadOnlySpan(this string input)
    {
        
        var buffer = Encoding.UTF8.GetBytes(input);
        return new ReadOnlySpan<byte>(buffer);
    }
    public static string ToPascalCase(this string s)
    {
        var tokens = s.SplitList(" ");
       
        StringBuilder sb = new StringBuilder();
        foreach (var token in tokens)
        {
            if (token.Length>1)
            {
               
                sb.Append(char.ToUpper(token[0])).Append(token.Substring(1).ToLower()).Append(" ");
            }
            else
            {
                sb.Append(token.ToUpper()).Append(" ");
            }
        }
        return sb.RemoveLast().ToString();
    }
    /// <summary>
    /// 使用字典进行字符串替换
    /// </summary>
    /// <param name="s"></param>
    /// <param name="dic">要替换的字典</param>
    /// <returns></returns>
    public static string Replace(this string s, Dictionary<string, string> dic)
    {
        StringBuilder sb = new StringBuilder(s);
        foreach (var item in dic)
        {
            sb.Replace(item.Key, item.Value);
        }
        return sb.ToString();
    }
    /// <summary>
    /// 转化为计算机表达的长度字符串形式
    /// </summary>
    /// <param name="Size"></param>
    /// <returns></returns>
    public static string FormatLength(this long Size)
    {
        string m_strSize = "";
        long FactSize = 0;
        FactSize = Size;
        if (FactSize < 1024.00)
            m_strSize = FactSize.ToString("F2") + " Byte";
        else if (FactSize >= 1024.00 && FactSize < 1048576)
            m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
        else if (FactSize >= 1048576 && FactSize < 1073741824)
            m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
        else if (FactSize >= 1073741824)
            m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
        return m_strSize;
    }
    /// <summary>
    /// 将Excel的列表达形式转化为从1开始的索引
    /// </summary>
    /// <param name="s">0表示处理异常</param>
    /// <returns></returns>
    public static int ExcelColumnToIndex(this string s)
    {
        string upperCase = s.ToUpper();
        if (string.IsNullOrEmpty(s)) return 0;
        int n = 0;
        for (int i = s.Length - 1, j = 1; i >= 0; i--, j *= 26)
        {
            char c = Char.ToUpper(s[i]);
            if (c < 'A' || c > 'Z') return 0;
            n += ((int)c - 64) * j;
        }
        return n;
    }
    /// <summary>
    /// 返回包括当前字符串的StringBuilder
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static StringBuilder Append(this string s, string newContent)
    {
        return new StringBuilder(s).Append(newContent);
    }
   
    
    /// <summary>
    /// 获取当前字符串MD5后前15位表示的正长整数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static long ToUniquePositiveNumber(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return 0;
        }
        var id = (long)(System.Convert.ToUInt64($"0x{SecurityUtil.GetMD5String(s).Substring(0, 16)}", 16) >> 1);
        return id;
    }
     
    /// <summary>
    /// 尝试将字符串转化为整型数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static long TryParseLong(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return 0;
        }
        long.TryParse(s, out long result);
        return result;
    }
    /// <summary>
    /// 尝试将字符串转化为单精度浮点数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static float TryParseFloat(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return 0;
        }
        float.TryParse(s, out float result);
        return result;
    }
    /// <summary>
    /// 尝试将字符串转化为双精度浮点数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static double TryParseDouble(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return 0;
        }
        double.TryParse(s, out double result);
        return result;
    }
    /// <summary>
    /// 尝试将输入字符串转化为日期类型
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static DateTime TryParseDateTime(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return new DateTime(1900, 1, 1);
        }
        DateTime.TryParse(s, out DateTime result);
        return result;
    }
    /// <summary>
    /// 尝试将字符串转化为整型数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int TryParseInt(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return 0;
        }
        int.TryParse(s, out int result);
        return result;
    }
    public static bool TryParseBoolean(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return false;
        }
        bool.TryParse(s, out bool result);
        return result;
    }
     
    

    /// <summary>
    /// 当前字符串是否为NULL或空串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string s)
    {
        if (s == null || s.Trim() == "")
        {
            return true;
        }
        return false;

    }
    /// <summary>
    /// 字符串是否不为NULL或空串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsNotNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s) == false;
    }
    /// <summary>
    /// 如果当前字符串不为空，则调用notNullCallback，并返回其结果，否则返回空串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="notNullCallback"></param>
    /// <returns></returns>
    public static string IfNotNull(this string s, Func<string> notNullCallback)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        return notNullCallback();
    }
    
    /// <summary>
    /// 是否全部字符都是数字
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool AllCharAreNumbers(this string s)
    {
        return s.All(c => c >= '0' && c <= '9');
    }
    
    /// <summary>
    /// 是否以任意后缀结束
    /// </summary>
    /// <param name="s"></param>
    /// <param name="sources">后缀列表</param>
    /// <returns></returns>
    public static bool EndsWithAny(this string s, params string[] sources)
    {
        foreach (var item in sources)
        {
            if (s.EndsWith(item))
            {
                return true;
            }
        }
        return false;
    }
    
    
    /// <summary>
    /// 是否包括任意子字符串
    /// </summary>
    /// <param name="s">当前字符串</param> 
    /// <param name="subs">子字符串集合</param>
    /// <returns></returns>
    public static bool ContainsAny(this string s, IEnumerable<string> subs)
    {
        if (s == null || s.Length == 0 || subs == null || subs.Count() == 0)
        {
            return false;
        }
        foreach (var item in subs)
        {
            if (s.AsSpan().Contains(item.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 是否包括全部子字符串
    /// </summary>
    /// <param name="s">当前字符串</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <param name="subs">子字符串集合</param>
    /// <returns></returns>
    public static bool ContainsAll(this string s, bool ignoreCase = true, params string[] subs)
    {
        if (s == null || s.Length == 0 || subs == null || subs.Length == 0)
        {
            return false;
        }
        
        foreach (var item in subs)
        {
            if (s.AsSpan().Contains(item.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                 
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    private static bool MatchStart(ReadOnlySpan<char> span, int index, ReadOnlySpan<char> item)
    {

        if (index == 0)
        {
            return true;
        }
        var startChar = span[index - 1];
        if ((startChar >= 'a' && startChar <= 'z') || (startChar >= 'A' && startChar <= 'Z'))
        {
            return false;
        }
        return true;
    }
    private static bool MatchEnd(ReadOnlySpan<char> span, int index, ReadOnlySpan<char> item)
    {

        if (index == span.Length - item.Length)
        {
            return true;
        }
        var endChar = span[index + item.Length];
        if ((endChar >= 'a' && endChar <= 'z') || (endChar >= 'A' && endChar <= 'Z'))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 是否匹配全部英文单词
    /// </summary>
    /// <param name="s"></param>
    /// <param name="subs"></param>
    /// <returns></returns>
    public static bool MatchAllEnglishWords(this string s, List<string> subs)
    {
        return MatchAllEnglishWords(s, subs.ToArray());
    }
    /// <summary>
    /// 是否匹配全部英文单词
    /// </summary>
    /// <param name="s"></param>
    /// <param name="subs"></param>
    /// <returns></returns>
    public static bool MatchAllEnglishWords(this string s, params string[] subs)
    {
        if (s == null || s.Length == 0 || subs == null || subs.Length == 0)
        {
            return false;
        }
        var span = s.AsSpan();
        foreach (var item in subs)
        {
            if (item.IsNotNullOrEmpty())
            {
                var itemSpan = item.AsSpan();
                var index = span.IndexOf(itemSpan, StringComparison.OrdinalIgnoreCase);
                if (index == -1)
                {
                    return false;
                }
                else
                {
                    if (!MatchStart(span, index, itemSpan) || !MatchEnd(span, index, itemSpan))
                    {
                        return false;
                    }
                }
            }

        }
        return true;
    }
    /// <summary>
    /// 是否匹配任意英文单词
    /// </summary>
    /// <param name="s"></param>
    /// <param name="subs"></param>
    /// <returns></returns>
    public static bool MatchAnyEnglishWords(this string s, List<string> subs)
    {
        return MatchAnyEnglishWords(s, subs.ToArray());
    }
    /// <summary>
    /// 是否匹配任意英文单词
    /// </summary>
    /// <param name="s"></param>
    /// <param name="subs"></param>
    /// <returns></returns>
    public static bool MatchAnyEnglishWords(this string s, params string[] subs)
    {
        if (s == null || s.Length == 0 || subs == null || subs.Length == 0)
        {
            return false;
        }
        var span = s.AsSpan();
        foreach (var item in subs)
        {
            if (item.IsNotNullOrEmpty())
            {
                var itemSpan = item.AsSpan();
                var index = span.IndexOf(itemSpan, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    if (MatchStart(span, index, itemSpan) && MatchEnd(span, index, itemSpan))
                    {
                        return true;
                    }
                }

            }

        }
        return false;
    }
    
     
    /// <summary>
    /// 是否按顺序包含子串数组（忽略大小写）
    /// </summary>
    /// <param name="s"></param>
    /// <param name="subStrings">子串数组</param>
    /// <param name="reverse">子串是否需要反序</param>
    /// <returns></returns>
    public static bool SquenceContains(this string s, List<string> subStrings, bool reverse = false)
    {
        bool result = false;

        if (s.IsNotNullOrEmpty())
        {
            var lower = s.ToLower().AsSpan();

            int matchCount = 0;
            if (!reverse)
            {
                for (int i = 0; i < subStrings.Count; i++)
                {
                    var pos = lower.IndexOf(subStrings[i].AsSpan(), StringComparison.OrdinalIgnoreCase);
                    if (pos != -1)
                    {
                        matchCount++;
                        lower = lower.Slice(pos + subStrings[i].Length);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = subStrings.Count - 1; i >= 0; i--)
                {
                    var pos = lower.IndexOf(subStrings[i].AsSpan(), StringComparison.OrdinalIgnoreCase);
                    if (pos != -1)
                    {
                        matchCount++;
                        lower = lower.Slice(pos + subStrings[i].Length);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (matchCount == subStrings.Count)
            {
                result = true;
            }
        }
        return result;
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




#region 字符串清理函数
   

    private static bool InRange(ReadOnlySpan<char> span, int index, CharRange[] ranges)
    {
        for (int i = 0; i < ranges.Length; i++)
        {
            if (span[i] >= ranges[i].Start && span[i] <= ranges[i].End)
            {
                return true;
            }
        }
        return false;
    }
    private static bool InRange(ReadOnlySpan<char> span, int index, char[] ranges)
    {
        var current = span[index];
        return ranges.Any(c => c == current);
    }
    /// <summary>
    /// 通过内存指针方式获取纯净字符串（默认只包括中文及英文字符）
    /// </summary>
    /// <param name="s">当前字符串</param>
    /// <param name="ranges">除了中文及英文字符以外，需要满足的字符范围</param>
    /// <returns></returns>
    public static string AsSpanClearText(this string s, params char[] ranges)
    {
        if (s == null || s.Length == 0)
        {
            return "";
        }
        var span = s.AsSpan();
        if (ranges == null)
        {
            ranges = new char[0];
        }
        var arr = new char[s.Length];
        int arrIdx = 0;
        for (int idx = 0; idx < span.Length; idx++)
        {
            if (((span[idx] >= 0x4e00 && span[idx] <= 0x9fbb) || (span[idx] >= 65 && span[idx] <= 90) || (span[idx] >= 97 && span[idx] <= 122) || InRange(span, idx, ranges)))
            {
                arr[arrIdx] = span[idx];
                arrIdx++;
            }
        }
        return new string(arr, 0, arrIdx);
    }

    /// <summary>
    /// 通过内存指针方式获取纯净字符串（默认只包括中文及英文字符）
    /// </summary>
    /// <param name="s">当前字符串</param>
    /// <param name="ranges">除了中文及英文字符以外，需要满足的字符范围</param>
    /// <returns></returns>
    public static string AsSpanClearText(this string s, params CharRange[] ranges)
    {
        if (s == null || s.Length == 0)
        {
            return "";
        }
        var span = s.AsSpan();
        if (ranges == null)
        {
            ranges = new CharRange[0];
        }
        var arr = new char[s.Length];
        int arrIdx = 0;
        for (int idx = 0; idx < span.Length; idx++)
        {
            if (((span[idx] >= 0x4e00 && span[idx] <= 0x9fbb) || (span[idx] >= 65 && span[idx] <= 90) || (span[idx] >= 97 && span[idx] <= 122) || InRange(span, idx, ranges)))
            {
                arr[arrIdx] = span[idx];
                arrIdx++;
            }
        }
        return new string(arr, 0, arrIdx);
    }
    
    public static string ClearText(this string s, bool withNumber, bool replaceToSpace)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        
        Dictionary<char, byte> specialDic = new Dictionary<char, byte>()
        {
            { '〇',0 },{ 'Ⅰ',0 },{ 'Ⅱ',0 },{ 'Ⅲ',0 }
        };
        var span = s.AsSpan();
        var arr = new char[s.Length];
        int arrIdx = 0;
        for (int idx = 0; idx < span.Length; idx++)
        {
            if (withNumber)
            {
                if ((span[idx] >= 0x4e00 && span[idx] <= 0x9fbb) || (span[idx] >= 65 && span[idx] <= 90) || (span[idx] >= 97 && span[idx] <= 122) || (span[idx] >= '0' && span[idx] <= '9')|| specialDic.ContainsKey(span[idx]) )
                {
                    arr[arrIdx] = span[idx];
                    arrIdx++;
                }
                else
                {
                    if (replaceToSpace)
                    {
                        if (arrIdx > 0 && arr[arrIdx - 1] != ' ')
                        {
                            arr[arrIdx] = ' ';
                            arrIdx++;
                        }
                    }

                }
            }
            else
            {
                if (((span[idx] >= 0x4e00 && span[idx] <= 0x9fbb) || (span[idx] >= 65 && span[idx] <= 90) || (span[idx] >= 97 && span[idx] <= 122)))
                {
                    arr[arrIdx] = span[idx];
                    arrIdx++;
                }
                else
                {
                    if (replaceToSpace)
                    {
                        if (arrIdx > 0 && arr[arrIdx - 1] != ' ')
                        {
                            arr[arrIdx] = ' ';
                            arrIdx++;
                        }
                    }

                }
            }
        }
        return new string(arr, 0, arrIdx).Trim().ToLower();

    }
    
    /// <summary>
    /// 将非中文、字母、数字字符替换为空格
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ReplaceNonWordCharToSpace(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            if ((s[i] >= 0x4E00 && s[i] <= 0x9FA5) || (s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9'))
            {
                sb.Append(s[i]);
            }
            else
            {
                sb.Append(" ");
            }
        }
        return sb.ToString();

    }
    /// <summary>
    /// 移除字符串中的数字
    /// </summary>
    /// <param name="s"></param>
    /// <param name="length">移除数量，默认为0，移除全部数字</param>
    /// <returns></returns>
    public static string RemoveNumbers(this string s, int length = 0)
    {
        var numbers = s.GetIntNumbers();
        if (numbers.Count == 0)
        {
            return s;
        }
        if (length == 0)
        {
            return s.Remove(numbers.Select(m => m.ToString()).ToArray());
        }
        else
        {
            string result = s;
            for (int i = 0; i < length; i++)
            {
                result = result.Remove(numbers[i].ToString());
            }
            return result;
        }
    }
    /// <summary>
    /// 将字符串数组附加到缓冲区
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="parts">字符串数组</param>
    /// <returns></returns>
    public static StringBuilder AppendRange(this StringBuilder sb, params string[] parts)
    {
        foreach (var item in parts)
        {
            sb.Append(item);
        }
        return sb;
    }
    /// <summary>
    /// 拼接字符串
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    public static string BuildString(this StringBuilder sb, params string[] parts)
    {

        foreach (var item in parts)
        {
            sb.Append(item);
        }
        return sb.ToString();
    }
    /// <summary>
    /// 移除StringBuilder中指定长度的末尾元素
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="length">移除长度默认为1</param>
    /// <returns></returns>
    public static StringBuilder RemoveLast(this StringBuilder sb, int length = 1)
    {
        if (sb.Length != 0)
        {
            sb.Remove(sb.Length - length, length);
        }

        return sb;
    }

    /// <summary>
    /// 当满足指定条件时执行附加操作
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="checkIfExp">条件检测表达式</param>
    /// <param name="s">要附加的字符串</param>
    /// <returns></returns>
    public static StringBuilder AppendIf(this StringBuilder sb, Func<bool> checkIfExp, string s)
    {
        if (checkIfExp())
        {
            sb.Append(s);
        }
        return sb;
    }
    /// <summary>
    /// 当满足指定条件时执行附加操作
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="checkIfExp">条件检测表达式</param>
    /// <param name="getText">要附加的字符串</param>
    /// <returns></returns>
    public static StringBuilder AppendIf(this StringBuilder sb, Func<bool> checkIfExp, Func<string> getText)
    {
        if (checkIfExp())
        {
            sb.Append(getText());
        }
        return sb;
    }
    private static Dictionary<string, string> htmlChars = new Dictionary<string, string>()
        {

             {"&ensp;","" }, {"&#8194;","" },
             {"&emsp;","" }, {"&#8195;","" },
             {"&nbsp;"," " }, {"&#160;"," " },
             {"&lt;","<" }, {"&#60;","<" },
             {"&gt;",">" }, {"&#62;",">" },
             {"&amp;","&" }, {"&#38;","&" },
             {"&quot;","\"" }, {"&#34;","\"" },
             {"&copy;","" }, {"&#169;","" },
             {"&times;","×" }, {"&#215;","×" }
        };
    /// <summary>
    /// 移除字符串的HTML转义字符
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string RemoveHtmlChars(this string input)
    {
        if (input.IsNullOrEmpty())
        {
            return "";
        }


        foreach (var item in htmlChars)
        {
            input = input.AsSpanReplace(item.Key, item.Value);
        }
        return input;
    }

    /// <summary>
    /// 移除开头的0
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string RemoveStartZero(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }

        string result = s;
        if (result[0] == '0')
        {
            StringBuilder sb = new StringBuilder(result);
            while (sb.Length > 0 && sb[0] == '0')
            {
                sb.Remove(0, 1);
            }
            result = sb.ToString();
        }
        return result;
    }

    /// <summary>
    /// 移除括号
    /// </summary>
    /// <param name="s"></param>
    /// <param name="replaceChar">替换为的字符</param>
    /// <param name="others"></param>
    /// <returns></returns>
    public static string ReplaceQuate(this string s, string replaceChar, params string[] others)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        bool skip = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (!skip)
            {
                if (s[i] != '[' && s[i] != '(')
                {

                    sb.Append(s[i]);


                }
                else
                {
                    sb.Append(replaceChar);
                    skip = true;
                }
            }
            else
            {
                if (s[i] == ']' || s[i] == ')')
                {
                    skip = false;
                }
            }

        }
        if (others != null)
        {
            foreach (var item in others)
            {
                sb.Replace(item, replaceChar);
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// 移除字符串中的括号
    /// </summary>
    /// <param name="s"></param>
    /// <param name="replaceChar">替换为的字符</param>
    /// <param name="others"></param>
    /// <returns></returns>
    public static string ReplaceQuate(this string s, char replaceChar, params char[] others)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        bool skip = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (!skip)
            {
                if (s[i] != '[' && s[i] != '(')
                {

                    sb.Append(s[i]);


                }
                else
                {
                    sb.Append(replaceChar);
                    skip = true;
                }
            }
            else
            {
                if (s[i] == ']' || s[i] == ')')
                {
                    skip = false;
                }
            }

        }
        if (others != null)
        {
            foreach (var item in others)
            {
                var sb1 = sb.Replace(item, replaceChar);
            }
        }
        return sb.ToString();
    }
    public static bool IsSpace(this char c)
    {
        return c == 32 || c == 160;
    }
    /// <summary>
    /// 移除连续空格，只保留一个
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string RemoveMultipleSpaces(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(s[0]);
        for (int i = 1; i < s.Length; i++)
        {
            if (s[i] .IsSpace() && s[i - 1] .IsSpace())
            {
                continue;
            }
            else
            {
                sb.Append(s[i]);
            }
        }
        return sb.ToString();
    }

#endregion

#region 正则表达式操作

    /// <summary>
    /// 移除满足正则表达式的的字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正则表达式</param>
    /// <param name="options">正规表达式选项</param>
    /// <returns></returns>
    public static string RemoveMatch(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return Regex.Replace(s, pattern, "", options);
    }
    /// <summary>
    /// 将满足正则表达式的字符串替换为指定字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正则表达式</param>
    /// <param name="value">要替换的值</param>
    /// <param name="options">正规表达式选项</param>
    /// <returns></returns>
    public static string ReplaceMatch(this string s, string pattern, string value, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return Regex.Replace(s, pattern, value, options);
    }
    /// <summary>
    /// 获取单个配置正则表达式的字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正规表达式</param>
    /// <param name="options">正规表达式选项</param>
    /// <returns></returns>
    public static string GetMatch(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        if (s.IsNullOrEmpty() || pattern.IsNullOrEmpty())
        {
            return "";
        }
        var match = Regex.Match(s, pattern, options);
        if (match.Success)
        {
            return match.Value;
        }

        return "";
    }
    /// <summary>
    /// 获取全部配置正则表达式的字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正规表达式</param>
    /// <param name="options">正规表达式选项</param>
    /// <returns></returns>
    public static string[] GetMatches(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {

        var matches = Regex.Matches(s, pattern, options);
        return matches.Cast<Match>().Select(m => m.Value).ToArray();
    }
    /// <summary>
    /// 是包含单个文字（前后为非字母字符）
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">文字</param>
    /// <param name="options">表达式选项，默认为忽略大小写</param>
    /// <returns></returns>
    public static bool IsMathSingleWord(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        if (s.IsNullOrEmpty())
        {
            return false;
        }
        bool result = false;

        if (s.AsSpan().Contains(pattern.AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            result = Regex.IsMatch(s, $"(^|\\W){pattern}($|\\W)", options);
        }
        return result;
    }
    /// <summary>
    /// 是否不满足指定的正则表达式
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正则表达式</param>
    /// <param name="options">表达式选项，默认为忽略大小写</param>
    /// <returns></returns>
    public static bool IsNotMatch(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return !s.IsMatch(pattern, options);
    }
    /// <summary>
    /// 是否满足指定的正则表达式
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern">正则表达式</param>
    /// <param name="options">表达式选项，默认为忽略大小写</param>
    /// <returns></returns>
    public static bool IsMatch(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        if (s.IsNullOrEmpty())
        {
            return false;
        }
        return Regex.IsMatch(s, pattern, options);
    }
#endregion


    public static string AsSpanReplace(this string s, string source, string desc)
    {
        var strSpan = s.AsSpan();

        var splitSapn = source.AsSpan();

        int m = 0, n = 0;


        List<string> arr = new List<string>();

        while (true)
        {
            m = n;
            n = strSpan.IndexOf(splitSapn, StringComparison.OrdinalIgnoreCase);
            if (n > -1)
            {
                arr.Add(strSpan.Slice(0, n).ToString());
                strSpan = strSpan.Slice(n + splitSapn.Length);
            }
            else
            {
                arr.Add(strSpan.ToString());
                break;
            }
        }
        return string.Join(desc, arr);

    }

    public static ReadOnlySpan<char> AsSpanReplace(ReadOnlySpan<char> strSpan, ReadOnlySpan<char> splitSapn, string desc)
    {


        int m = 0, n = 0;


        List<string> arr = new List<string>();

        while (true)
        {
            m = n;
            n = strSpan.IndexOf(splitSapn);
            if (n > -1)
            {
                arr.Add(strSpan.Slice(0, n).ToString());
                strSpan = strSpan.Slice(n + splitSapn.Length);
            }
            else
            {
                arr.Add(strSpan.ToString());
                break;
            }
        }
        return string.Join(desc, arr).AsSpan();

    }
    /// <summary>
    /// 将指定字符替换为空格
    /// </summary>
    /// <param name="s"></param>
    /// <param name="letters">指定元素数组</param>
    /// <returns></returns>
    public static string ReplaceToSpace(this string s, params char[] letters)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }

        StringBuilder sb = new StringBuilder(s);

        foreach (var item in letters)
        {
            sb.Replace(item, ' ');
        }
        return sb.ToString();
    }
    /// <summary>
    /// 移除指定的字符
    /// </summary>
    /// <param name="s"></param>
    /// <param name="letters">指定元素数组</param>
    /// <returns></returns>
    public static string RemoveChars(this string s, params char[] letters)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < s.Length; i++)
        {
            if (!letters.Contains(s[i]))
            {
                sb.Append(s[i]);
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// 移除指定的字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="letters">指定元素数组</param>
    /// <returns></returns>
    public static string Remove(this string s, params string[] letters)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder(s);
        foreach (var item in letters.Where(l => l.IsNotNullOrEmpty()))
        {
            sb = sb.Replace(item, "");
        }
        return sb.ToString();
    }
    /// <summary>
    /// 移除指定的字符串
    /// </summary>
    /// <param name="s"></param>
    /// <param name="letters">指定元素数组</param>
    /// <returns></returns>
    public static string RemoveLast(this string s)
    {
        if (s.IsNullOrEmpty())
        {
            return "";
        }
        return s.Substring(0, s.Length - 1);
    }

    /// <summary>
    /// 获取字符串中所有连续整数
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static List<int> GetIntNumbers(this string s)
    {
        List<int> result = new List<int>();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] >= '0' && s[i] <= '9')
            {
                sb.Append(s[i]);
            }
            else
            {
                if (sb.Length != 0)
                {
                    result.Add(int.Parse(sb.ToString()));
                    sb.Clear();
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 通过正则表达式对字符串进行分割
    /// </summary>
    /// <param name="s"></param>
    /// <param name="splitterPattern">用于分割的正则表达式</param>
    /// <returns></returns>
    public static List<string> SplitListRegex(this string s, string splitterPattern)
    {
        if (s.IsNullOrEmpty())
        {
            return new List<string>();
        }
        return Regex.Split(s, splitterPattern, RegexOptions.IgnoreCase).Where(s1 => s1.Trim().IsNotNullOrEmpty()).Select(s1 => s1.Trim()).ToList();
    }
    /// <summary>
    /// 通过非文字字符（非汉字，非英文字符）对字符串进行分割
    /// </summary>
    /// <param name="s"></param>
    /// <param name="toLower">是否转化为小写</param>
    /// <returns></returns>
    public static List<string> SplitListWithNONELetterChar(this string s, bool toLower = false)
    {
        List<string> result = new List<string>();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            if ((s[i] >= 0x4E00 && s[i] <= 0x9FA5) || (s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z'))
            {
                sb.Append(s[i]);
            }
            else
            {
                if (sb.Length != 0)
                {
                    if (!toLower)
                    {
                        result.Add(sb.ToString());
                    }
                    else
                    {
                        result.Add(sb.ToString().ToLower());
                    }
                    sb.Clear();
                }
            }
        }
        if (sb.Length != 0)
        {
            if (!toLower)
            {
                result.Add(sb.ToString());
            }
            else
            {
                result.Add(sb.ToString().ToLower());
            }
        }

        return result;
    }

    public static List<string>SplitLines(this string s)
    {
        List<string> result = new List<string>();
        if (s.IsNotNullOrEmpty())
        {
            using (StringReader sr=new StringReader(s))
            {
                var line = sr.ReadLine();
                while (line!=null)
                {
                    result.Add(line);
                    line = sr.ReadLine();
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 通过指定分割符对字符串进行分割，并且Trim（）
    /// </summary>
    /// <param name="s"></param>
    /// <param name="splitter">分割符</param>
    /// <returns></returns>
    public static List<string> SplitList(this string s, string splitter)
    {
        if (s.IsNullOrEmpty())
        {
            return new List<string>();
        }


        var strSpan = s.AsSpan();

        var splitSapn = splitter.AsSpan();

        int m = 0, n = 0;


        List<string> arr = new List<string>();

        while (true)
        {
            m = n;
            n = strSpan.IndexOf(splitSapn);
            if (n > -1)
            {
                var spTemp = strSpan.Slice(0, n).Trim();
                
                if (spTemp.Length != 0)
                {

                    arr.Add(spTemp.ToString());

                }

                strSpan = strSpan.Slice(n + splitSapn.Length);
            }
            else
            {
                var rest = strSpan.Trim().ToString();
                if (rest.Length != 0)
                {
                    arr.Add(rest);
                }
                break;
            }
        }
        return arr;

    }
    /// <summary>
    /// 通过指定分割符对字符串进行分割，并且Trim（）
    /// </summary>
    /// <param name="s"></param>
    /// <param name="splitter">分割符</param>
    /// <returns></returns>
    public static List<string> SplitList(this string s, char splitter)
    {
        if (s.IsNullOrEmpty())
        {
            return new List<string>();
        }
        List<string> result = new List<string>();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != splitter)
            {
                sb.Append(s[i]);
            }
            else
            {
                var part = sb.ToString().Trim();
                if (part.IsNotNullOrEmpty())
                {
                    result.Add(part);
                }
                sb.Clear();
            }
        }
        if (sb.Length != 0)
        {
            var part = sb.ToString().Trim();
            if (part.IsNotNullOrEmpty())
            {
                result.Add(part);
            }
        }
        return result;
    }

    /// <summary>
    /// 判定当前字符串是否为数字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsNumberString(this string text)
    {

        return double.TryParse(text.Trim(), out double n);
    }
    /// <summary>
    /// 当前字符是否为字母或汉字
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsWordChar(this char c)
    {
        return (c >= 0x4E00 && c <= 0x9FA5) || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
    /// <summary>
    /// 判断文字是否为中文
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsChinese(this string text)
    {
        if (text.IsNullOrEmpty())
        {
            return false;
        }
        var chineseChars = text.Count(c => c.IsChinese());
        var englishChars = text.Count(c => c.IsChinese() == false);
        return chineseChars > 0;

    }
    /// <summary>
    /// 当前字符是否为中文
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsChinese(this char c)
    {
        return c >= 0x4e00 && c <= 0x9fbb;
    }
    /// <summary>
    /// 当前字符串是否包括中文
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool HasChineseChars(this string text)
    {
        if (text.IsNullOrEmpty())
        {
            return false;
        }
        return text.Any(a => a.IsChinese())||text.Contains("〇");
    }
    /// <summary>
    /// 是否为中国的邮编
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsChineseZipCode(this string text)
    {
        if (text.Length != 6)
        {
            return false;
        }
        bool ok = true;
        for (int i = 0; i < text.Length; i++)
        {
            if (!(text[i] >= '0' && text[i] <= '9'))
            {
                ok = false;
                break;
            }
        }
        return ok;
    }

    public static string ToBase64String(this string content,Encoding encoding=null)
    {
        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }
        if (content.IsNullOrEmpty())
        {
            return "";
        }
        return Convert.ToBase64String(encoding.GetBytes(content));
    }
    /// <summary>
    /// 将Base64字符串转化为字符串
    /// </summary>
    /// <param name="content">字符内容</param>
    /// <param name="encoding">字符编码</param>
    /// <returns></returns>
    public static string FromBase64String(this string content, Encoding encoding = null)
    {
        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }
        var bytes = Convert.FromBase64String(content);

        var result = encoding.GetString(bytes, 0, bytes.Length);

        return result;
    }
    /// <summary>
    /// 将Base64字符串转化为字符串
    /// </summary>
    /// <param name="content">字符内容</param>
    /// <param name="encoding">字符编码</param>
    /// <returns></returns>
    public static string ToBase64Format(this string content, Encoding encoding = null)
    {
        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }
        var bytes = Convert.FromBase64String(content);

        var result = encoding.GetString(bytes, 0, bytes.Length);

        return result;
    }


    public static string ToHexString(this string input)
    {
        
#if NETSTANDARD2_0
        var bytes = Encoding.UTF8.GetBytes(input);
        return bytes.ToHexString();
#endif
#if NET5_0_OR_GREATER
        return Convert.ToHexString(input.ToReadOnlySpan());
#endif
    }
   
    public static string ToHexString(this byte[] input)
    {
        if (input == null)
            return null;
        StringBuilder output = new StringBuilder(input.Length * 2);
        for (int i = 0; i < input.Length; i++)
        {
            int current = input[i] & 0xff;
            if (current < 16)
                output.Append('0');
            output.Append(current.ToString("x"));
        }

        return output.ToString();
    }

    public static string FromHexString(this string input)
    {

#if NETSTANDARD2_0
       var bytes = HexStringToBytes(input);
       return Encoding.UTF8.GetString(bytes);
#endif
#if NET5_0_OR_GREATER
        var bytes = Convert.FromHexString(input);
        return Encoding.UTF8.GetString(bytes);
#endif
    }
    /// <summary>
    /// 将16进制字符串转化为Bytes
    /// </summary>
    /// <param name="hex">16进制字符串</param>
    /// <returns></returns>
    public static byte[] HexStringToBytes(this string hex)
    {
        if (hex.Length == 0)
        {
            return new byte[] { 0 };
        }

        if (hex.Length % 2 == 1)
        {
            hex = "0" + hex;
        }

        byte[] result = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length / 2; i++)
        {
            result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        return result;
    }




}
//}
