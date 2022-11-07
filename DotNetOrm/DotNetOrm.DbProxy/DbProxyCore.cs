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
        /// 主键查询
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

        public T Find<T>(int id) where T :BaseModel
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

            string sql = @$"SELECT {strProps} FROM {type.Name} WHERE Id = {id}";
            #endregion

            // 执行sql命令
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            // 返回查询结果集
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //product.Id = Convert.ToInt32(reader["Id"]);
                //product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                //product.Name = reader["Name"].ToString()!;
                //product.Price = Convert.ToInt32(reader["Price"]);
                //product.Url = reader["Url"].ToString()!;
                //product.ImageUrl = reader["ImageUrl"].ToString();
                //Console.WriteLine("===============================================");


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
    }
}
