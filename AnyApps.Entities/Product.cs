using AnyApps.Core.Repository.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyApps.Entities
{
    public class Product : Entity
    {
        public Product()
        {
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
