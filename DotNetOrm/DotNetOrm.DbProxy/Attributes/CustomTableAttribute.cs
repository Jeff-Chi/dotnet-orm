using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Attributes
{
    /// <summary>
    /// 映射实体与数据库表的名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomTableAttribute: Attribute
    {
        private readonly string _name;

        public CustomTableAttribute(string name)
        {
            _name = name;
        }

        public string GetName() => _name;
    }
}
