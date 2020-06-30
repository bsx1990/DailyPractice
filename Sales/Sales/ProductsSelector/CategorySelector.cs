using System.Collections.Generic;
using System.Linq;

namespace Sales.ProductsSelector
{
    public class CategorySelector : IProductsSelector
    {
        public ProductCategory AppliedCategory { get; set; }
        public IList<ProductSelectRule> ExpectProducts { get; set; }

        public IList<BillItem> Select(List<BillItem> billItems)
        {
            return billItems.Where(item =>
            {
                var itemNotInExpectedProducts = ExpectProducts == null || ExpectProducts.All(expectProduct => expectProduct.Name != item.Name);
                return item.Category == AppliedCategory && itemNotInExpectedProducts;
            }).ToList();
        }
    }
}