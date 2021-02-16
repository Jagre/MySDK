using System.Text;

namespace MySDK.Dapper.Extentions
{
    public static class QueryConditionExtention
    {
        public static StringBuilder AndIf(this StringBuilder builder, bool flag, string condition)
        {
            if (flag)
            {
                builder.Append($" AND {condition}");
            }
            return builder;
        }

        public static string AndIf(this string sql, bool flag, string condition)
        {
            if (flag)
            {
                sql += $" AND {condition}";
            }
            return sql;
        }

        public static StringBuilder OrIf(this StringBuilder builder, bool flag, string condition)
        {
            if (flag)
            {
                builder.Append($" OR ({condition})");
            }
            return builder;
        }

        public static string OrIf(this string sql, bool flag, string condition)
        {
            if (flag)
            {
                sql += $" OR ({condition})";
            }
            return sql;
        }
    }
}
