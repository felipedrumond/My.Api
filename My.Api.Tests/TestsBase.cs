using My.Api.Models;
using System.Collections.Generic;
using WXDevChallengeService.Models;

namespace My.Api.Tests
{
    /// <summary>
    /// This class is intended to ease the creation of fake objects for testing purposes.
    /// As the solution grows and therefore unit tests number also grows, these methods becomes very handy
    /// as they help developers to write better unit tests on top of them
    /// </summary>
    public class TestsBase
    {
        protected CustomerHistory FakeCustomerHistory(int customerId, List<WXDevChallengeService.Models.Product> products)
        {
            var customerHistory = new CustomerHistory()
            {
                CustomerId = customerId,
                Products = products,
            };

            return customerHistory;
        }

        protected WXDevChallengeService.Models.Product FakeProduct(string name, decimal price, decimal quantity)
        {
            var product = new WXDevChallengeService.Models.Product()
            {
                Name = name,
                Price = price,
                Quantity = quantity
            };

            return product;
        }

        protected Models.Product FakeProduct(string name, decimal price)
        {
            var product = new Models.Product()
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


        protected Trolley FakeTrolley(List<Models.Product> products, List<Special> bundleConfigurations, List<Quantity> quantities)
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