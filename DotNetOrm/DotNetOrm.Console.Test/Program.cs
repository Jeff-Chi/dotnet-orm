// See https://aka.ms/new-console-template for more information
using DotNetOrm.DbProxy;
using DotNetOrm.DbProxy.Models;

Console.WriteLine("Hello, World!");

try
{
    DbProxyCore dbProxyCore = new DbProxyCore();
    //var product = dbProxyCore.GetProduct(1);
    var productT = dbProxyCore.Find<Product>(1);
    //var productT1 = dbProxyCore.Find<Program>(1);

}
catch (Exception ex)
{

}