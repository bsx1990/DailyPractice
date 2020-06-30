using System.Collections.Generic;

namespace Sales.ProductsSelector
{
    public class BillSelector : IProductsSelector
    {
        public IList<BillItem> Select(List<BillItem> billItems)
        {
            return billItems;
        }
    }
}