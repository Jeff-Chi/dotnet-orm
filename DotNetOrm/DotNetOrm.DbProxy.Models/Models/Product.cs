using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrm.DbProxy.Models
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product : BaseModel
    {
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
