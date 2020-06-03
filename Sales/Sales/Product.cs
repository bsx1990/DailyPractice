namespace Sales
{
    public class Product
    {
        public string Name { get; }
        public ProductCategory Category { get; }
        public double Price { get; }

        public Product(string name, ProductCategory category, double price)
        {
            Name = name;
            Category = category;
            Price = price;
        }
    }
}