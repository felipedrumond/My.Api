using Microsoft.VisualStudio.TestTools.UnitTesting;
using My.Api.Models;
using My.Api.Trolleys;
using System;
using System.Collections.Generic;

namespace My.Api.Tests.Trolleys
{
    [TestClass]
    public class TrolleyCalculatorTests : TestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Calculate_Trolley_With_Null_Trolley_Throws_Exception()
        {
            // act
            new TrolleyCalculator(null);
        }

        [TestMethod]
        [ExpectedException(typeof(TrolleyCalculationException), "Trolley failed to calculate its total.")]
        public void Calculate_Trolley_Total_Price_With_Duplicated_Products_In_Catalog_Throws_Exception()
        {
            // arrange
            var fakeProducts = new List<Models.Product>() {
                FakeProduct("berries", 2),
                FakeProduct("berries", 5),
            };

            #region fakeQuantitiesForSpecials

            var fakeQuantitiesForSpecials1 = new List<Quantity>() {
                FakeQuantity("berries", 3),
            };

            var fakeQuantitiesForSpecials2 = new List<Quantity>() {
                FakeQuantity("berries", 1),
            };

            var bundleConfigurations = new List<Special>()
            {
                FakeSpecial(fakeQuantitiesForSpecials1, 5),
                FakeSpecial(fakeQuantitiesForSpecials2, 10),
            };

            #endregion fakeQuantitiesForSpecials

            var fakeQuantities = new List<Quantity>()
            {
                FakeQuantity("berries", 3),
            };

            var trolley = FakeTrolley(fakeProducts, bundleConfigurations, fakeQuantities);

            var trolleyCalculator = new TrolleyCalculator(trolley);

            // act
            var total = trolleyCalculator.CalculateTotal();

            // assert
            Assert.AreEqual(14M, total);
        }

        [TestMethod]
        [ExpectedException(typeof(TrolleyCalculationException), "Trolley failed to calculate its total.")]
        public void Calculate_Trolley_Total_Price_With_Product_Not_Found_In_Catalog_Throws_Exception()
        {
            // arrange
            var fakeProducts = new List<Models.Product>() {
                FakeProduct("toilet paper", 2),
            };

            var bundleConfigurations = new List<Special>() { };

            var fakeQuantities = new List<Quantity>()
            {
                FakeQuantity("berries", 3),
            };

            var trolley = FakeTrolley(fakeProducts, bundleConfigurations, fakeQuantities);

            var trolleyCalculator = new TrolleyCalculator(trolley);

            // act
            var total = trolleyCalculator.CalculateTotal();

            // assert
            Assert.AreEqual(14M, total);
        }

        [TestMethod]
        public void Calculate_Trolley_Total_Price()
        {
            // arrange
            var fakeProducts = new List<Models.Product>() {
                FakeProduct("berries", 2),
                FakeProduct("avocado", 5)
            };

            #region fakeQuantitiesForSpecials

            var fakeQuantitiesForSpecials1 = new List<Quantity>() {
                FakeQuantity("berries", 3),
                FakeQuantity("avocado", 0),
            };

            var fakeQuantitiesForSpecials2 = new List<Quantity>() {
                FakeQuantity("berries", 1),
                FakeQuantity("avocado", 2),
            };

            var bundleConfigurations = new List<Special>()
            {
                FakeSpecial(fakeQuantitiesForSpecials1, 5),
                FakeSpecial(fakeQuantitiesForSpecials2, 10),
            };

            #endregion fakeQuantitiesForSpecials

            var fakeQuantities = new List<Quantity>()
            {
                FakeQuantity("berries", 3),
                FakeQuantity("avocado", 2)
            };

            var trolley = FakeTrolley(fakeProducts, bundleConfigurations, fakeQuantities);

            var trolleyCalculator = new TrolleyCalculator(trolley);

            // act
            var total = trolleyCalculator.CalculateTotal();

            // assert
            Assert.AreEqual(14M, total);
        }

        [TestMethod]
        public void Calculate_Trolley_Total_Price_With_Reminiscent_Items()
        {
            // arrange
            var fakeProducts = new List<Models.Product>() {
                FakeProduct("berries", 2),
                FakeProduct("avocado", 5),
            };

            #region fakeQuantitiesForSpecials

            var fakeQuantitiesForBundle1 = new List<Quantity>() {
                FakeQuantity("berries", 3),
                FakeQuantity("avocado", 0),
            };

            var fakeQuantitiesForBundle2 = new List<Quantity>() {
                FakeQuantity("berries", 1),
                FakeQuantity("avocado", 2),
            };

            var bundleConfigurations = new List<Special>()
            {
                FakeSpecial(fakeQuantitiesForBundle1, 5),
                FakeSpecial(fakeQuantitiesForBundle2, 10),
            };

            #endregion fakeQuantitiesForSpecials

            var fakeQuantities = new List<Quantity>()
            {
                FakeQuantity("berries", 3),
                FakeQuantity("avocado", 3)
            };

            var trolley = FakeTrolley(fakeProducts, bundleConfigurations, fakeQuantities);

            var trolleyCalculator = new TrolleyCalculator(trolley);

            // act
            var total = trolleyCalculator.CalculateTotal();

            // assert
            Assert.AreEqual(19M, total);
        }
    }
}