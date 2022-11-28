using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 时间扩展函数
/// </summary>
public static class TimeExtensions
{
    /// <summary>
    /// 等待时间段
    /// </summary>
    /// <param name="start">开始小时</param>
    /// <param name="length">距离开始时间的小时数</param>
    /// <returns></returns>
    public static async Task WaitTime(int start, int length)
    {
        while (true)
        {
            var now = DateTime.Now;
            var todayStart = new DateTime(now.Year, now.Month, now.Day, start, 0, 0);
            var todayEnd = todayStart + TimeSpan.FromHours(length);
            var yesterdayStart = todayStart - TimeSpan.FromDays(1);
            var yesterdayEnd = yesterdayStart + TimeSpan.FromHours(length);
            if ((DateTime.Now >= todayStart && DateTime.Now < todayEnd) || (DateTime.Now >= yesterdayStart && DateTime.Now < yesterdayEnd))
            {
                break;
            }
            else
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
    /// <summary>
    /// 指定的时间是否位置开始时间和结束时间之间
    /// </summary>
    /// <param name="now"></param>
    /// <param name="start">开始小时</param>
    /// <param name="hours">距离开始时间的小时数</param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime now,int start,int hours)
    {
        var todayStart = new DateTime(now.Year, now.Month, now.Day, start, 0, 0);
        var todayEnd = todayStart + TimeSpan.FromHours(hours);
        var yesterdayStart = todayStart - TimeSpan.FromDays(1);
        var yesterdayEnd = yesterdayStart + TimeSpan.FromHours(hours);
        if ((DateTime.Now >= todayStart && DateTime.Now < todayEnd) || (DateTime.Now >= yesterdayStart && DateTime.Now < yesterdayEnd))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 指定的时间是否位置开始时间和结束时间之间
    /// </summary>
    /// <param name="time"></param>
    /// <param name="start">开始时间</param>
    /// <param name="hours">距离开始时间的小时数</param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime time, DateTime start,int hours)
    {
        return time >= start && time <= start+TimeSpan.FromHours(hours);
    }
    /// <summary>
    /// 指定的时间是否位置开始时间和结束时间之间
    /// </summary>
    /// <param name="time"></param>
    /// <param name="start">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime time,DateTime start,DateTime end)
    {
        return time >= start && time <= end;
    }
    /// <summary>
    /// 将UTC时间戳转化为时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this long timeStamp)
    {
        return new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(timeStamp) + TimeSpan.FromHours(8);
    }
    /// <summary>
    /// 将当前时间转化为UTC时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ToTimeStamp(this DateTime time)
    {
        return (long)(time - new DateTime(1970, 1, 1) - TimeSpan.FromHours(8)).TotalMilliseconds;
    }
    /// <summary>
    /// 获取当前北京时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime BejingTime(this DateTime time)
    {
        
        return time.ToUniversalTime() + TimeSpan.FromHours(8);
    }
}

