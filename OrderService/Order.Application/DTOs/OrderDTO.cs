using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
