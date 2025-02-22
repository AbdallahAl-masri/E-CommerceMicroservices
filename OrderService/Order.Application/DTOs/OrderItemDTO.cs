using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class OrderItemDTO
    {
        [Required, Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
