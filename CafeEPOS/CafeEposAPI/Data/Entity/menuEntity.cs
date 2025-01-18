using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEposAPI.Data.Entity
{
    [Table("menu")]
    public class menuEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int categortyId { get; set; }
        public string price { get; set; }
        public int sysAccountId { get; set; }

    }
}
