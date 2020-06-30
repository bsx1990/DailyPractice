using System;
using System.Linq;
using Sales.ProductsSelector;

namespace Sales.StrategyCalculators
{
    public class EveryReductionCalculator : ICalculator
    {
        public ReductionRule ReductionRule { get; set; }
        
        public Bill Calculate(IProductsSelector productsSelector, Bill bill)
        {
            var billItems = bill.ShoppingList;
            var selectedItems = productsSelector.Select(billItems);
            var originalAmount = selectedItems.Sum(item => item.Price * item.Quantity);
            var benefit = Math.Floor( originalAmount / ReductionRule.Floor ) * ReductionRule.Reduction;
            return new Bill(billItems, bill.TotalAmount - benefit);
        }
    }
}