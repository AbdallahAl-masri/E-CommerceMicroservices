using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entites
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
