using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEposAPI.Data.Entity
{
    [Table("cateogry")]
    public class categoryEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? parentId { get; set; }
        public int sysAccountId { get; set; }
        public int archived { get; set; }
    }
}
