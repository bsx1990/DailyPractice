using System.Collections.Generic;

namespace Sales
{
    public static class TotalPriceCalculator
    {
        public static Bill CreateBill(List<Product> myShoppingList) => new Bill(myShoppingList);
        public static Bill CreateBill(List<BillItem> billItems) => new Bill(billItems);
    }
}