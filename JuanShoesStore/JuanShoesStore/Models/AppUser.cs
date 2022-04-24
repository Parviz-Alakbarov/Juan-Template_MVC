using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
        [StringLength(maximumLength: 50)]
        public string Country { get; set; }
        [StringLength(maximumLength: 30)]
        public string City { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<ShoeComment> ShoeComments { get; set; }
    }
}
