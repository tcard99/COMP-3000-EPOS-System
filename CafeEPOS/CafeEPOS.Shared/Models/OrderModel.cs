using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class OrderModel
    {
        public string? WaiterName { get; set; }
        public string? Table { get; set; }
        public List<OrderItemsModel> Items { get; set; } = [];
        public decimal Total
        {
            get
            {
                return Items.Sum(x => x.Price);
            }
        }
    }
}
