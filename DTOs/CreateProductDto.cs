using System.ComponentModel.DataAnnotations;
namespace TechGearAPI.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage ="Name is Required")]
        [StringLength(100 , MinimumLength = 3,
            ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100,MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Range(1,10000, 
            ErrorMessage ="Price cant be less than 1 and greater than 10000")]
        public double Price { get; set; }
        [Range(0,int.MaxValue)]
        public int StockQuantity { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;
    }
}
