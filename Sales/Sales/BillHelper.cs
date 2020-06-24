using System;
using Sales.SaleStrategies;

namespace Sales
{
    public static class BillHelper
    {
        public static Bill UseSaleStrategy(this Bill bill, Func<Bill, Bill> strategy) => strategy(bill);
        public static Bill UseSaleStrategy(this Bill bill, ISaleStrategy strategy) => strategy.Calculate(bill);
    }
}