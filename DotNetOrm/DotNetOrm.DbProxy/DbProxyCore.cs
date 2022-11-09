using DotNetOrm.DbProxy.SqlBuilder;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // 打开连接
            connection.Open();

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
            sql = $"{sql}{id}";

            #endregion

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            // 返回查询结果集
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ss = type.GetProperties();
                foreach (var prop in type.GetProperties())
                {
                    // 逐个赋值
                    // prop.SetValue(obj,reader["Id"]);

                    // 直接赋值  判断是否为dbnull
                    prop.SetValue(obj, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
                }

                var s = reader["Id"];
            }

            connection.Close();
            return (T)obj!;
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Query<T>() where T : BaseModel
        {
            string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // 泛型缓存
            string sql = ObjectManagerProvider<T>.GetFindSql();

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            var reader = cmd.ExecuteReader();

            List<T> list = new List<T>();
            Type type = typeof(T);

            //if (!reader.HasRows)
            //{
            //    return default!;
            //}

            while (reader.Read())
            {
                var obj = Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    // 直接赋值  判断是否为dbnull
                    prop.SetValue(obj, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
                }
                list.Add((T)obj!);
            }

            return list;
        }
    }
}
