using System;
using Sales.ProductsSelector;
using Sales.StrategyCalculators;

namespace Sales.SaleStrategies
{
    public interface ISaleStrategy
    {
        public IProductsSelector ProductsSelector { get; set; }
        public ICalculator Calculator { get; set; }
        Bill Calculate(Bill bill);
    }
    
    public class SaleStrategy : ISaleStrategy
    {
        public IProductsSelector ProductsSelector { get; set; }
        public ICalculator Calculator { get; set; }
        public Bill Calculate(Bill bill)
        {
            if (Calculator == null) { throw new Exception("No valid calculator"); }

            return Calculator.Calculate(ProductsSelector, bill);
        }
    }
}