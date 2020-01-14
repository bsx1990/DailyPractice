using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesStrategy
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ShouldBe35_WhenAVipCustomerBought50()
        {
            var customer = new Customer {Type = CustomerType.Vip};
            var sumCaculator = new SumCaculator();

            var actual = sumCaculator.GetSum(customer.Type, 50);
            
            Assert.AreEqual(35,actual);
        }

        [TestMethod]
        public void ShouldBe60_WhenAVipCustomerBought100()
        {
            var customer = new Customer {Type = CustomerType.Vip};
            var sumCaculator = new SumCaculator();

            var actual = sumCaculator.GetSum(customer.Type, 100);
            
            Assert.AreEqual(60,actual);
        }

        [TestMethod]
        public void ShouldBe17_WhenANewCustomerBought20()
        {
            var customer = new Customer {Type = CustomerType.New};
            var sumCaculator = new SumCaculator();

            var actual = sumCaculator.GetSum(customer.Type, 20);
            
            Assert.AreEqual(17,actual);
        }

        [TestMethod]
        public void ShouldBe7_WhenANewCustomerBought7()
        {
            var customer = new Customer {Type = CustomerType.New};
            var sumCaculator = new SumCaculator();

            var actual = sumCaculator.GetSum(customer.Type, 7);
            
            Assert.AreEqual(7,actual);
        }

        [TestMethod]
        public void ShouldBe9_WhenAOldCustomerBought10()
        {
            var customer = new Customer {Type = CustomerType.Old};
            var sumCaculator = new SumCaculator();

            var actual = sumCaculator.GetSum(customer.Type, 10);
            
            Assert.AreEqual(9,actual);
        }
    }

    public class SumCaculator
    {
        private readonly Dictionary<CustomerType, Func<double, double>> _caculatFuncs =
            new Dictionary<CustomerType, Func<double, double>>
            {
                {CustomerType.Vip, total => total >= 100 ? total - 40 : total * 0.7},
                {CustomerType.New, total => total >= 10 ? total - 3 : total},
                {CustomerType.Old, total => total*0.9},
            };

        public double GetSum(CustomerType customerType, int total)
        {
            return _caculatFuncs.ContainsKey(customerType)
                ? _caculatFuncs[customerType].Invoke(total)
                : total;
        }
    }

    public enum CustomerType
    {
        Vip,
        New,
        Old
    }

    public class Customer
    {
        public CustomerType Type { get; set; }
    }
}
