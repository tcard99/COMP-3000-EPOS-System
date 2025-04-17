using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class UpdateAmountPaidReturnModel
    {
        public int Id { get; set; }
        public decimal AmountDue { get; set; }
        public decimal Total { get; set; }
    }
}
