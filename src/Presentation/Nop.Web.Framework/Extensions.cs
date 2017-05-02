using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Services.Helpers;
using Nop.Web.Framework.Kendoui;

namespace Nop.Web.Framework
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extensions
    {
        public static IEnumerable<T> PagedForCommand<T>(this IEnumerable<T> current, DataSourceRequest command)
        {
            return current.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        }

        /// <summary>
        ///返回一个值，表示是否实际选择是不可能的
        /// </summary>
        /// <param name="items">项目</param>
        /// <param name="ignoreZeroValue">一个值，表示我们是否忽略具有“0”值的项目</param>
        /// <returns>指示实际选择是否不可能的值</returns>
        public static bool SelectionIsNotPossible(this IList<SelectListItem> items, bool ignoreZeroValue = true)
        {
            if (items == null)
                throw  new ArgumentNullException("items");

            //我们忽略具有“0”值的项目？ 通常它是像“全选”，“等等”
            return items.Count(x => !ignoreZeroValue || !x.Value.ToString().Equals("0")) < 2;
        }

        /// <summary>
        /// DateTime的相对格式（例如2个月前，一个月前）
        /// </summary>
        /// <param name="source">来源（UTC格式）</param>
        /// <returns>格式化日期和时间字符串</returns>
        public static string RelativeFormat(this DateTime source)
        {
            return RelativeFormat(source, string.Empty);
        }
        /// <summary>
        /// DateTime的相对格式（例如2个月前，一个月前）
        /// </summary>
        /// <param name="source">来源（UTC格式）</param>
        /// <param name="defaultFormat">默认格式字符串（在不应用相对格式的情况下）</param>
        /// <returns>格式化日期和时间字符串</returns>
        public static string RelativeFormat(this DateTime source, string defaultFormat)
        {
            return RelativeFormat(source, false, defaultFormat);
        }
        /// <summary>
        /// DateTime的相对格式（例如2个月前，一个月前）
        /// </summary>
        /// <param name="source">来源（UTC格式）</param>
        /// <param name="convertToUserTime">一个值，表示是否应将DateTime实例转换为用户本地时间（如果未应用相对格式化）</param>
        /// <param name="defaultFormat">默认格式字符串（在不应用相对格式的情况下）</param>
        /// <returns>格式化日期和时间字符串</returns>
        public static string RelativeFormat(this DateTime source,
            bool convertToUserTime, string defaultFormat)
        {
            string result = "";

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
            double delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    result = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                }
                else if (delta < 120) //2 (minutes) * 60 (seconds)
                {
                    result = "a minute ago";
                }
                else if (delta < 2700) // 45 (minutes) * 60 (seconds)
                {
                    result = ts.Minutes + " minutes ago";
                }
                else if (delta < 5400) // 90 (minutes) * 60 (seconds)
                {
                    result = "an hour ago";
                }
                else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    result = hours + " hours ago";
                }
                else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = "yesterday";
                }
                else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = ts.Days + " days ago";
                }
                else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    result = months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    result = years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            else
            {
                DateTime tmp1 = source;
                if (convertToUserTime)
                {
                    tmp1 = EngineContext.Current.Resolve<IDateTimeHelper>().ConvertToUserTime(tmp1, DateTimeKind.Utc);
                }
                //default formatting
                if (!string.IsNullOrEmpty(defaultFormat))
                {
                    result = tmp1.ToString(defaultFormat);
                }
                else
                {
                    result = tmp1.ToString();
                }
            }
            return result;
        }
    }
}
