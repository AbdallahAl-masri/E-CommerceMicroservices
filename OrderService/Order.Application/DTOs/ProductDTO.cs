using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}
