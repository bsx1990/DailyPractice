using System;
using System.Collections.Generic;

namespace Sales.SaleStrategies
{
    public abstract class SaleStrategy : ISaleStrategy
    {
        public StrategyAppliedType StrategyAppliedType { get; }
        public Func<Bill, Bill> Calculate { get; set; }
        
        protected SaleStrategy(StrategyAppliedType strategyAppliedType)
        {
            StrategyAppliedType = strategyAppliedType;
        }
    }

    public class CategorySaleStrategy : SaleStrategy, ICategorySaleStrategy
    {
        public ProductCategory AppliedCategory { get; }

        public CategorySaleStrategy(ProductCategory appliedCategory) : base(StrategyAppliedType.CategorySaleStrategy)
        {
            AppliedCategory = appliedCategory;
        }
    }

    public class BillSaleStrategy : SaleStrategy, IBillSaleStrategy
    {
        public BillSaleStrategy() : base(StrategyAppliedType.BillSaleStrategy) { }
    }

    public class ProductSaleStrategy : SaleStrategy, IProductSaleStrategy
    {
        public List<ProductName> AppliedProductNames { get; }
        public ProductSaleStrategy(List<ProductName> appliedProductNames) : base(StrategyAppliedType.ProductSaleStrategy)
        {
            AppliedProductNames = appliedProductNames;
        }
    }
}