using System.Collections.Generic;
using System.Linq;
using Sales.ProductsSelector;
using Sales.SaleStrategies;

namespace Sales.StrategyCalculators
{
    public class StepwiseReductionCalculator : ICalculator
    {
        public IList<ReductionRule> ReductionRules { get; set; }
        
        public Bill Calculate(IProductsSelector productsSelector, Bill bill)
        {
            var billItems = bill.ShoppingList;
            var selectedItems = productsSelector.Select(billItems);
            var originalAmount = selectedItems.Sum(item => item.Price * item.Quantity);
            var benefit = GetMaxReduction(ReductionRules, originalAmount);
            return new Bill(billItems, bill.TotalAmount - benefit);
        }

        private double GetMaxReduction(IEnumerable<ReductionRule> reductionRules, double originalAmount)
        {
            var matchedRule = reductionRules?.OrderByDescending(rule => rule.Floor)
                .FirstOrDefault(rule1 => rule1.Floor <= originalAmount);

            return matchedRule?.Reduction ?? 0.0;
        }
    }

    public class ReductionRule
    {
        public double Floor { get; set; }
        public double Reduction { get; set; }
    }
}