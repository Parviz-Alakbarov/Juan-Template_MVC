using System.Collections.Generic;

namespace JuanShoesStore.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public List<Shoe> Shoes { get; set; }

    }
}
