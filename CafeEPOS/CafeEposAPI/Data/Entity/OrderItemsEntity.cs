using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CafeEposAPI.Data.Entity
{
    [Table("orderItems")]
    public class OrderItemsEntity
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public int sysAccountId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTime placed { get; set; }
        public DateTime updated { get; set; }
        [JsonIgnore]
        public OrderInfoEntity OrderInfo { get; set; }
    }
}
