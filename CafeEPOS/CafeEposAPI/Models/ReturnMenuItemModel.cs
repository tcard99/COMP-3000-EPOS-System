namespace CafeEposAPI.Models
{
    public class ReturnMenuItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string price { get; set; }
        public int archived { get; set; }
    }
}
