using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationShop.Models
{
    public class Basket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool HasPaid { get; set; } = false;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
