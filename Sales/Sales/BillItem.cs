namespace Sales
{
    public class BillItem
    {
        public ProductName Name { get; set; }
        public ProductCategory Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}