using JuanShoesStore.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanShoesStore.Models
{
    public class Order : TimeLoggingEntity
    {
        public string AppUserId { get; set; }
        [Required]
        [StringLength(maximumLength:15)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string City { get; set; }
        [StringLength(maximumLength: 300)]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        [StringLength(maximumLength:300)]
        public string Note { get; set; }

        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
