using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Helpers
{
    public class ValueConverterHelper
    {
        public static int ToInt(object input)
        {
            if (input == null)
                return 0;
            if (input is int i)
                return i;
            if (input is decimal d)
                return Convert.ToInt32(d);
            if (input is double db)
                return Convert.ToInt32(db);
            if (input is float f)
                return Convert.ToInt32(f);
            if (input is long l)
                return Convert.ToInt32(l);
            if (input is string s && !string.IsNullOrWhiteSpace(s))
            {
                if (int.TryParse(s.Replace(",", ""), out var result))
                    return result;
            }
            if (int.TryParse(input.ToString().Replace(",", ""), out var fallback))
                return fallback;

            return 0;
        }

        public static decimal ToDecimal(object input)
        {
            if (input == null)
                return 0m;
            if (input is decimal d)
                return d;
            if (input is double db)
                return (decimal)db;
            if (input is float f)
                return (decimal)f;
            if (input is int i)
                return i;
            if (input is long l)
                return l;
            if (input is string s && !string.IsNullOrWhiteSpace(s))
            {
                if (decimal.TryParse(s.Replace(",", ""), out var result))
                    return result;
            }
            if (decimal.TryParse(input.ToString().Replace(",", ""), out var fallback))
                return fallback;

            return 0m;
        }

        public static double ToDouble(object input)
        {
            if (input == null)
                return 0.0;

            if (input is double d)
                return d;
            if (input is float f)
                return f;
            if (input is decimal dec)
                return (double)dec;
            if (input is int i)
                return i;
            if (input is long l)
                return l;
            if (input is string s && !string.IsNullOrWhiteSpace(s))
            {
                if (double.TryParse(s.Replace(",", ""), out var result))
                    return result;
            }

            if (double.TryParse(input.ToString().Replace(",", ""), out var fallback))
                return fallback;

            return 0.0;
        }

        public static DateTime ToDate(object input)
        {
            if (input == null)
                return DateTime.Now;

            string str = input.ToString().Trim();

            if (str.Length == 8 && long.TryParse(str, out _))
            {
                string year = str.Substring(0, 4);
                string month = str.Substring(4, 2);
                string day = str.Substring(6, 2);
                string formatted = $"{year}-{month}-{day}";
                if (DateTime.TryParse(formatted, out var dt))
                    return dt;
            }

            if (DateTime.TryParse(str, out var parsed))
                return parsed;

            return DateTime.Now;
        }
    }
}
