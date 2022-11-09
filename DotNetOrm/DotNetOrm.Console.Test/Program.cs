// See https://aka.ms/new-console-template for more information
using DotNetOrm.DbProxy;
using DotNetOrm.DbProxy.Models;

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

    var list = dbProxyCore.Query<Product>();

    #endregion

}
catch (Exception ex)
{

}