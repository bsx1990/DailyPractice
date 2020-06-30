using System.Collections.Generic;

namespace Sales.ProductsSelector
{
    public interface IProductsSelector
    {
        IList<BillItem> Select(List<BillItem> billItems);
    }
}