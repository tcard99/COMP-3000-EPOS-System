using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CafeEposAPI.Data.Entity
{
    [Table("cateogry")]
    public class categoryEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? parentId { get; set; }
        [JsonIgnore]
        public int sysAccountId { get; set; }
        public int archived { get; set; }
        public Collection<menuEntity> menuItems { get; set; } = [];
        public categoryEntity? parentCategory { get; set; }
    }
}
