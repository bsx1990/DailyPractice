using System.Collections.Generic;

namespace Sales.SaleStrategies
{
    public interface ISaleStrategy
    {
        public StrategyAppliedType StrategyAppliedType { get; }
        public Bill Calculate(Bill bill);
    }

    public interface IProductSaleStrategy : ISaleStrategy
    {
        public List<ProductName> AppliedProductNames { get; }
    }

    public interface ICategorySaleStrategy : ISaleStrategy
    {
        public ProductCategory AppliedCategory { get; }
    }

    public interface IBillSaleStrategy : ISaleStrategy
    {
        
    }
}