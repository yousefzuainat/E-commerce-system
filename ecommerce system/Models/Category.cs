namespace ecommerce_system.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public ICollection<Proudect>? Proudects { get; set; }
    }
}
