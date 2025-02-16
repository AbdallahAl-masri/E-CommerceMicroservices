using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class OrderDetailsDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
