using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class Brand : TimeLoggingEntity
    {
        [Required]
        [StringLength(maximumLength:40)]
        public string Name { get; set; }


        public List<Shoe> Shoes { get; set; }
    }
}
