using Hackathon.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;
using TimeZoneNames;

namespace Hackathon.Service.Utilities
{
    public class TimeUtility
    {
        public static int CalculateUtcOffset(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                timeZone = "UTC";
            }

            
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(timeZone);
            var time = TimeZoneInfo.ConvertTime(DateTime.Now, tzi);
            
            var offset = tzi.GetUtcOffset(time);

            return offset.Hours;
        }

        public static TimeZoneInfo GetTimeZoneInfo(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                timeZone = "UTC";

            return TZConvert.GetTimeZoneInfo(timeZone);
        }

        public static Dictionary<string,string> GetTimeZoneDictionary()
        {
            var tzList = new List<IDictionary<string, string>>
            {
                TZNames.GetTimeZonesForCountry("US", "en-US", DateTimeOffset.UtcNow),
                TZNames.GetTimeZonesForCountry("GB", "en-US", DateTimeOffset.UtcNow),
                TZNames.GetTimeZonesForCountry("IN", "en-US", DateTimeOffset.UtcNow),
                TZNames.GetTimeZonesForCountry("AU", "en-US", DateTimeOffset.UtcNow)
            };

            return Merge(tzList);
        }

        private static Dictionary<K, V> Merge<K, V>(IEnumerable<IDictionary<K, V>> dictionaries)
        {
            return dictionaries.SelectMany(x => x)
                            .GroupBy(d => d.Key)
                            .ToDictionary(x => x.Key, y => y.First().Value);
        }

    }
}
