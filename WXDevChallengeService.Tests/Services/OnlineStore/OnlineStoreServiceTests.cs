using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using WXDevChallengeService.Models;
using WXDevChallengeService.Services.OnlineStore;

namespace WXDevChallengeService.Tests.Services.OnlineStore
{
    [TestClass]
    public class OnlineStoreServiceTests : TestsBase
    {
        [TestMethod]
        public void GetCustomersHistoryAsync_Returns_CustomerHistory()
        {
            // arrange
            var fakeProductA = FakeProduct("A", 1, 1);
            var fakeProductB = FakeProduct("B", 2, 2);
            var fakeProductC = FakeProduct("C", 3, 3);

            var fakeHistory1 = FakeCustomerHistory(1, new List<Product> { fakeProductA, fakeProductB });
            var fakeHistory2 = FakeCustomerHistory(2, new List<Product> { fakeProductC });
            var fakeCustomerHistories = new List<CustomerHistory>() { fakeHistory1, fakeHistory2 };

            var fakeResponseContent = JsonSerializer.Serialize(fakeCustomerHistories);
            var fakeHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeResponseContent)
            };

            var handlerMock = FakeMessageHandler(fakeHttpResponse);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://www.test.com/")
            };

            var subjectUnderTest = new OnlineStoreService(httpClient);

            // act
            var result = subjectUnderTest.GetCustomersHistoryAsync(string.Empty).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(fakeCustomerHistories.First().CustomerId, result.First().CustomerId);
            Assert.AreEqual(fakeCustomerHistories.First().Products.Count, result.First().Products.Count);
            Assert.AreEqual(fakeCustomerHistories.Last().CustomerId, result.Last().CustomerId);
            Assert.AreEqual(fakeCustomerHistories.Last().Products.Count, result.Last().Products.Count);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        public void GetProductsAsync_Returns_Products()
        {
            // arrange
            var fakeProducts = new List<Product>() {
                FakeProduct("A", 1, 1),
                FakeProduct("B", 2, 2)
            };

            var fakeResponseContent = JsonSerializer.Serialize(fakeProducts);
            var fakeHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeResponseContent)
            };

            var handlerMock = FakeMessageHandler(fakeHttpResponse);
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://www.test.com/")
            };

            var subjectUnderTest = new OnlineStoreService(httpClient);

            // act
            var result = subjectUnderTest.GetProductsAsync(string.Empty).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(fakeProducts.First().Name, result.First().Name);
            Assert.AreEqual(fakeProducts.First().Price, result.First().Price);
            Assert.AreEqual(fakeProducts.First().Quantity, result.First().Quantity);
            Assert.AreEqual(fakeProducts.Last().Name, result.Last().Name);
            Assert.AreEqual(fakeProducts.Last().Price, result.Last().Price);
            Assert.AreEqual(fakeProducts.Last().Quantity, result.Last().Quantity);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        public void GetTrolleyTotalAsync_Returns_String()
        {
            // arrange
            var fakeResponseContent = "14";
            var fakeHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeResponseContent)
            };

            var handlerMock = FakeMessageHandler(fakeHttpResponse);
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://www.test.com/")
            };

            var subjectUnderTest = new OnlineStoreService(httpClient);

            // act
            var result = subjectUnderTest.GetTrolleyTotalAsync(new { }, string.Empty).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("14", result);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}