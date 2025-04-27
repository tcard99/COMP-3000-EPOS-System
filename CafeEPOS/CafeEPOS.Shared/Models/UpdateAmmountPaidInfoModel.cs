using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class UpdateAmmountPaidInfoModel
    {
        public int OrderId { get; set; }
        public decimal AmmountPaid { get; set; }
    }
}
