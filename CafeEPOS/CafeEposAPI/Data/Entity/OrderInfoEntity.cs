using Microsoft.Identity.Client;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEposAPI.Data.Entity
{
    [Table("orderInfo")]
    public class OrderInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int sysAccountId { get; set; }
        public required string waiterName { get; set; }
        public string? table {  get; set; }
        public DateTime date { get; set; }
        public required string status { get; set; }
        public decimal ammountPaid { get; set; }
        public required decimal total { get; set; }
        public Collection<OrderItemsEntity> items { get; set; } = [];
    }
}
