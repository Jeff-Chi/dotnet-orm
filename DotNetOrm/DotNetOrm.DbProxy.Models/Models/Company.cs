using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Models
{
    /// <summary>
    /// 公司
    /// </summary>
    public class Company: BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
    }
}
