using System.ComponentModel.DataAnnotations;

namespace RealStateAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Category name can't be null or empty")]  // name is required and cannot be null or empty
        public string? Name { get; set; }
        [Required(ErrorMessage = "Category imageUrl can't be null or empty")]
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Category description can't be null or empty")]
        public string? Description { get; set; }

        public ICollection<Property> Properties { get; set; } // this will be a list of properties
    }
}
