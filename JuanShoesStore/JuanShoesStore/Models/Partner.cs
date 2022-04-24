using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class Partner
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [StringLength(maximumLength: 100)]
        public string Image { get; set; }  
        [StringLength(maximumLength: 100)]
        public string RedirectURL { get; set; }
    }
}
