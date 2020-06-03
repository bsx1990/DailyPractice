using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Sales.Test
{
    public class Tests
    {
        private readonly Func<Bill, Bill> _wineAndChickenAsAPackageStrategy =
            bill =>
            {
                var shoppingList = bill.ShoppingList;
                static bool IsWine(BillItem item) => item.Name == "wine";
                static bool IsChicken(BillItem item) => item.Name == "chicken";
                if (!shoppingList.Any(IsWine) || !shoppingList.Any(IsChicken))
                {
                    return bill;
                }

                var combinedList = shoppingList
                                   .Where(item =>
                                              !IsWine(item) && !IsChicken(item))
                                   .ToList();
                var wineCount = shoppingList.First(IsWine).Quantity;
                var chickenCount = shoppingList.First(IsChicken).Quantity;
                var minCount = Math.Min(wineCount, chickenCount);
                combinedList.Add(new BillItem
                                 {
                                     Name = "wineAndChicken",
                                     Category = ProductCategory.WineAndChicken,
                                     Price = 23,
                                     Quantity = minCount
                                 });
                if (wineCount > minCount)
                {
                    var wine = shoppingList.First(IsWine);
                    wine.Quantity = wineCount - minCount;
                    combinedList.Add(wine);
                }

                if (chickenCount > minCount)
                {
                    var chicken = shoppingList.First(IsChicken);
                    chicken.Quantity = chickenCount - minCount;
                    combinedList.Add(chicken);
                }

                return new Bill(combinedList);
            };

        private readonly Func<Bill, Bill> _secondHalfPriceForWineAndChickenPackageStrategy =
            bill =>
            {
                var shoppingList = bill.ShoppingList;
                static bool IsWineAndChickenPackage(BillItem item) => item.Name == "wineAndChicken";
                var packageCount = shoppingList.First(IsWineAndChickenPackage).Quantity;
                if (packageCount < 2)
                {
                    return bill;
                }

                var combinedList = shoppingList
                                   .Where(item => !IsWineAndChickenPackage(item))
                                   .ToList();
                combinedList.Add(new BillItem
                                 {
                                     Name = "secondHalfPriceForWineAndChicken",
                                     Category = ProductCategory.WineAndChicken,
                                     Price = 23 * 1.5,
                                     Quantity = packageCount / 2
                                 });
                var leftPackage = packageCount % 2;
                if (leftPackage > 0)
                {
                    var package = shoppingList.First(IsWineAndChickenPackage);
                    package.Quantity = leftPackage;
                    combinedList.Add(package);
                }

                return new Bill(combinedList);
            };

        private readonly Func<Bill, Bill> _drinkDiscountStrategy = 
            bill =>
            {
                var drinkTotalAmount = bill.ShoppingList
                                           .Where(item => item.Category == ProductCategory.Drink)
                                           .Sum(item => item.Price * item.Quantity);
                var drinkDiscount = drinkTotalAmount * (1 - 0.8);
                return new Bill(bill.ShoppingList, bill.TotalAmount - drinkDiscount);
            };

        private readonly Func<Bill, Bill> _meatReductionStrategy = 
            bill =>
            {
                var meatTotalAmount = bill.ShoppingList
                                          .Where(item =>
                                                     item.Category == ProductCategory.Meat &&
                                                     item.Name != "pork")
                                          .Sum(item => item.Price * item.Quantity);
                var meatDiscount = meatTotalAmount >= 60
                    ? 8
                    : meatTotalAmount >= 20
                        ? 2
                        : 0;
                return new Bill(bill.ShoppingList, bill.TotalAmount - meatDiscount);
            };

        private readonly Func<Bill, Bill> _minus50Over200Strategy = 
            bill =>
            {
                var discount = Math.Floor(bill.TotalAmount / 200) * 50;
                return new Bill(bill.ShoppingList, bill.TotalAmount - discount);
            };


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PriceShouleBe204D5WhenGivenTwoWineChicken()
        {
            var myShoppingList = new List<Product>
                                 {
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("light", ProductCategory.Electronics, 100),
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(204.5, actual);
        }
        
        [Test]
        public void PriceShouleBe199_WhenGivenOneWineAndChickenPackage()
        {
            var myShoppingList = new List<Product>
                                 {
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("light", ProductCategory.Electronics, 100),
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(199, actual);
        }
        
        [Test]
        public void PriceShouleBe239_WhenGivenFourWineAndChickenPackage()
        {
            var myShoppingList = new List<Product>
                                 {
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("light", ProductCategory.Electronics, 100),
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(239, actual);
        }
        
        [Test]
        public void PriceShouleBe227D5_WhenGivenThreeWineAndChickenPackage()
        {
            var myShoppingList = new List<Product>
                                 {
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("wine", ProductCategory.Drink, 15),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("chicken", ProductCategory.Meat, 10),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("cola", ProductCategory.Drink, 5),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("pork", ProductCategory.Meat, 25),
                                     new Product("light", ProductCategory.Electronics, 100),
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(227.5, actual);
        }
        
        [Test]
        public void PriceShouleBe154D5_WhenMinus50Over200()
        {
            var myShoppingList = new List<BillItem>
                                 {
                                     new BillItem{ Name = "wine", Category = ProductCategory.Drink, Price = 15, Quantity = 3 },
                                     new BillItem{ Name = "chicken", Category = ProductCategory.Meat, Price = 10, Quantity = 2 },
                                     new BillItem{ Name = "cola", Category = ProductCategory.Drink, Price = 5, Quantity = 2 },
                                     new BillItem{ Name = "pork", Category = ProductCategory.Meat, Price = 25, Quantity = 2 },
                                     new BillItem{ Name = "light", Category = ProductCategory.Electronics, Price = 100, Quantity = 1 },
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .UseSaleStrategy(_minus50Over200Strategy)
                         .TotalAmount;
            Assert.AreEqual(154.5, actual);
        }
        
        [Test]
        public void PriceShouleBe7604D5_WhenMinus50Over200()
        {
            var myShoppingList = new List<BillItem>
                                 {
                                     new BillItem{ Name = "wine", Category = ProductCategory.Drink, Price = 15, Quantity = 3 },
                                     new BillItem{ Name = "chicken", Category = ProductCategory.Meat, Price = 10, Quantity = 2 },
                                     new BillItem{ Name = "cola", Category = ProductCategory.Drink, Price = 5, Quantity = 2 },
                                     new BillItem{ Name = "pork", Category = ProductCategory.Meat, Price = 25, Quantity = 2 },
                                     new BillItem{ Name = "light", Category = ProductCategory.Electronics, Price = 100, Quantity = 100 },
                                 };
            var actual = TotalPriceCalaculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatReductionStrategy)
                         .UseSaleStrategy(_minus50Over200Strategy)
                         .TotalAmount;
            Assert.AreEqual(7604.5, actual);
        }
    }
}
