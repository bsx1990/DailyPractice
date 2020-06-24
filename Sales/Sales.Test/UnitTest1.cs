using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sales.SaleStrategies;

namespace Sales.Test
{
    public class Tests
    {
        private readonly IProductSaleStrategy _wineAndChickenAsAPackageStrategy =
            new ProductSaleStrategy(new List<ProductName> {ProductName.Wine, ProductName.Chicken})
            {
                Calculate = bill =>
                {
                    var shoppingList = bill.ShoppingList;
                    static bool IsWine(BillItem item) => item.Name == ProductName.Wine;
                    static bool IsChicken(BillItem item) => item.Name == ProductName.Chicken;
                    if (!shoppingList.Any(IsWine) || !shoppingList.Any(IsChicken)) { return bill; }

                    var combinedList = shoppingList
                        .Where(item => !IsWine(item) && !IsChicken(item))
                        .ToList();
                    var wineCount = shoppingList.First(IsWine).Quantity;
                    var chickenCount = shoppingList.First(IsChicken).Quantity;
                    var minCount = Math.Min(wineCount, chickenCount);
                    combinedList.Add(new BillItem
                    {
                        Name = ProductName.WineAndChicken,
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

                    if (chickenCount <= minCount) { return new Bill(combinedList); }
                    
                    var chicken = shoppingList.First(IsChicken);
                    chicken.Quantity = chickenCount - minCount;
                    combinedList.Add(chicken);
                    return new Bill(combinedList);
                }
            };

        private readonly IProductSaleStrategy _secondHalfPriceForWineAndChickenPackageStrategy =
            new ProductSaleStrategy(new List<ProductName> {ProductName.WineAndChicken})
            {
                Calculate = bill =>
                {
                    var shoppingList = bill.ShoppingList;
                    static bool IsWineAndChickenPackage(BillItem item) => item.Name == ProductName.WineAndChicken;
                    var packageCount = shoppingList.First(IsWineAndChickenPackage).Quantity;
                    if (packageCount < 2) { return bill; }

                    var combinedList = shoppingList
                        .Where(item => !IsWineAndChickenPackage(item))
                        .ToList();
                    combinedList.Add(new BillItem
                    {
                        Name = ProductName.SecondHalfPriceForWineAndChicken,
                        Category = ProductCategory.WineAndChicken,
                        Price = 23 * 1.5,
                        Quantity = packageCount / 2
                    });
                    var leftPackage = packageCount % 2;
                    if (leftPackage <= 0)
                    {
                        return new Bill(combinedList);
                    }

                    var package = shoppingList.First(IsWineAndChickenPackage);
                    package.Quantity = leftPackage;
                    combinedList.Add(package);
                    return new Bill(combinedList);
                }
            };

        private readonly ICategorySaleStrategy _drinkDiscountStrategy =
            new CategorySaleStrategy(ProductCategory.Drink)
            {
                Calculate = bill =>
                {
                    var drinkTotalAmount = bill.ShoppingList
                        .Where(item => item.Category == ProductCategory.Drink)
                        .Sum(item => item.Price * item.Quantity);
                    var drinkDiscount = drinkTotalAmount * (1 - 0.8);
                    return new Bill(bill.ShoppingList, bill.TotalAmount - drinkDiscount);
                }
            };

        private readonly ICategorySaleStrategy _meatReductionStrategy =
            new CategorySaleStrategy(ProductCategory.Drink)
            {
                Calculate = bill =>
                {
                    var meatTotalAmount = bill.ShoppingList
                        .Where(item => item.Category == ProductCategory.Meat && item.Name != ProductName.Pork)
                        .Sum(item => item.Price * item.Quantity);
                    var meatDiscount = meatTotalAmount >= 60
                        ? 8
                        : meatTotalAmount >= 20 ? 2 : 0;
                    return new Bill(bill.ShoppingList, bill.TotalAmount - meatDiscount); 
                }
            };

        private readonly IBillSaleStrategy _minus50Over200Strategy =
            new BillSaleStrategy
            {
                Calculate = bill =>
                {
                    var discount = Math.Floor(bill.TotalAmount / 200) * 50;
                    return new Bill(bill.ShoppingList, bill.TotalAmount - discount);
                }
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
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Light, ProductCategory.Electronics, 100),
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
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Light, ProductCategory.Electronics, 100),
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
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Wine, ProductCategory.Drink, 15),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Chicken, ProductCategory.Meat, 10),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Cola, ProductCategory.Drink, 5),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Pork, ProductCategory.Meat, 25),
                                     new Product(ProductName.Light, ProductCategory.Electronics, 100),
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
                                     new BillItem{ Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3 },
                                     new BillItem{ Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 1 },
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
                                     new BillItem{ Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3 },
                                     new BillItem{ Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2 },
                                     new BillItem{ Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 100 },
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
