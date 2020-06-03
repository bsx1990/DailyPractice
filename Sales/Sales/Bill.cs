using System.Collections.Generic;
using System.Linq;

namespace Sales
{
    public class Bill
    {
        public Bill(List<Product> shoppingList)
        {
            ShoppingList = shoppingList.GroupBy(product => product.Name)
                                       .Select(group =>
                                                   new BillItem
                                                   {
                                                       Name = group.First().Name,
                                                       Category = group.First().Category,
                                                       Price = group.First().Price,
                                                       Quantity = group.Count()
                                                   }).ToList();
            TotalAmount = ShoppingList.Sum(item => item.Price * item.Quantity);
        }

        public Bill(List<BillItem> shoppingList)
        {
            ShoppingList = shoppingList;
            TotalAmount = ShoppingList.Sum(item => item.Price * item.Quantity);
        }

        public Bill(List<BillItem> shoppingList, double totalAmount)
        {
            ShoppingList = shoppingList;
            TotalAmount = totalAmount;
        }

        public double TotalAmount { get; }
        public List<BillItem> ShoppingList { get; }
    }
}