using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

Console.WriteLine("Hello, World!");

//Server = myServerAddress; Database = myDataBase; Uid = myUsername; Pwd = myPassword;
// Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
string connectionString = "Server=localhost;Uid=root;Pwd=admin123456;Database=dotnetorm;";
using (MySqlConnection connection = new MySqlConnection(connectionString))
{
    Console.WriteLine(connection.State);
    // 打开连接
    connection.Open();

    #region 增删改

    // 普通语句
    //    string sql = @"INSERT INTO `product` (`CategoryId`, `Name`, `Price`, `Url`, `ImageUrl`) 
    //VALUES (1, 'a', 11.00, 'www.baidu.com', 'www.baidu.com');";
    // 参数化
    string sql = @"INSERT INTO `product` (`CategoryId`, `Name`, `Price`, `Url`, `ImageUrl`) 
VALUES (@CategoryId, @Name, @Price, @Url, @ImageUrl);";

    // 执行sql命令
    MySqlCommand cmd = connection.CreateCommand();
    cmd.CommandText = sql;
    // 参数化
    cmd.Parameters.Add(new MySqlParameter("@CategoryId", 111));
    cmd.Parameters.Add(new MySqlParameter("@Name", "par"));
    cmd.Parameters.Add(new MySqlParameter("@Price", 1.1));
    cmd.Parameters.Add(new MySqlParameter("@Url", "baidu"));
    cmd.Parameters.Add(new MySqlParameter("@ImageUrl", "baidu.index"));
    int rowsAffected = cmd.ExecuteNonQuery();
    Console.WriteLine(rowsAffected);

    #endregion

    #region 查询

    //string selectSql = @"SELECT * FROM `product`;";
    //cmd.CommandText = selectSql;
    //// 返回查询结果集
    //var reader = cmd.ExecuteReader();
    //while (reader.Read())
    //{
    //    Console.WriteLine(reader["Id"]);
    //    Console.WriteLine(reader["CategoryId"]);
    //    Console.WriteLine(reader["Name"]);
    //    Console.WriteLine(reader["Price"]);
    //    Console.WriteLine(reader["Url"]);
    //    Console.WriteLine(reader["ImageUrl"]);
    //    Console.WriteLine("===============================================");
    //}

    {
        // SqlDataAdapter 适配器
        string selectSql2 = @"SELECT * FROM `product`;";
        cmd.CommandText = selectSql2;
        // 返回查询结果集
        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //DataSet dataSet = new DataSet();
        //adapter.Fill(dataSet);

        DataTable dt = new DataTable();
        adapter.Fill(dt);
    }

    #endregion


    #region 事务

    using (MySqlTransaction transaction=connection.BeginTransaction())
    {
        try
        {
            //todo 
            cmd.Transaction = transaction;
            // do something

            // 提交事务
            transaction.Commit();
        }
        catch (Exception ex)
        {
            // 回滚事务
            transaction.Rollback();
        }
    }

    #endregion
    connection.Close();
    Console.WriteLine(connection.State);

}
Console.ReadKey();