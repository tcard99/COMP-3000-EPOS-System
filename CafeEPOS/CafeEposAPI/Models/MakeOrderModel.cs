namespace CafeEposAPI.Models
{
    public class MakeOrderModel
    {
        public string? WaiterName {  get; set; }
        public string? Table {  get; set; }
        public required List<OrderItemsMakeOrderModel> Items { get; set; } = [];
    }
}
