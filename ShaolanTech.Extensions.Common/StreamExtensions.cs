using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public static class StreamExtensions
{
    /// <summary>
    /// 执行流复制
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest">目标流</param>
    /// <param name="size">复制长度</param>
    /// <param name="callback">用于回显示的回调函数</param>
    public static void CopyToStream(this Stream source, Stream dest, long size, Action<double> callback = null)
    {
        int take = 10000;
        var page = size / take;
        if (size % take != 0)
        {
            page++;
        }
        long last = size % take;
        long total = 0;
        for (int i = 0; i < page; i++)
        {
            int copyLength = take;
            if (i == page - 1)
            {
                copyLength = (int)last;
            }
            source.CopyTo(dest, copyLength);
            total += copyLength;
            if (callback != null)
            {
                callback(total * 100d / size);
            }
        }

    }



    public static bool EOF(this BinaryReader binaryReader)
    {
        var bs = binaryReader.BaseStream;
        return (bs.Position == bs.Length);
    }

}

