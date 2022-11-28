using DotNetOrm.DbProxy.Expressions.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Expressions.Vistor
{
    public class ConditionBuilderVisitor : ExpressionVisitor
    {
        private Stack<string> _stack = new Stack<string>();
        private List<MySqlParameter> _parameters = new List<MySqlParameter>();
        private object? _tempValue;


        /// <summary>
        /// 返回解析表达式目录树生成的查询条件
        /// </summary>
        /// <returns></returns>
        public string GetQueryCondition(out List<MySqlParameter> parameters)
        {
            string condition = string.Concat(_stack.ToArray());
            _stack.Clear();
            parameters = _parameters;
            return condition;
        }

        /// <summary>
        /// 解析表达式目录树的入口
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        //[return: NotNullIfNotNull("node")]
        //public override Expression? Visit(Expression? node)
        //{
        //    return base.Visit(node);
        //}

        /// <summary>
        /// 解析二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _stack.Push(" ) ");
            base.Visit(node.Right);
            _stack.Push(node.NodeType.GetSqlOperator());
            base.Visit(node.Left);
            _stack.Push(" ( ");
            return node;
        }

        /// <summary>
        /// 解析常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            // 将值插入栈 非参数化 stack存入具体的值
            // _stack.Push($"'{node.Value}'");

            // 参数化  stack不存入具体的值
            _tempValue = node.Value;
            return node;
        }

        /// <summary>
        /// 解析成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ConstantExpression)
            {
                var value1 = InvokeValue(node);
                var value2 = ReflectionValue(node);

                // 非参数化 stack存入具体的值
                // _stack.Push($"'{value1}'");

                // 参数化 tack不存入具体的值
                _tempValue = value1;
            }
            else
            {
                // 非参数化
                // _stack.Push($"{node.Member.Name}");

                // 参数化
                if(_tempValue != null)
                {
                    string name = node.Member.Name;//.GetName();
                    string paraName = $"@{name}{_parameters.Count}";
                    string sOperator = _stack.Pop();

                    // stack的特性，后进先出
                    _stack.Push(paraName);
                    _stack.Push(sOperator);
                    _stack.Push(name);

                    var tempValue = _tempValue;
                    _parameters.Add(new MySqlParameter(paraName, tempValue));
                    _tempValue = null;
                }
            }
            return node;
        }


        /// <summary>
        /// 方法表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            // 
            Visit(node.Arguments[0]);
            string format;

            // 方法名
            switch (node.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                case "Equals":
                    format = "({0} = {1})";
                    break;

                default:
                    throw new NotSupportedException(node.NodeType + " is not supported!");
            }

            _stack.Push(format);
            // 
            Visit(node.Object);
            string left = _stack.Pop();
            format = _stack.Pop();
            string right = _stack.Pop();
            _stack.Push(string.Format(format, left, right));

            return node;
        }


        #region private methods

        /// <summary>
        /// 获取属性值 -- 使用委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private object InvokeValue(MemberExpression expression)
        {
            var objExp = Expression.Convert(expression, typeof(object));
            return Expression.Lambda<Func<object>>(objExp).Compile().Invoke();
        }

        /// <summary>
        /// 使用反射
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private object ReflectionValue(MemberExpression member)
        {
            var obj = (member.Expression as ConstantExpression)!.Value;
            return (member.Member as FieldInfo)!.GetValue(obj)!;
        }
        #endregion
    }
}
