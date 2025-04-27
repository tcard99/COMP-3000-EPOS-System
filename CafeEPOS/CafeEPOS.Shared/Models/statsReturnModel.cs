using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class statsReturnModel
    {
        public decimal Sales { get; set; }
        public int TotalOrders { get; set; }
        public int OrdersPreparing { get; set; }
        public decimal AvgPrepTime { get; set; }
        public DateTime Date { get; set; }
    }
}
