using System;

namespace JuanShoesStore.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int ShoeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Count { get; set; }

        public AppUser AppUser { get; set; }
        public Shoe Shoe { get; set; }
    }
}
