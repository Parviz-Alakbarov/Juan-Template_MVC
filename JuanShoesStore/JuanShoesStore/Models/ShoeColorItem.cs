namespace JuanShoesStore.Models
{
    public class ShoeColorItem
    {
        public int Id { get; set; }
        public int ShoeId { get; set; }
        public int ColorId { get; set; }


        public Shoe Shoe { get; set; }
        public Color Color { get; set; }
    }
}
