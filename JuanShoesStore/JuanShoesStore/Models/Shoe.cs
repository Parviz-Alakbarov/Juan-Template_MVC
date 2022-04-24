using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanShoesStore.Models
{
    public class Shoe : TimeLoggingEntity
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        [StringLength(maximumLength:100)]
        public string Image { get; set; }
        [StringLength(maximumLength: 500)]
        public string Desc { get; set; }
        [Required]
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public bool IsNew { get; set; }
        [Range(1, 5)]
        [Column(TypeName = "decimal(2,1)")]
        public int Rate { get; set; }
        public bool IsAvailable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool? Gender { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public List<int> ColorIds { get; set; } = new List<int>();


        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public List<ShoeColorItem> ShoeColorItems { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<ShoeComment> ShoeComments { get; set; }


    }
}
