using System;

namespace Sales
{
    public static class BillHelper
    {
        public static Bill UseSaleStrategy(this Bill bill, Func<Bill, Bill> strategy) => strategy(bill);
    }
}