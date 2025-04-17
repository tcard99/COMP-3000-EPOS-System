using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class MakeOrderModel
    {
        public string? WaiterName { get; set; }
        public string? Table { get; set; }
        public List<MakeOrderItemModel> Items { get; set; } = [];
    }
}
