using Microsoft.VisualStudio.TestTools.UnitTesting;
using My.Api.Models;
using My.Api.Trolleys;
using System;
using System.Collections.Generic;

namespace My.Api.Tests.Trolleys
{
    [TestClass]
    public class TrolleyCalculatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Calculate_Trolley_With_Null_Trolley_Throws_Exception()
        {
            // act
            new TrolleyCalculator(null);
        }

        [TestMethod]
        public void Calculate_Trolley_Total_Price()
        {
            // arrange
            var fakeProducts = new List<Models.Product>() {
                FakeProduct("1", 2),
                FakeProduct("2", 5)
            };

            #region fakeQuantitiesForSpecials

            var fakeQuantitiesForSpecials1 = new List<Quantity>() {
                FakeQuantity("1", 3),
                FakeQuantity("2", 0),
            };

            var fakeQuantitiesForSpecials2 = new List<Quantity>() {
                FakeQuantity("1", 1),
                FakeQuantity("2", 2),
            };

            var bundleConfigurations = new List<Special>()
            {
                FakeSpecial(fakeQuantitiesForSpecials1, 5),
                FakeSpecial(fakeQuantitiesForSpecials2, 10),
            };

            #endregion fakeQuantitiesForSpecials

            var fakeQuantities = new List<Quantity>()
            {
                FakeQuantity("1", 3),
                FakeQuantity("2", 2)
            };

            var trolley = FakeTrolley(fakeProducts, bundleConfigurations, fakeQuantities);

            var trolleyCalculator = new TrolleyCalculator(trolley);

            // act
            var total = trolleyCalculator.CalculateTotal();

            // assert
            Assert.AreEqual(14M, total);
        }

        protected Product FakeProduct(string name, decimal price)
        {
            var product = new Product()
            {
                Name = name,
                Price = price,
            };

            return product;
        }

        protected Special FakeSpecial(List<Quantity> quantities, decimal total)
        {
            var special = new Special()
            {
                Quantities = quantities,
                Total = total
            };

            return special;
        }

        protected Quantity FakeQuantity(string name, decimal quantity)
        {
            var _quantity = new Quantity()
            {
                Name = name,
                _Quantity = quantity
            };

            return _quantity;
        }

        protected Trolley FakeTrolley(List<Product> products, List<Special> bundleConfigurations, List<Quantity> quantities)
        {
            var trolley = new Trolley()
            {
                Products = products,
                Specials = bundleConfigurations,
                Quantities = quantities
            };

            return trolley;
        }
    }
}