using JuanShoesStore.Models;
using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Shoe> NewShoes { get; set; }
        public List<Shoe> BestSellerShoes { get; set; }
        public List<Partner> Partners { get; set; }
    }
}
