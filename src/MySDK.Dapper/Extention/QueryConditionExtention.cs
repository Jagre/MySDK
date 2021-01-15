using System.Text;

namespace MySDK.Dapper.Extention
{
    public static class QueryConditionExtention
    {
        public static StringBuilder AndIf(this StringBuilder builder, bool flag, string condition)
        {
            if (flag)
            {
                builder.Append($" and {condition}");
            }
            return builder;
        }

        public static string AndIf(this string sql, bool flag, string condition)
        {
            if (flag)
            {
                sql += $" and {condition}";
            }
            return sql;
        }

        public static StringBuilder OrIf(this StringBuilder builder, bool flag, string condition)
        {
            if (flag)
            {
                builder.Append($" or ({condition})");
            }
            return builder;
        }

        public static string OrIf(this string sql, bool flag, string condition)
        {
            if (flag)
            {
                sql += $" or ({condition})";
            }
            return sql;
        }
    }
}
