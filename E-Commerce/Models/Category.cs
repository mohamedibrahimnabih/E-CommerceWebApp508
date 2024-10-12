using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The filed must be have 3 character")]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
