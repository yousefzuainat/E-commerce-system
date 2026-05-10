namespace ecommerce_system.Models
{
    public class Proudect
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Descrption { get; set; }  

        public string Img { get; set; }

        public decimal price { get; set; }

        public int CategoryId { get; set; } //fk


    }
}
