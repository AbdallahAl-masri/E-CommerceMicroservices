using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class OrderDetailsDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public UserDTO User { get; set; }

        [Required]
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
