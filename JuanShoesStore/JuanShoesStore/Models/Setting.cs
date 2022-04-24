using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanShoesStore.Models
{
    public class Setting
    {
        public int Id { get; set;}
        
        [Required]
        [StringLength(maximumLength:50)]
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength:500)]
        public string Value { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
