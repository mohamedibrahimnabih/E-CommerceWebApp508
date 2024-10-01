namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public double Rate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
