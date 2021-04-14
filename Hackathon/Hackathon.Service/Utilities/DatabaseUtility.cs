using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Data;
using System.Security.Claims;

namespace Hackathon.Service.Utilities
{
    public class DatabaseUtility
    {
        public static DateTime? ReadDateTimeUTC(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                var dt = Convert.ToDateTime(r[column]);
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ReadDateTimeUTC(DataRow r, string column, int offset)
        {
            if (!r.IsNull(column))
            {
                var dt = Convert.ToDateTime(r[column]);
                return DateTime.SpecifyKind(dt.AddHours(offset), DateTimeKind.Utc);
            }
            else
            {
                return null;
            }
        }

        public static int? ReadNullableInt(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return Convert.ToInt32(r[column]);
            }
            else
            {
                return null;
            }
        }

        public static int? ReadNullableIntId(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return Convert.ToInt32(r[column]);
            }
            else
            {
                return 0;
            }
        }

        public static string ReadNullableString(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return r[column].ToString();
            }
            else
            {
                return null;
            }
        }

        public static Guid? ReadNullableGuid(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return Guid.Parse(r[column].ToString());
            }
            else
            {
                return null;
            }
        }

        public static bool ReadIntToBool(DataRow r, string column)
        {
            return bool.Parse(r[column].ToString());
        }

        public static bool? ReadNullableIntToBool(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return bool.Parse(r[column].ToString());
            }
            else
            {
                return null;
            }
            
        }

        public static double? ReadNullableDouble(DataRow r, string column)
        {
            if (!r.IsNull(column))
            {
                return Convert.ToDouble(r[column]);
            }
            else
            {
                return null;
            }
        }
    }
}
