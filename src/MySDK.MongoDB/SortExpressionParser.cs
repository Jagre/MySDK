using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MySDK.MongoDB
{
    public class SortExpressionParser
    {
        public void Parsing<T>(Expression<Func<IEnumerable<T>, dynamic>> predicate, Stack<string> stackMemeber, Stack<int> stackSortMethod)
        {
            ParseMethodCallExpression(predicate.Body, stackMemeber, stackSortMethod);
        }

        private void ParseMethodCallExpression(Expression exp, Stack<string> stackMemeber, Stack<int> stackSortMethod)
        {
            if (exp is MethodCallExpression)
            {
                var methodExp = exp as MethodCallExpression;
                var methodName = methodExp.Method.Name;
                if ("OrderBy|ThenBy|OrderByDescending|ThenByDescending".IndexOf(methodName) < 0)
                    throw new Exception("Pls use sorting method of LinQ(eg: OrderBy, ThenBy, OrderByDescending, ThenByDescending)");

                if (methodName == "OrderByDescending" || methodName == "ThenByDescending")
                    stackSortMethod.Push(-1);
                else
                    stackSortMethod.Push(1);

                if (methodExp.Arguments != null && methodExp.Arguments.Any())
                {
                    
                    for (var i = methodExp.Arguments.Count - 1; i >= 0; i--)
                    {
                        var expression = methodExp.Arguments[i];
                        if (expression is MethodCallExpression)
                        {
                            ParseMethodCallExpression(expression, stackMemeber, stackSortMethod);
                        }
                        else if (expression is LambdaExpression)
                        {
                            ParseLambdaExpression(expression, stackMemeber, stackSortMethod);
                        }
                    }
                }
            }
        }

        private void ParseLambdaExpression(Expression exp, Stack<string> stackMember, Stack<int> stackSortMethod)
        {
            if (exp is LambdaExpression)
            {
                var lambda = exp as LambdaExpression;
                if (lambda.Body is MemberExpression)
                {
                    ParseMemberExpression(lambda.Body as MemberExpression, stackMember, stackSortMethod);
                }
                else if (lambda.Body is NewExpression)
                {
                    ParseNewExpression(lambda.Body as NewExpression, stackMember, stackSortMethod);
                }
            }
        }

        private void ParseMemberExpression(MemberExpression memberExp, Stack<string> stackMember, Stack<int> stackSortMethod)
        {
            if (memberExp.NodeType == ExpressionType.MemberAccess)
            {
                var name = memberExp.Member.Name;
                stackMember.Push(name);
            }
        }

        private void ParseNewExpression(NewExpression newExp, Stack<string> stackMember, Stack<int> stackSortMethod)
        {
            if (newExp.Arguments != null && newExp.Arguments.Any())
            {
                var sortMethod = stackSortMethod.Pop();
                for (var i = newExp.Arguments.Count - 1; i >= 0; i--)
                {
                    var argExp = newExp.Arguments[i];
                    if (argExp is MemberExpression)
                    {
                        var memberExp = argExp as MemberExpression;
                        if (memberExp.NodeType == ExpressionType.MemberAccess)
                        {
                            stackMember.Push(memberExp.Member.Name);
                            stackSortMethod.Push(sortMethod);
                        }
                    }
                }
            }
        }
    }
}
