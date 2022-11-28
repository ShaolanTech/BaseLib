using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace System
{
    public static class CookieExtensions
    {
        /// <summary>
        /// 获取全部Cookie
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static CookieCollection GetAllCookies(this CookieContainer container)
        {
            var allCookies = new CookieCollection();
            var domainTableField = container.GetType().GetRuntimeFields().FirstOrDefault(x => x.Name == "m_domainTable");
            var domains = (IDictionary)domainTableField.GetValue(container);

            foreach (var val in domains.Values)
            {
                var type = val.GetType().GetRuntimeFields().First(x => x.Name == "m_list");
                var values = (IDictionary)type.GetValue(val);
                foreach (CookieCollection cookies in values.Values)
                {
                    allCookies.Add(cookies);
                }
            }
            return allCookies;
        }


    }
}
