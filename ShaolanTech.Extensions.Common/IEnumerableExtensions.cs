using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;

public static class IEnumerableExtensions
{
    public static List<string>   AddTagIfNotNullOrEmpty(this List<string>source,string tag,string value)
    {
        if (value.IsNotNullOrEmpty())
        {
            source.Add($"<{tag}>{value}</{tag}>");
        }
        return source;
    }
    /// <summary>
    /// 对数据源进行分页读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="itemCountPerPage">每页元素数量</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>>ToPage<T>(this IEnumerable<T> source,int itemCountPerPage)
    {
        //var count = source.Count();
        //var page = count / itemCountPerPage;
        //if (count%itemCountPerPage!=0)
        //{
        //    page++;
        //}
        //for (int i = 0; i < length; i++)
        //{

        //}
        int page = 0;
        while (true)
        {
            var items = source.Skip(page * itemCountPerPage).Take(itemCountPerPage);
            if (items.Count()==0)
            {
                yield break;
            }
            yield return items;
            page++;
        }
    }
    /// <summary>
    /// 获取字符串数据的2元素全组合
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<List<string>> ComposeElement(this List<string> source)
    {

        

        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        for (int i = 0; i < source.Count; i++)
        {

            for (int j = 0; j < source.Count; j++)
            {
                if (i != j)
                {
                    SortedSet<string> set = new SortedSet<string>();
                    
                    set.Add(source[i]);
                    set.Add(source[j]);
                    var list = set.ToList();
                    if (dic.ContainsKey($"{list[0]}{list[1]}")==false)
                    {
                        dic.Add($"{list[0]}{list[1]}", list);
                    }
                }
            }
        }
        List<List<string>> result = new List<List<string>>();
        foreach (var item in dic)
        {
            result.Add(item.Value);
        }
        return result;
    }
   
    /// <summary>
    /// 当前集合是否为非NULL并且元素数量大于0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return !source.IsNullOrEmpty();
    }
    /// <summary>
    /// 当前集合是否为NULL或元素数量等于0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return source == null || source.Count() == 0;
         
    }
    
    /// <summary>
    /// 获取最后指定数量的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="count">获取数量</param>
    /// <returns></returns>
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
    {
        return source.Reverse<T>().Take(count).Reverse<T>();
    }
    /// <summary>
    /// 获取以符合指定数据后缀的元素
    /// </summary>
    /// <param name="source"></param>
    /// <param name="compare">指定数据后缀的元素</param>
    /// <returns></returns>
    public static List<string> IntersectEndWith(this List<string> source, List<string> compare)
    {
        return source.Where(s => compare.Any(c => s.AsSpan().EndsWith(c.AsSpan()))).ToList();
    }
    /// <summary>
    /// 获取数组的最短项，例如：["hello world","world"],将转化为["world"]
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<string> ToShortestForm(this List<string> source)
    {
        if (source.Count <= 1)
        {
            return source;
        }
        List<string> result = new List<string>();
        List<string> temp = new List<string>(source.OrderBy(s => s.Length));

        while (true)
        {
            if (temp.Count == 0)
            {
                break;
            }

            var first = temp[0];

            var containsItems = temp.Skip(1).Where(s => s.AsSpan().Contains(first.AsSpan(), StringComparison.OrdinalIgnoreCase)).ToArray();
            foreach (var item in containsItems)
            {
                temp.Remove(item);
            }
            temp.Remove(first);
            result.Add(first);

        }
        return result;
    }
    /// <summary>
    /// 连接字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="splitter">分割符</param>
    /// <param name="format">格式化回调函数</param>
    /// <returns></returns>
    public static string JoinString<T>(this IEnumerable<T> source, string splitter, Func<T, string> format = null)
    {
        if (format == null)
        {
            return string.Join(splitter, source);
        }
        else
        {
            return string.Join(splitter, source.Select(s => format(s)));
        }
    }
    /// <summary>
    /// 向列表中添加不重复元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="items"></param>
    /// <param name="conflictPredict"></param>
    /// <returns></returns>
    public static List<T> AddRangeNoConflict<T>(this List<T> source, IEnumerable<T> items, Func<T, object> conflictPredict)
    {
        Dictionary<object, byte> sourceDic = new Dictionary<object, byte>();
        foreach (var item in source)
        {
            var key = conflictPredict(item);
            if (sourceDic.ContainsKey(key)==false)
            {
                sourceDic.Add(key, 0);
            }
        }
        foreach (var item in items)
        {
            var key = conflictPredict(item);
            if (sourceDic.ContainsKey(key)==false)
            {
                source.Add(item);
                sourceDic.Add(key, 0);
            }
        }
        sourceDic.Clear();
        GC.Collect();
        return source;
    }
    /// <summary>
    /// 向列表中添加元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="item">要添加的项</param>
    /// <param name="conflictPredict">唯一键读取函数</param>
    /// <returns></returns>
    public static List<T> AddNoConflict<T>(this List<T> source,T item,Func<T,string>conflictPredict)
    {
        if (source.Any(s=>conflictPredict(s)==conflictPredict(item)))
        {

        }
        else
        {
            source.Add(item);
        }
        return source;
    }
    /// <summary>
    /// 向集合中添加元素，如果存在相同元素则不添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="items">新添加元素列表</param>
    public static List<T> AddRangeIfNotExists<T>(this List<T> source, IEnumerable<T> items)
    {
        if (items.IsNotNullOrEmpty())
        {
            foreach (var item in items)
            {
                source.AddIfNotExists(item);
            }
        }
        return source;
    }
    /// <summary>
    /// 向集合中添加元素，如果存在相同元素则不添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="item">新添加元素</param>
    public static List<string> AddIfNotExistsAndNotEmpty(this List<string> source, string item)
    {
        if (item.IsNotNullOrEmpty()&&source.Contains(item) == false)
        {
            source.Add(item);
        }
        return source;
    }
    /// <summary>
    /// 向集合中添加元素，如果存在相同元素则不添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="item">新添加元素</param>
    public static void AddIfNotExists<T>(this List<T> source, T item)
    {
        if (source.Contains(item) == false)
        {
            source.Add(item);
        }
    }
    /// <summary>
    /// 生成用于匹配的正则表达式
    /// </summary>
    /// <param name="source"></param>
    /// <param name="withAnyMatch">是否为任意匹配，默认为False，即全匹配</param>
    /// <returns></returns>
    public static string BuildMatchRegExpression(this IEnumerable<string> source, bool withAnyMatch = false)
    {
        string m = "";
        if (withAnyMatch)
        {
            m = ".*";
        }
        return string.Join("|", source.Select(s => $"({m}{s}{m})"));
    }
}

