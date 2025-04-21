using System.Text.Json.Serialization;

namespace CafeEposAPI.Models
{
    public class OrderInfoReturnModel
    {
        public int Id { get; set; }
        public required string waiterName { get; set; }
        public string? table { get; set; }
        public DateTime date { get; set; }
        public required string status { get; set; }
        public decimal ammountPaid { get; set; }
        public required decimal total { get; set; }
    }
}
