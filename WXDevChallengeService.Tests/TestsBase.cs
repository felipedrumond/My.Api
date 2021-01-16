using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WXDevChallengeService.Models;

namespace WXDevChallengeService.Tests
{
    /// <summary>
    /// This class is intended to ease the creation of fake objects for testing purposes.
    /// As the solution grows and therefore unit tests number also grows, these methods becomes very handy
    /// as they help developers to write better unit tests on top of them
    /// </summary>
    public class TestsBase
    {
        protected Product FakeProduct(string name, decimal price, decimal quantity)
        {
            var product = new Product()
            {
                Name = name,
                Price = price,
                Quantity = quantity
            };

            return product;
        }

        protected CustomerHistory FakeCustomerHistory(int customerId, List<Product> products)
        {
            var customerHistory = new CustomerHistory()
            {
                CustomerId = customerId,
                Products = products
            };

            return customerHistory;
        }

        protected Mock<HttpMessageHandler> FakeMessageHandler(HttpResponseMessage httpResponseMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(httpResponseMessage)
               .Verifiable();

            return handlerMock;
        }

    }
}