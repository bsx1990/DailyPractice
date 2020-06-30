using System.Collections.Generic;

namespace Sales.ProductsSelector
{
    public class SpecifiedProductsSelector : IProductsSelector
    {
        public IList<ProductSelectRule> ProductSelectRules { get; set; }
        public IList<BillItem> Select(List<BillItem> billItems)
        {
            throw new System.NotImplementedException();
        }
    }
}