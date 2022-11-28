using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Warensoft.EntLib.Common;
using System.Linq;
using ShaolanTech.BaseModels;

public static class ADOExtensions
{
    public static void Dispose(this ResultInfo<DataSet> ds)
    {
        if (ds.AdditionalData!=null)
        {
            foreach (DataTable table in ds.AdditionalData.Tables)
            {
                table.Rows.Clear();
                table.Dispose();
            }
            ds.AdditionalData.Tables.Clear();
            ds.AdditionalData.Dispose();
        }
    }
    /// <summary>
    /// 获取当前结果集中的记录数量
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static int Count(this ResultInfo<DataSet> ds)
    {
        if (ds.OperationDone == false)
        {
            return 0;
        }
        return ds.AdditionalData.Tables[0].Rows.Count;
    }
    /// <summary>
    /// 遍历当前结果集
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static IEnumerable<DataRow> ReadRow(this ResultInfo<DataSet> ds)
    {
        if (ds.OperationDone == false || (ds.AdditionalData != null && ds.AdditionalData.Tables[0].Rows.Count == 0))
        {
            return new List<DataRow>();
        }
        else
        {
            return ds.AdditionalData.Tables[0].Rows.Cast<DataRow>();

        }
    }

    /// <summary>
    /// 获取当前结果集中的记录数量
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static int Count(this DataSet ds)
    {
        if (ds.Tables.Count == 0)
        {
            return 0;
        }
        return ds.Tables[0].Rows.Count;
    }
    /// <summary>
    /// 遍历当前结果集
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static IEnumerable<DataRow> ReadRow(this DataSet ds)
    {
        if (ds.Tables.Count == 0)
        {
            yield break;
        }
        else
        {
            foreach (var row in ds.Tables[0].Rows.Cast<DataRow>())
            {
                yield return row;
            }

        }
    }


    /// <summary>
    /// 将DataRow转化为对象
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public static ExpandoObject ToExpendoObject(this DataRow row)
    {
        ExpandoObject obj = new ExpandoObject();
        foreach (DataColumn col in row.Table.Columns)
        {
            obj.SetPropery(col.ColumnName, row[col.ColumnName]);
        }
        return obj;
    }
    public static string ReadString(this DataRow row,string column)
    {
        return row.Read<string>(column);
    }
    public static int ReadInt(this DataRow row, string column)
    {
        return row.Read<int>(column);
    }
    public static long ReadLong(this DataRow row, string column)
    {
        return row.Read<long>(column);
    }
    public static DateTime ReadDateTime(this DataRow row, string column)
    {
        return row.Read<DateTime>(column);
    }
    public static bool ReadBoolean(this DataRow row, string column)
    {
        return row.Read<bool>(column);
    }
    /// <summary>
    /// 读取数据行中的列
    /// </summary>
    /// <typeparam name="T">当前列的数据类型</typeparam>
    /// <param name="row"></param>
    /// <param name="columnName">列名</param>
    /// <returns></returns>
    public static T Read<T>(this DataRow row, string columnName)
    {
        if (row.Table.Columns.IndexOf(columnName) == -1)
        {
            return default(T);
        }
        var data = row[columnName];
        if (data == DBNull.Value || data == null)
        {
            return default(T);
        }
        return (T)data;
    }

    /// <summary>
    /// 读取数据行中的列
    /// </summary>
    /// <typeparam name="T">当前列的数据类型</typeparam>
    /// <param name="row"></param>
    /// <param name="index">列序号</param>
    /// <returns></returns>
    public static T Read<T>(this DataRow row, int index)
    {
        var data = row[index];
        if (data == DBNull.Value || data == null)
        {
            return default(T);
        }
        return (T)data;
    }
    /// <summary>
    /// 读取JSON对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="row"></param>
    /// <param name="columnName">列名</param>
    /// <returns></returns>
    public static T ReadJson<T>(this DataRow row, string columnName)
    {
        var data = row.Read<string>(columnName);
        if (data.IsNullOrEmpty())
        {
            return default(T);
        }
        return data.FromJsonString<T>();
    }
    /// <summary>
    /// 读取JSON对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="row"></param>
    /// <param name="index">列序号</param>
    /// <returns></returns>
    public static T GetJsonValue<T>(this DataRow row, int index)
    {
        var data = row.Read<string>(index);
        if (data.IsNullOrEmpty())
        {
            return default(T);
        }
        return data.FromJsonString<T>();
    }
    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="row"></param>
    /// <param name="field">字段名</param>
    /// <returns></returns>
    public static string GetStringValue(this DataRow row, string field)
    {
        return row.Read<string>(field);
    }


    public static List<T> CastFields<T>(this ResultInfo<DataSet> ds, int field = 0, bool fromJson = false)
    {
        List<T> result = new List<T>();
        if (ds.OperationDone)
        {
            result = ds.AdditionalData.Tables[0].Rows.Cast<DataRow>().Where(r => r[field] != DBNull.Value && r[field].ToString().IsNotNullOrEmpty()).Select(r => fromJson ? (r[field].ToString().FromJsonString<T>()) : (T)r[field]).ToList();
        }

        return result;

    }

    /// <summary>
    /// 将数据集合转化为对象列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static List<T> CastToObject<T>(this ResultInfo<DataSet> ds)
    {
        return ds.AdditionalData.Tables[0].Rows.Cast<DataRow>().Where(r => r[0] != DBNull.Value && r[0].ToString().IsNotNullOrEmpty()).Select(r => r[0].ToString().FromJsonString<T>()).ToList();
    }
}
