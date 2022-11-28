// See https://aka.ms/new-console-template for more information
using DotNetOrm.DbProxy;
using DotNetOrm.DbProxy.Models;
using System.Linq.Expressions;
using DotNetOrm.DbProxy.Expressions.Extensions;

Console.WriteLine("Hello, World!");

try
{
    DbProxyCore dbProxyCore = new DbProxyCore();

    #region 主键查询
    //var product = dbProxyCore.GetProduct(1);

    // find 
    // var productT = dbProxyCore.Find<Product>(1);

    //var productT1 = dbProxyCore.Find<Program>(1);
    #endregion


    #region 集合查询

    //var products = dbProxyCore.Query<Product>();
    //var companies = dbProxyCore.Query<Company>();

    //// expression
    //Expression<Func<Company, bool>> expression = c => c.Name.Equals("ss") && c.Id == 1;
    //Expression<Func<Company, bool>> expression1 = c => c.Name.Equals("ss");
    //Expression<Func<Company, bool>> expression2 = c => c.Id == 1;

    //Func<Company, bool> func = c => c.Name.Equals("ss");// && c.Id == 1;
    //Func<Company, bool> func2 = M1;

    // expression拼装

    #endregion

    #region 集合查询--按查询条件筛选

    {

        string title = "商品16";
        int id = 20016;
        decimal price = 270;

        Expression<Func<Product, bool>> expression1 = p => p.Name.Equals(title);
        // Expression<Func<Product, bool>> expression11 = p => p.Name.Equals("商品16"); // 字面值
        Expression<Func<Product, bool>> expression2 = c => c.Id == id;
        Expression<Func<Product, bool>> expression3 = c => c.Price == price;

        // 按条件筛选
       // List<Product> products1 = dbProxyCore.Query2<Product>(expression1);
        // 字面值
        //List<Product> products2 = dbProxyCore.Query2<Product>(expression11);

        // 条件拼接
        Expression<Func<Product, bool>> expression4 = ExpressionOperationExtension.And(expression1, expression2);
        Expression<Func<Product, bool>> expression5 = expression4.And(expression3);

        var notExp = expression2.Not();

    }

    // 特性: 指定表名和属性名的映射
    {
        Product entity = dbProxyCore.Find<Product>(123);
    }

    #endregion

    static bool M1(Company company)
    {
        return company.Id == 1; ;
    }

}
catch (Exception ex)
{

}