using System.ComponentModel.DataAnnotations.Schema;

namespace JuanShoesStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public int ShoeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountedPrice { get; set; }

        public Order Order { get; set; }
        public Shoe Shoe { get; set; }
    }
}
