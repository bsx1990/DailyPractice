using System;

namespace Sales
{
    public class Product
    {
        public ProductName Name { get; }
        public ProductCategory Category { get; }
        public double Price { get; }

        public Product(string name, ProductCategory category, double price)
        {
            try
            {
                Name = Enum.Parse<ProductName>(name);
            }
            catch (Exception)
            {
                throw new Exception($"Invalid product name: {name}");
            }
            Category = category;
            Price = price;
        }
        
        public Product(ProductName name, ProductCategory category, double price)
        {
            Name = name;
            Category = category;
            Price = price;
        }
    }
}