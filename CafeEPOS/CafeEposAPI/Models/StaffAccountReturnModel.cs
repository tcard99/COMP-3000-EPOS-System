namespace CafeEposAPI.Models
{
    public class StaffAccountReturnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string staffId { get; set; }
        public string passcode { get; set; }
        public int role { get; set; }
        public int archived { get; set; }
    }
}
