using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Domain.Entities
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
    }
}
