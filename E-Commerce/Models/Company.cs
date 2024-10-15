using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
