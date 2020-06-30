using System.Collections.Generic;
using NUnit.Framework;
using Sales.ProductsSelector;
using Sales.SaleStrategies;
using Sales.StrategyCalculators;

namespace Sales.Test
{
    public class Tests
    {
        private readonly ISaleStrategy _wineAndChickenAsAPackageStrategy =
            new SaleStrategy
            {
                ProductsSelector = new SpecifiedProductsSelector
                {
                    ProductSelectRules = new List<ProductSelectRule>
                    {
                        new ProductSelectRule { Name = ProductName.Wine, Qty = 1 },
                        new ProductSelectRule { Name = ProductName.Chicken, Qty = 1 },
                    }
                },
                Calculator = new PackageBenefitCalculator
                {
                    NewProductName = ProductName.WineAndChicken,
                    NewProductPrice = 23,
                    NewProductCategory = ProductCategory.WineAndChicken
                }
            };

        private readonly ISaleStrategy _secondHalfPriceForWineAndChickenPackageStrategy =
            new SaleStrategy
            {
                ProductsSelector = new SpecifiedProductsSelector
                {
                    ProductSelectRules = new List<ProductSelectRule>
                    {
                        new ProductSelectRule{ Name = ProductName.WineAndChicken, Qty = 2 }
                    }
                },
                Calculator = new PackageBenefitCalculator
                {
                    NewProductName = ProductName.SecondHalfPriceForWineAndChicken,
                    NewProductPrice = 34.5,
                    NewProductCategory = ProductCategory.WineAndChicken
                }
            };

        private readonly ISaleStrategy _drinkDiscountStrategy =
            new SaleStrategy
            {
                ProductsSelector = new CategorySelector
                {
                    AppliedCategory = ProductCategory.Drink
                },
                Calculator = new DiscountCalculator
                {
                    Discount = 0.8
                }
            };

        private readonly ISaleStrategy _meatExpectPorkReductionStrategy =
            new SaleStrategy
            {
                ProductsSelector = new CategorySelector
                {
                    AppliedCategory = ProductCategory.Meat,
                    ExpectProducts = new List<ProductSelectRule>{ new ProductSelectRule{ Name = ProductName.Pork } }
                },
                Calculator = new StepwiseReductionCalculator
                {
                    ReductionRules = new List<ReductionRule>
                    {
                        new ReductionRule{ Floor = 20, Reduction = 2 },
                        new ReductionRule{ Floor = 60, Reduction = 8 },
                    }
                }
            };

        private readonly ISaleStrategy _minus50Over200Strategy =
            new SaleStrategy
            {
                ProductsSelector = new BillSelector(),
                Calculator = new EveryReductionCalculator
                {
                    ReductionRule = new ReductionRule {Floor = 200, Reduction = 50}
                }
            };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe225WhenNoStrategies()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3},
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2},
                new BillItem {Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
                new BillItem {Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 1},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .TotalAmount;
            Assert.AreEqual(225, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe221WhenAddWineAndChickenAsAPackageStrategy()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3},
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2},
                new BillItem {Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
                new BillItem {Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 1},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .TotalAmount;
            Assert.AreEqual(221, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe209D5WhenAddHalfPriceWithWineAndChickenPackageStrategy()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3},
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2},
                new BillItem {Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
                new BillItem {Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 1},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                .TotalAmount;
            Assert.AreEqual(209.5, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe204D5WhenAddDrinkDiscountStrategy()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Wine, Category = ProductCategory.Drink, Price = 15, Quantity = 3},
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2},
                new BillItem {Name = ProductName.Cola, Category = ProductCategory.Drink, Price = 5, Quantity = 2},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
                new BillItem {Name = ProductName.Light, Category = ProductCategory.Electronics, Price = 100, Quantity = 1},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                .UseSaleStrategy(_drinkDiscountStrategy)
                .TotalAmount;
            Assert.AreEqual(204.5, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe66WhenAddMeatDiscountStrategy()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 2},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                .UseSaleStrategy(_drinkDiscountStrategy)
                .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                .TotalAmount;
            Assert.AreEqual(68, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe102WhenAddMeatDiscountStrategy()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Chicken, Category = ProductCategory.Meat, Price = 10, Quantity = 6},
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 2},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                .UseSaleStrategy(_drinkDiscountStrategy)
                .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                .TotalAmount;
            Assert.AreEqual(102, actual);
        }
        
        [Test]
        [Category("Refactor Test")]
        public void PriceShouldBe150WhenAddMinus50Over200()
        {
            var myShoppingList = new List<BillItem>
            {
                new BillItem {Name = ProductName.Pork, Category = ProductCategory.Meat, Price = 25, Quantity = 8},
            };
            var actual = TotalPriceCalculator
                .CreateBill(myShoppingList)
                .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                .UseSaleStrategy(_drinkDiscountStrategy)
                .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                .UseSaleStrategy(_minus50Over200Strategy)
                .TotalAmount;
            Assert.AreEqual(150, actual);
        }

        [Test]
        [Category("AC")]
        public void PriceShouldBe204D5WhenGivenTwoWineChicken()
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(204.5, actual);
        }
        
        [Test]
        [Category("AC")]
        public void PriceShouleBe199_WhenGivenOneWineAndChickenPackage()
        {
            var myShoppingList = new List<Product>
                                 {
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(199, actual);
        }
        
        [Test]
        [Category("AC")]
        public void PriceShouldBe239_WhenGivenFourWineAndChickenPackage()
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(239, actual);
        }
        
        [Test]
        [Category("AC")]
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .TotalAmount;
            Assert.AreEqual(227.5, actual);
        }
        
        [Test]
        [Category("AC")]
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .UseSaleStrategy(_minus50Over200Strategy)
                         .TotalAmount;
            Assert.AreEqual(154.5, actual);
        }
        
        [Test]
        [Category("AC")]
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
            var actual = TotalPriceCalculator
                         .CreateBill(myShoppingList)
                         .UseSaleStrategy(_wineAndChickenAsAPackageStrategy)
                         .UseSaleStrategy(_secondHalfPriceForWineAndChickenPackageStrategy)
                         .UseSaleStrategy(_drinkDiscountStrategy)
                         .UseSaleStrategy(_meatExpectPorkReductionStrategy)
                         .UseSaleStrategy(_minus50Over200Strategy)
                         .TotalAmount;
            Assert.AreEqual(7604.5, actual);
        }
    }
}
