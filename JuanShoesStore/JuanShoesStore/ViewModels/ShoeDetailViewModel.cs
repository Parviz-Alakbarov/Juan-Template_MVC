using JuanShoesStore.Models;
using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class ShoeDetailViewModel
    {
        public Shoe Shoe { get; set; }
        public List<Shoe> RelatedShoes { get; set; }
        public ShoeComment Comment { get; set; }
    }
}
