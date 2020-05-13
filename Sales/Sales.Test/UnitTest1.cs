using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Sales.Test
{
    public class Tests
    {
        private List<Product> MyShoppingList { get; set; }
        private List<Product> MyShoppingList2 { get; set; }

        [SetUp]
        public void Setup()
        {
            MyShoppingList = new List<Product>
                             {
                                 new Product("wine", ProductCategory.Drink, 15),
                                 new Product("cola", ProductCategory.Drink, 5),
                                 new Product("cola", ProductCategory.Drink, 5),
                                 new Product("pork", ProductCategory.Meat, 25),
                                 new Product("pork", ProductCategory.Meat, 25),
                                 new Product("chicken", ProductCategory.Meat, 10),
                                 new Product("chicken", ProductCategory.Meat, 10),
                                 new Product("chicken", ProductCategory.Meat, 10),
                                 new Product("chicken", ProductCategory.Meat, 10),
                                 new Product("chicken", ProductCategory.Meat, 10),
                                 new Product("light", ProductCategory.Electronics, 100),
                             };
            MyShoppingList2 = new List<Product>
                             {
                                 new Product("wine", ProductCategory.Drink, 15),
                                 new Product("cola", ProductCategory.Drink, 5),
                                 new Product("cola", ProductCategory.Drink, 5),
                                 new Product("pork", ProductCategory.Meat, 5),
                                 new Product("pork", ProductCategory.Meat, 15),
                             };
        }

        [Test]
        public void PriceShouldBe212_WhenDrinkDiscount8AndMeatReduce8()
        {
            var drink8DiscountStrategy = new Func<Product, double>(product => product.Category == ProductCategory.Drink
                                              ? product.Price * 0.8
                                              : product.Price);
            var meatReduceStrategy = new Func<List<Product>, double, double>((products, totalPrice) =>
                                                                     {
                                                                         var totalPriceOfMeat = products.Sum(product => 
                                                                                                                 product.Category == ProductCategory.Meat && product.Name != "pork"
                                                                                                                     ? product.Price 
                                                                                                                     : 0);
                                                                         var meatDiscount = totalPriceOfMeat >= 60 ? 8 : totalPriceOfMeat >= 20 ? 2 : 0;
                                                                         return totalPrice - meatDiscount;
                                                                     });
            var actual = TotalPriceCalaculator.Calc(MyShoppingList, drink8DiscountStrategy, meatReduceStrategy);
            Assert.AreEqual(218, actual);
        }
        
        [Test]
        public void PriceShouldBe38_WhenDrinkDiscount8AndMeatReduce8()
        {
            var drink8DiscountStrategy = new Func<Product, double>(product => product.Category == ProductCategory.Drink
                                              ? product.Price * 0.8
                                              : product.Price);
            var meatReduceStrategy = new Func<List<Product>, double, double>((products, totalPrice) =>
                                                                     {
                                                                         var totalPriceOfMeat = products.Sum(product => 
                                                                                                                 product.Category == ProductCategory.Meat
                                                                                                                     ? product.Price 
                                                                                                                     : 0);
                                                                         var meatDiscount = totalPriceOfMeat >= 60 ? 8 : totalPriceOfMeat >= 20 ? 2 : 0;
                                                                         return totalPrice - meatDiscount;
                                                                     });
            var actual = TotalPriceCalaculator.Calc(MyShoppingList2, drink8DiscountStrategy, meatReduceStrategy);
            Assert.AreEqual(38, actual);
        }
    }
}