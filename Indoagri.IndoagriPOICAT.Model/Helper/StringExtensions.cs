using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Helper
{
    public static class StringExtensions
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static string ToCurrencyFormat(decimal value = 0, string format = null)
        {
            if (format == "IDR")
            {
                return value.ToString("n0", CultureInfo.CreateSpecificCulture("id-IDR"));
            }
            else
            {
                return value.ToString("n2", CultureInfo.CreateSpecificCulture("id-IDR"));
            }
        }

        public static string TaxFormat(this string taxNo)
        {
            if (taxNo.Length == 15)
            {
                return taxNo.Insert(2, ".").Insert(6, ".").Insert(10, ".").Insert(12, "-").Insert(16, ".");
            }
            else { return taxNo; }
        }

        public static string ToSqlSafeLikeData(this string val)
        {
            return Regex.Replace(val, @"([%_\[])", @"[$1]").Replace("'", "''");
        }

        public static string ToSqlSafeString(this string val)
        {
            return "'" + val.Replace("'", "''") + "'";
        }
    }
}
