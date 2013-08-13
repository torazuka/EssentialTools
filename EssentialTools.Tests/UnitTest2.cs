using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;
using System.Linq;
using Moq;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest2
    {

        private Product[] products = { 
            new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
            new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
            new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
            new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
        };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>())).Returns<decimal>(total => total);
            var target = new LinqValueCalculator(mock.Object);

            var result = target.ValueProducts(products);

            Assert.AreEqual(products.Sum(e => e.Price), result);
        }

        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>())).Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0))).Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100))).Returns<decimal>(total => (total * 0.9M));
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive))).Returns<decimal>(total => total - 5);
            var target = new LinqValueCalculator(mock.Object);

            decimal FiveDollerDiscount = target.ValueProducts(createProduct(5));
            decimal TenDollerDiscount = target.ValueProducts(createProduct(10));
            decimal FiftyDollerDiscount = target.ValueProducts(createProduct(50));
            decimal HundredDollerDiscount = target.ValueProducts(createProduct(100));
            decimal FiveHundredDollerDiscount = target.ValueProducts(createProduct(500));

            Assert.AreEqual(5, FiveDollerDiscount, "$5 Fail");
            Assert.AreEqual(5, TenDollerDiscount, "$10 Fail");
            Assert.AreEqual(45, FiftyDollerDiscount, "$50 Fail");
            Assert.AreEqual(95, HundredDollerDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveHundredDollerDiscount, "$500 Fail");
            target.ValueProducts(createProduct(0));
        }
    }
}
