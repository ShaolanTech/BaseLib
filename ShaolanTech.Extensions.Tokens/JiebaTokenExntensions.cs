using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class JiebaTokenExtension
{
    private static JiebaSegmenter segmenter = new JiebaSegmenter();
    
    /// <summary>
    /// 利用结巴分词词库，获取当前的字符的分词形式
    /// </summary>
    /// <param name="s"></param>
    /// <param name="webSearch">是否为Web搜索创建分词，如果为否，则返回精准分词</param>
    /// <returns></returns>
    public static string GetJiebaToken(this string s, bool webSearch = true)
    {
        if (s.HasChineseChars())
        {
            var result = s;
            if (webSearch)
            {
                var segments = segmenter.CutForSearch(s);
                result = string.Join(" ", segments.Select(ss => ss.Trim()).Where(ss => ss.Length <= 200));
            }
            else
            {
                var segments = segmenter.Cut(s);
                result = string.Join(" ", segments.Select(ss => ss.Trim()).Where(ss => ss.Length <= 200));
            }
            result = result.Replace(",", " ").Replace(".", " ").RemoveMultipleSpaces();
            return result;
        }
        else
        {
            var tokens = s.RemoveMultipleSpaces().SplitList(" ").Where(ss => ss.Length <= 200);
            return string.Join(" ", tokens);
        }

    }

    /// <summary>
    /// 替换字符之间的非字母字符
    /// </summary>
    /// <param name="s"></param>
    /// <param name="toLowerCase"></param>
    /// <returns></returns>
    public static string ClearToken(this string s)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            if ((s[i] >= 0x4E00 && s[i] <= 0x9FA5) || (s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9') || s[i] == '〇')
            {
                sb.Append(s[i]);
            }
            else
            {
                sb.Append(' ');
            }
        }

        return sb.ToString();

    }
    /// <summary>
    /// 利用结巴分词词库，获取当前的字符的分词形式（忽略长度大于200个字符的分词）
    /// </summary>
    /// <param name="s"></param>
    /// <param name="webSearch">是否为Web搜索创建分词，如果为否，则返回精准分词</param>
    /// <returns></returns>
    public static List<string> GetJiebaTokens(this string s, bool webSearch = true)
    {
        if (s.HasChineseChars())
        {
            List<string> result = new List<string>();

            if (webSearch)
            {
                var segments = segmenter.CutForSearch(s);
                result = segments.ToList();
            }
            else
            {
                var segments = segmenter.Cut(s);
                result = segments.ToList();
            }
            return result.Where(t => t.Trim().IsNotNullOrEmpty()).Where(t => t.Length <= 200).ToList();
        }
        else
        {

            return s.ClearToken().RemoveMultipleSpaces().SplitList(" ").Where(t => t.Length <= 200).ToList();
        }

    }
}

