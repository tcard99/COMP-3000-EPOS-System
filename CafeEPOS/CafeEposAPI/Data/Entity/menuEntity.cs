using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CafeEposAPI.Data.Entity
{
    [Table("menu")]
    public class menuEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int categortyId { get; set; }
        public categoryEntity category { get; set; } = null!;
        public string price { get; set; }
        [JsonIgnore]
        public int sysAccountId { get; set; }
        public int archived { get; set; }
    }
}
