using JuanShoesStore.Models;
using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class ShopViewModel
    {
        public Pagination<Shoe> Shoes { get; set; }
        public List<Brand> Brands { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Color> Colors { get; set; }
        public List<int> FilterColorIds { get; set; }
        public List<int> FilterBrandIds { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public int MaleGenderCount { get; set; }
        public int FemaleGenderCount { get; set; }

    }
}
