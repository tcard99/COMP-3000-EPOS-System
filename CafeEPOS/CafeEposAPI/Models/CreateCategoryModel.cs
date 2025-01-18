namespace CafeEposAPI.Models
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }
        public int? parentId {  get; set; }
    }
}
