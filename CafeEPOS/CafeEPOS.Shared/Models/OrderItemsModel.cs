using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class OrderItemsModel
    {
        public int ItemId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}
