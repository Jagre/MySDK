using MySDK.Dapper.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;

namespace MySDK.Dapper
{
    public class UpdateBuilder<TTable>
    {
        private static readonly ConcurrentDictionary<Type, string> _primarykeys = new ConcurrentDictionary<Type, string>();
        private readonly StringBuilder _builder = new StringBuilder();
        public UpdateBuilder()
        {
        }

        public UpdateBuilder<TTable> BuildColum<TField>(Expression<Func<TTable, TField>> predicate)
        {
            var lambda = predicate as LambdaExpression;
            if (lambda.Body is MemberExpression)
            {
                var member = lambda.Body as MemberExpression;
                if (member.NodeType == ExpressionType.MemberAccess)
                {
                    var name = member.Member.Name;
                    _builder.Append($"{name} = @{name}, ");
                }
            }
            return this;
        }

        public string BuildSql()
        {
            var primaryKeyName = GetPrimaryKeyFieldName();
            if (string.IsNullOrEmpty(primaryKeyName))
                return string.Empty;

            var updateFields = _builder.ToString().TrimEnd(',').Trim();
            if (string.IsNullOrEmpty(updateFields))
                return string.Empty;

            return $@"
                UPDATE  {typeof(TTable).Name}
                SET     {updateFields} 
                WHERE   {primaryKeyName} = @{primaryKeyName}";
        }

        private static string GetPrimaryKeyFieldName()
        {
            var table = typeof(TTable);
            var primaryKeyName = string.Empty;
            if (_primarykeys.TryGetValue(table, out primaryKeyName))
            {
                return primaryKeyName;
            }
            primaryKeyName = table.GetPrimaryKeyName();
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                _primarykeys.TryAdd(table, primaryKeyName);
            }
            return primaryKeyName;
        }


    }
}
