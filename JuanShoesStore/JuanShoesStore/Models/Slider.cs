using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanShoesStore.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string Subtitle { get; set; }
        [StringLength(maximumLength: 50)]
        [Required]
        public string Title { get; set; }
        [StringLength(maximumLength: 100)]
        public string Desc { get; set; }
        [StringLength(maximumLength: 30)]
        [Required]
        public string BtnText { get; set; }
        [StringLength(maximumLength: 200)]
        [Required]
        public string RedirectURL { get; set; }
        [StringLength(maximumLength:100)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }
    }
}
