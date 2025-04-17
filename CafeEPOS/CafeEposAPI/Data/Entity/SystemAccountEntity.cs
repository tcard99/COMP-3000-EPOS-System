using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CafeEposAPI.Data.Entity
{
    [Table("SystemAccount")]
    public class SystemAccountEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Token { get; set; }
        public Collection<staffLoginEntity> Staff { get; set; } = [];

    }
}
