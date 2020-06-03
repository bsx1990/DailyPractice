using System.Collections.Generic;

namespace Sales
{
    public static class TotalPriceCalaculator
    {
        public static Bill CreateBill(List<Product> myShoppingList) => new Bill(myShoppingList);
        public static Bill CreateBill(List<BillItem> billItems) => new Bill(billItems);
    }
}