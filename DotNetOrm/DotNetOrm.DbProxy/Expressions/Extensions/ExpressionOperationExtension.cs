using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DotNetOrm.DbProxy.Expressions.Extensions
{
    /// <summary>
    /// Expression Operation Extension
    /// 表达式目录 按指定运算拼接扩展
    /// </summary>
    public static class ExpressionOperationExtension
    {
        /// <summary>
        /// And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            var binaryExpression = Expression.And(expression1.Body, expression2.Body);
            return Expression.Lambda<Func<T, bool>>(binaryExpression, newParameter);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            var binaryExpression = Expression.Or(expression1.Body, expression2.Body);
            return Expression.Lambda<Func<T, bool>>(binaryExpression, newParameter);
        }

        /// <summary>
        /// Not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var candidateExpr = expression.Parameters[0];
            var body = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

    }
}
