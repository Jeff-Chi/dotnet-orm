using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Attributes
{
    /// <summary>
    /// 抽象类,所有自定义Name的Attribute都继承此类
    /// </summary>
    public abstract class AbstractNameAttribute: Attribute
    {
        private string _name;
        public AbstractNameAttribute(string name)
        {
            _name = name;
        }
        public string GetName() => _name;
    }
}
