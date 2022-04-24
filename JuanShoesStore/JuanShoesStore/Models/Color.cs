using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class Color
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required,StringLength(maximumLength: 7)]
        public string HexValue { get; set; }

        public List<ShoeColorItem> ShoeColorItems { get; set; }

    }
}
