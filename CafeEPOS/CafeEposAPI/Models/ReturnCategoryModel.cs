using CafeEposAPI.Data.Entity;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CafeEposAPI.Models
{
    public class ReturnCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? parentId { get; set; }
        public string parentName { get; set; }
        public int archived { get; set; }
    }
}
