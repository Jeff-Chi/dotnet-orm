using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Attributes
{
    /// <summary>
    /// 映射数据库表字段名与实体的属性名
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomColumnAttribute: AbstractNameAttribute
    {
        public CustomColumnAttribute(string name) : base(name)
        {
        }
    }
}
