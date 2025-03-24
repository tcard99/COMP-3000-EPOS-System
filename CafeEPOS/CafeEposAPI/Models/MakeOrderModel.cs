namespace CafeEposAPI.Models
{
    public class MakeOrderModel
    {
        public string WaiterName {  get; set; }
        //public string? Table {  get; set; }
        public List <OrderItemsMakeOrderModel> Items { get; set; }
    }
}
