namespace Sales
{
    public class Product
    {
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public double Price { get; set; }

        public Product(string name, ProductCategory category, double price)
        {
            Name = name;
            Category = category;
            Price = price;
        }
    }
}