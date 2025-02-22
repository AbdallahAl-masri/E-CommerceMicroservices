using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entites
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
