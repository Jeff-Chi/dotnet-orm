using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Expressions.Extensions
{
    internal static class ExpressionTypeExtension
    {
        internal static string GetSqlOperator(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return "AND";
                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    return "OR";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Equal:
                    return "=";
                default:
                    throw new ArgumentException("type is not supported");
            }
        }
    }
}
