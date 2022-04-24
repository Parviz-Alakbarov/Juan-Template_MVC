using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemDetaildViewModel> BasketItemDVM { get; set; }
        public decimal BasketTotalPrice { get; set; }
        public decimal VatPercent { get; set; }
        public decimal TaxPercent { get; set; }
    }
}
