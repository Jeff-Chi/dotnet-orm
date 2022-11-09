using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.SqlBuilder
{
    /// <summary>
    /// 泛型缓存
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    public class ObjectManagerProvider<T>
    {
        private static Type type = typeof(T);
        /// <summary>
        /// 主键查询
        /// </summary>
        private static string findSql = string.Empty;
        /// <summary>
        /// 集合查询
        /// </summary>
        private static string querySql = string.Empty;
        static ObjectManagerProvider()
        {
            string tableName = type.Name;

            IEnumerable<PropertyInfo> propertyInfos = type.GetProperties();

            // 属性名
            List<string> propNames = type.GetProperties().Select(p => p.Name).ToList();

            string strProps = string.Join(",", propNames);

            findSql = @$"SELECT {strProps} FROM {tableName} WHERE Id =@id";

            querySql = @$"SELECT {strProps} FROM {tableName}";
        }

        public ObjectManagerProvider()
        {
            Console.WriteLine(findSql);
        }

        public static string GetFindSql() => findSql;

        public static string GetQuerySql() => querySql;

    }

    /// <summary>
    /// 普通缓存
    /// </summary>
    public class ObjectManagerProviderDict
    {
        // sql字典
        private static Dictionary<string, string> sqlCache = new Dictionary<string, string>();
        public static string GetFindSql<T>()
        {
            Type type = typeof(T);
            string className = type.FullName!;
            if (sqlCache.ContainsKey(className))
            {
                return sqlCache[className];
            }
            else
            {
                // 属性名
                List<string> propNames = type.GetProperties().Select(p => p.Name).ToList();

                string strProps = string.Join(",", propNames);

                sqlCache[className] = @$"SELECT {strProps} FROM {type.Name} WHERE Id =";
                return sqlCache[className];
            }
        }

    }
}
