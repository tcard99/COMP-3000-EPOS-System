using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class OrderItemsGetReturnModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTime placed { get; set; }
        public DateTime updated { get; set; }
        public OrderInfoReturnModel OrderInfo { get; set; }
    }
}
