using System.ComponentModel.DataAnnotations;

namespace WexAssessmentApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "'Name' is required")]
        [MaxLength(100, ErrorMessage = "'Name' should have maximum length of '100'")]
        public string? Name { get; set; }

        [Range(0.01, double.PositiveInfinity, ErrorMessage = "'Price' should be a positive decimal")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "'Category' is required")]
        public string? Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "'StockQuantity' should be a non-negative integer")]
        public int StockQuantity { get; set; }
    }
}
