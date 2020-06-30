using System.Linq;
using Sales.ProductsSelector;

namespace Sales.StrategyCalculators
{
    public class DiscountCalculator : ICalculator
    {
        public double Discount { get; set; }
        
        public Bill Calculate(IProductsSelector productsSelector, Bill bill)
        {
            var billItems = bill.ShoppingList;
            var selectedItems = productsSelector.Select(billItems);
            var originalAmount = selectedItems.Sum(item => item.Price * item.Quantity);
            var benefit = originalAmount * ( 1 - Discount );
            return new Bill(billItems, bill.TotalAmount - benefit);
        }
    }
}