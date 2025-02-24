using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        [Required]
        public List<OrderItemDTO> Products { get; set; } = new List<OrderItemDTO>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
