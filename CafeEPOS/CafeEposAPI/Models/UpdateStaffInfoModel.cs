namespace CafeEposAPI.Models
{
    public class UpdateStaffInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StaffId { get; set; }
        public string Passcode { get; set; }
        public int Role { get; set; }
    }
}
