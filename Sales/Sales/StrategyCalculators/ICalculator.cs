using Sales.ProductsSelector;

namespace Sales.StrategyCalculators
{
    public interface ICalculator
    {
        public Bill Calculate(IProductsSelector productsSelector, Bill bill);
    }
}