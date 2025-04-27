using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class UpdateStaffAccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StaffId { get; set; }
        public string Passcode { get; set; }
        public int Role { get; set; }
    }
}
