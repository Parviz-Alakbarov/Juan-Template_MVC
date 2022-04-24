using JuanShoesStore.Models;
using System.Collections.Generic;

namespace JuanShoesStore.ViewModels
{
    public class MemberProfileViewModel
    {
        public MemberUpdateViewModel ProfileUpdateVM { get; set; }
        public List<Order> Orders { get; set; }
    }
}
