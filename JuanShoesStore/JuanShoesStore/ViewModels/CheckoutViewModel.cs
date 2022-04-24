using JuanShoesStore.Models;
using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public BasketViewModel BasketVM { get; set; }
    }
}
