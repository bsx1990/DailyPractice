namespace Sales.SaleStrategies
{
    public abstract class SaleStrategy : ISaleStrategy
    {
        public abstract StrategyAppliedType StrategyAppliedType { get; }
        public abstract Bill Calculate(Bill bill);
    }
}