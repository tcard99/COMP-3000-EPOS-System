using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class OrderInfoReturnModel
    {
        public int Id { get; set; }
        public required string waiterName { get; set; }
        public string? table { get; set; }
        public DateTime date { get; set; }
        public required string status { get; set; }
        public decimal ammountPaid { get; set; }
        public required decimal total { get; set; }
        public Collection<OrderItemsGetReturnModel> items { get; set; }
    }
}
