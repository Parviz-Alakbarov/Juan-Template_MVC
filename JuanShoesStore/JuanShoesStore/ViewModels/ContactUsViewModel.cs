using JuanShoesStore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.ViewModels
{
    public class ContactUsViewModel
    {
        public ContactUs ContactUs { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
