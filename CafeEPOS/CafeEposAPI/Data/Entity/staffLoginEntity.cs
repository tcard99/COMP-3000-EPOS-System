using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEposAPI.Data.Entity
{
    [Table("staffLogin")]
    public class staffLoginEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string staffId { get; set; }
        public string passcode { get; set; }
        public int sysAccountId { get; set; }
        public int role { get; set; }
        public int archived { get; set; }
        public SystemAccountEntity systemAccount { get; set; }
    }
}
