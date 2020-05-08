using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sales.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PriceShouldBe180_WhenDiscount8Sales()
        {
            var shoppingList = new List<Product>
                               {
                                   new Product("wine",ProductCategory.Drink,15),
                                   new Product("cola",ProductCategory.Drink,5),
                                   new Product("cola",ProductCategory.Drink,5),
                                   new Product("pork",ProductCategory.Meat,25),
                                   new Product("pork",ProductCategory.Meat,25),
                                   new Product("chicken",ProductCategory.Meat,10),
                                   new Product("chicken",ProductCategory.Meat,10),
                                   new Product("chicken",ProductCategory.Meat,10),
                                   new Product("chicken",ProductCategory.Meat,10),
                                   new Product("chicken",ProductCategory.Meat,10),
                                   new Product("light",ProductCategory.Electronics,100),
                               };
            var strategy = new Func<Product, double>(product => product.Price * 0.8);
            var actual = TotalPriceCalaculator.Calc(shoppingList, strategy);
            Assert.AreEqual(180, actual);
        }
    }
}