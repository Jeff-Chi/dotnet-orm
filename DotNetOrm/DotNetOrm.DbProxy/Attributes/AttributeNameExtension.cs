using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Attributes
{
    public static class AttributeNameExtension
    {

        /// <summary>
        /// MemberInfo, Type PropertyInfo都继承此类
        /// 扩展方法
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetCustomAttributeName(this MemberInfo member)
        {

            #region 2.0 抽象类

            if (member.IsDefined(typeof(AbstractNameAttribute), true))
            {
                AbstractNameAttribute attribute = member.GetCustomAttribute<AbstractNameAttribute>()!;
                return attribute.GetName();
            }

            return member.Name;
            #endregion


            #region 1.0
            // 1.0 使用 if - else 判断是哪个Attribute

            //string name;

            //if (member.IsDefined(typeof(CustomTableAttribute), true))
            //{
            //    //获取标记的特性；
            //    CustomTableAttribute attribute = member.GetCustomAttribute<CustomTableAttribute>()!;
            //    name = attribute.GetName();
            //}
            //else if (member.IsDefined(typeof(CustomTableAttribute), true))
            //{
            //    CustomTableAttribute attribute = member.GetCustomAttribute<CustomTableAttribute>()!;
            //    name = attribute.GetName();
            //}
            //else
            //{
            //    name = member.Name;
            //}
            //return name;

            #endregion

        }

        /// <summary>
        /// 获取自定义的table名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            string tableName;
            if (type.IsDefined(typeof(CustomTableAttribute), true))
            {
                //获取标记的特性；
                CustomTableAttribute attribute = type.GetCustomAttribute<CustomTableAttribute>()!;
                tableName = attribute.GetName();
            }
            else
            {
                tableName = type.Name;
            }
            return tableName;
        }


        /// <summary>
        /// 获取自定义字段名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetColumnName(PropertyInfo prop)
        {
            string columnName;
            if (prop.IsDefined(typeof(CustomColumnAttribute), true))
            {
                //获取标记的特性；
                CustomColumnAttribute attribute = prop.GetCustomAttribute<CustomColumnAttribute>()!;
                columnName = attribute.GetName();
            }
            else
            {
                columnName = prop.Name;
            }
            return columnName;
        }

    }
}
