using Microsoft.Identity.Client;
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
        public string waiterName { get; set; }
        public string? table {  get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public string total { get; set; }
    }
}
