namespace CafeEposAPI.Models
{
    public class StatsReturnModel
    {
        public decimal Sales { get; set; }
        public int TotalOrders { get; set; }
        public int OrdersPreparing { get; set; }
        public decimal AvgPrepTime { get; set; }
        public DateTime Date { get; set; }
    }
}
