using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEposAPI.Data.Entity
{
    [Table("orderItems")]
    public class OrderItemsEntity
    {
        [Key]
        public int Id { get; set; }
        public int sysAccountId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public OrderInfoEntity OrderInfo { get; set; }
    }
}
