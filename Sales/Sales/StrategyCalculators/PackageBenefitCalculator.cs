using System;
using System.Collections.Generic;
using System.Linq;
using Sales.ProductsSelector;

namespace Sales.StrategyCalculators
{
    public class PackageBenefitCalculator : ICalculator
    {
        public string NewProductName { get; set; }
        public double NewProductPrice { get; set; }
        public ProductCategory NewProductCategory { get; set; }
        public Bill Calculate(IProductsSelector productsSelector, Bill bill)
        {
            if ((SpecifiedProductsSelector)productsSelector == null) { throw new ArgumentException("product selector is not suitable with current calculator"); }
            var (productsAfterPackaged, benefit) = Package((SpecifiedProductsSelector)productsSelector, bill);
            return new Bill(productsAfterPackaged, bill.TotalAmount - benefit);
        }

        private (List<BillItem>, double) Package(SpecifiedProductsSelector productsSelector, Bill bill)
        {
            var billItems = bill.ShoppingList;
            var canConvertedQty = GetConvertedQty(billItems, productsSelector.ProductSelectRules);
            if (canConvertedQty == 0) { return (billItems, 0); }

            var packageOriginalPrice = productsSelector.ProductSelectRules.Sum(productSelectRule => billItems.First(item => item.Name == productSelectRule.Name).Price * productSelectRule.Qty);
            var resultItems = RemoveOriginProductFromBillItems(billItems, productsSelector.ProductSelectRules, canConvertedQty);
            var newPackageItem = new BillItem
            {
                Name = NewProductName,
                Category = NewProductCategory,
                Price = NewProductPrice,
                Quantity = canConvertedQty
            };
            resultItems.Add(newPackageItem);
            
            var totalBenefit = (packageOriginalPrice - newPackageItem.Price) * newPackageItem.Quantity;
            return (resultItems, totalBenefit);
        }

        private static List<BillItem> RemoveOriginProductFromBillItems(List<BillItem> billItems, IEnumerable<ProductSelectRule> appliedProductNames, in int canConvertedQty)
        {
            var result = billItems;
            foreach (var productNameAndQty in appliedProductNames)
            {
                var originProduct = result.First(item => item.Name == productNameAndQty.Name);
                var originQty = originProduct.Quantity;
                var removedQty = canConvertedQty * productNameAndQty.Qty;
                if (originQty == removedQty)
                {
                    result.Remove(originProduct);
                }
                else
                {
                    originProduct.Quantity -= removedQty;
                }
            }

            return result;
        }

        private static int GetConvertedQty(IReadOnlyCollection<BillItem> billItems, ICollection<ProductSelectRule> productSelectRules)
        {
            if (productSelectRules.Count == 0) { return 0; }

            var result = int.MaxValue;
            foreach (var productSelectRule in productSelectRules)
            {
                if (billItems.All(item => item.Name != productSelectRule.Name)) { return 0; }

                var convertedQty = billItems.First(product => product.Name == productSelectRule.Name).Quantity / productSelectRule.Qty;
                result = Math.Min(result, convertedQty);
                if (result == 0) { return result; }
            }

            return result;
        } 
    }
}