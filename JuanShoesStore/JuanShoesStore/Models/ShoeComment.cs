using System;
using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class ShoeComment
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int ShoeId { get; set; }
        [Required]  
        [Range(1, 5)]
        public int Rate { get; set; }
        [StringLength(maximumLength:300)]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser AppUser { get; set; }
        public Shoe Shoe { get; set; }
    }
}
