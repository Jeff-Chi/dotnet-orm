// See https://aka.ms/new-console-template for more information
using DotNetOrm.DbProxy;
using DotNetOrm.DbProxy.Models;
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

try
{
    DbProxyCore dbProxyCore = new DbProxyCore();

    #region 主键查询
    //var product = dbProxyCore.GetProduct(1);
    var productT = dbProxyCore.Find<Product>(1);
    //var productT1 = dbProxyCore.Find<Program>(1);
    #endregion


    #region 集合查询

    var products = dbProxyCore.Query<Product>();
    var companies  = dbProxyCore.Query<Company>();

    // expression
    Expression<Func<Company, bool>> expression =  c => c.Name.Equals("ss") && c.Id == 1;
    Expression<Func<Company, bool>> expression1 =  c => c.Name.Equals("ss");
    Expression<Func<Company, bool>> expression2 =  c => c.Id == 1;

    Func<Company, bool> func = c => c.Name.Equals("ss");// && c.Id == 1;
    Func<Company, bool> func2 = M1;

    // expression拼装




    #endregion

    static bool M1(Company company)
    {
        return company.Id == 1; ;
    }

}
catch (Exception ex)
{

}