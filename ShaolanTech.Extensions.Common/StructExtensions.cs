using System;
using System.Collections.Generic;
using System.Text;


public static class StructExtensions
{
    /// <summary>
    /// 获取当前值，如果为NULL则返回False
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool GetValue(this bool? value)
    {
        if (value.HasValue==false )
        {
            return false;
        }
        return value.Value;
    }
    public static bool IsFalse(this bool? value)
    {
        if (value.HasValue ==false )
        {
            return true;
        }
        if (value==false )
        {
            return true;
        }
        return false;
    }
    public static bool IsTrue(this bool? value)
    {
        return !value.IsFalse();
    }
}

