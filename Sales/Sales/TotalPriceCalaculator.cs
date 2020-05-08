using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales
{
    public class TotalPriceCalaculator
    {
        public static double Calc(List<Product> shoppingList, Func<Product,double> strategy)
        {
            return shoppingList.Sum(strategy);
        }
    }
}