using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales
{
    public class TotalPriceCalaculator
    {
        public static double Calc(List<Product> shoppingList, Func<Product, double> productStrategy, Func<List<Product>, double, double> listStrategy)
        {
            var result = shoppingList.Sum(productStrategy);
            return listStrategy(shoppingList, result);
        }
    }
}