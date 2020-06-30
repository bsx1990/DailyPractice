using Sales.SaleStrategies;

namespace Sales
{
    public static class BillHelper
    {
        public static Bill UseSaleStrategy(this Bill bill, ISaleStrategy strategy) => strategy.Calculate(bill);
    }
}