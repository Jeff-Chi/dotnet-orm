using DotNetOrm.DbProxy.Expressions.Vistor;
using DotNetOrm.DbProxy.SqlBuilder;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNetOrm.DbProxy.Attributes;

namespace DotNetOrm.DbProxy
{
    /// <summary>
    /// 查询核心类
    /// </summary>
    public class DbProxyCore
    {
        /// <summary>
        /// 主键查询 普通版
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        //public Product GetProduct(int id)
        //{
        //    Product product = new();
        //    string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        Console.WriteLine(connection.State);
        //        // 打开连接
        //        connection.Open();

        //        string sql = @$"SELECT
        //                     `Id`,
        //                     `CategoryId`,
        //                     `Name`,
        //                     `Price`,
        //                     `Url`,
        //                     `ImageUrl` 
        //                    FROM
        //                     product 
        //                    WHERE
        //                     Id ={id};";

        //        // 执行sql命令
        //        MySqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = sql;

        //        // 返回查询结果集
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            product.Id = Convert.ToInt32(reader["Id"]);
        //            product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
        //            product.Name = reader["Name"].ToString()!;
        //            product.Price = Convert.ToInt32(reader["Price"]);
        //            product.Url = reader["Url"].ToString()!;
        //            product.ImageUrl = reader["ImageUrl"].ToString();
        //            Console.WriteLine("===============================================");
        //        }

        //        connection.Close();
        //    }
        //    return product;
        //}

        /// <summary>
        /// 主键查询 泛型通用版
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel
        {
            // T entity = new T();
            Type type = typeof(T);
            // 反射创建对象--调用无参数构造函数
            object? obj = Activator.CreateInstance(type);

            string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
            using MySqlConnection connection = new MySqlConnection(connectionString);
            Console.WriteLine(connection.State);
            

            #region 查询语句固定

            // 查询语句固定
            //string sql = @$"SELECT
            //                 `Id`,
            //                 `CategoryId`,
            //                 `Name`,
            //                 `Price`,
            //                 `Url`,
            //                 `ImageUrl` 
            //                FROM
            //                 product 
            //                WHERE
            //                 Id ={id};";

            #endregion

            #region 使用T类型的属性名，动态生成sql语句

            // 属性名
            List<string> propNames = type.GetProperties().Select(p => p.Name).ToList();

            string strProps = string.Join(",", propNames);

            // 每次都重新生成sql
            // string sql = @$"SELECT {strProps} FROM {type.Name} WHERE Id = {id}";

            // 普通字典缓存
            // string sql = ObjectManagerProviderDict.GetFindSql<T>();

            // 泛型缓存
            string sql = ObjectManagerProvider<T>.GetFindSql();

            #endregion
            // 参数化
            MySqlParameter parameter =new MySqlParameter("@id",id);

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(parameter);

            // 打开连接
            connection.Open();

            // 返回查询结果集
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                foreach (var prop in type.GetProperties())
                {
                    // 逐个赋值
                    // prop.SetValue(obj,reader["Id"]);

                    // 1.0 直接赋值  判断是否为dbnull
                    // prop.SetValue(obj, reader[prop.Name] is DBNull ? null : reader[prop.Name]);

                    // 2.0 从特性中获取属性名
                    //string columnName = AttributeNameExtension.GetColumnName(prop);
                    //prop.SetValue(obj, reader[columnName] is DBNull ? null : reader[columnName]);

                    // 3.0 从特性中获取属性名 抽象类型扩展方法
                    prop.SetValue(obj, reader[prop.GetCustomAttributeName()] is DBNull ? null : reader[prop.GetCustomAttributeName()]);
                }

                var s = reader["Id"];
            }

            connection.Close();
            return (T)obj!;
        }


        /// <summary>
        /// 集合查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Query<T>() where T : BaseModel
        {
            string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
            using MySqlConnection connection = new MySqlConnection(connectionString);

            // 泛型缓存
            string sql = ObjectManagerProvider<T>.GetQuerySql();

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            // 打开数据链接
            connection.Open();

            var reader = cmd.ExecuteReader();

            // 待优化 代码顺序..
            List<T> list = new List<T>();
            Type type = typeof(T);

            //if (!reader.HasRows)
            //{
            //    return default!;
            //}

            while (reader.Read())
            {
                var obj = Activator.CreateInstance(type);
                // TODO: 优化提取属性到外部，优化每次read都需要GetProperties()一次
                foreach (var prop in type.GetProperties())
                {
                    // 1.0 直接赋值  判断是否为dbnull
                    //prop.SetValue(obj, reader[prop.Name] is DBNull ? null : reader[prop.Name]);

                    // 2.0 从特性中获取属性名
                    //string columnName = AttributeNameExtension.GetColumnName(prop);
                    //prop.SetValue(obj, reader[columnName] is DBNull ? null : reader[columnName]);

                    // 3.0 从特性中获取属性名 抽象类型扩展方法
                    prop.SetValue(obj, reader[prop.GetCustomAttributeName()] is DBNull ? null : reader[prop.GetCustomAttributeName()]);
                }
                list.Add((T)obj!);
            }

            return list;
        }

        /// <summary>
        /// 集合查询 -- 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Query2<T>(Expression<Func<T,bool>> expression) where T : BaseModel
        {
            string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
            using MySqlConnection connection = new MySqlConnection(connectionString);

            // 泛型缓存
            string sql = ObjectManagerProvider<T>.GetQuerySql();

            // 查询条件表达式: x=>x.Name.Contains("ss") && x.Price>10
            // 对应的sql语句: where Name like "%ss%" and Price > 10
            var conditionBuilder = new ConditionBuilderVisitor();
            conditionBuilder.Visit(expression);

            // 参数集合
            List<MySqlParameter> mySqlParameters;

            string where = conditionBuilder.GetQueryCondition(out mySqlParameters);
            sql = $"{sql} Where {where}";

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            // 打开数据链接
            connection.Open();

            var reader = cmd.ExecuteReader();

            // 待优化 代码顺序..
            List<T> list = new List<T>();
            Type type = typeof(T);

            //if (!reader.HasRows)
            //{
            //    return default!;
            //}

            while (reader.Read())
            {
                var obj = Activator.CreateInstance(type);
                // TODO: 优化提取属性到外部，优化每次read都需要GetProperties()一次
                foreach (var prop in type.GetProperties())
                {
                    // 1.0 直接赋值  判断是否为dbnull
                    // prop.SetValue(obj, reader[prop.Name] is DBNull ? null : reader[prop.Name]);

                    // 2.0 从特性中获取属性名
                    //string columnName = AttributeNameExtension.GetColumnName(prop);
                    //prop.SetValue(obj, reader[columnName] is DBNull ? null : reader[columnName]);

                    // 3.0 从特性中获取属性名 抽象类型扩展方法
                    prop.SetValue(obj, reader[prop.GetCustomAttributeName()] is DBNull ? null : reader[prop.GetCustomAttributeName()]);
                }
                list.Add((T)obj!);
            }

            return list;
        }

    }
}
