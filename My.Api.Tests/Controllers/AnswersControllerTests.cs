using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using My.Api.Controllers;
using My.Api.Models;
using My.Api.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WXDevChallengeService.Models;
using WXDevChallengeService.Services.OnlineStore;

namespace My.Api.Tests.Controllers
{
    [TestClass]
    public class AnswersControllerTests : TestsBase
    {
        private Mock<IOnlineStoreService> onlineStoreServiceMock;
        private AnswersController controller;

        [TestInitialize()]
        public void Startup()
        {
            // mock the IConfiguration for the controller
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c[It.Is<string>(s => s == "CandidateToken")]).Returns("token");
            configuration.SetupGet(c => c[It.Is<string>(s => s == "CandidateName")]).Returns("candidateName");

            onlineStoreServiceMock = new Mock<IOnlineStoreService>(MockBehavior.Strict);
            controller = new AnswersController(onlineStoreServiceMock.Object, configuration.Object);
        }

        [TestMethod]
        public void GetUser_Returns_User()
        {
            // arrange

            // act
            var result = controller.GetUser();
            var okResult = result as OkObjectResult;
            var user = okResult.Value as User;

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(User));
            Assert.AreEqual("candidateName", user.Name);
            Assert.AreEqual("token", user.Token);
        }

        [TestMethod]
        public void GetTrolleyTotal_Returns_TrolleyTotal()
        {
            // arrange
            var trolley = new Trolley()
            {
                Products = new List<My.Api.Models.Product>() { new My.Api.Models.Product() { Name = "1", Price = 20 } },
                Quantities = new List<Quantity>() { new Quantity() { Name = "1", _Quantity = 1 } },
                Specials = new List<Special>() { }
            };

            // act
            var result = controller.GetTrolleyTotal(trolley);
            var okResult = result as OkObjectResult;
            var resultValue = (decimal)okResult.Value;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(decimal));
            Assert.AreEqual(20M, resultValue);
        }

        [TestMethod]
        public void GetTrolleyTotal_Returns_TrolleyTotal_From_OnlineStoreService()
        {
            // arrange
            onlineStoreServiceMock.Setup(r => r.GetTrolleyTotalAsync(It.IsAny<object>(), It.IsAny<string>())).Returns(Task.FromResult("14"));

            var trolley = new Trolley() { };

            // act
            var result = controller.GetTrolleyTotal_From_OnlineStoreService(trolley).Result;
            var okResult = result as OkObjectResult;
            var resultValue = okResult.Value as string;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(string));
            Assert.AreEqual("14", resultValue);
        }

        [TestMethod]
        public void Sort_Products_With_No_Order_Returns_Products_As_They_Are()
        {
            // arrange
            var fakeProductA = FakeProduct("A", 10, 1);
            var fakeProductB = FakeProduct("B", 20, 1);
            var fakeProductC = FakeProduct("C", 30, 1);
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC };

            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort(null).Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Name, "A");
            Assert.AreEqual(resultValue[1].Name, "B");
            Assert.AreEqual(resultValue[2].Name, "C");

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Sort_Products_By_Low_Returns_Products_Orded_By_Price_Asc()
        {
            // arrange
            var fakeProductA = FakeProduct("A", 10, 1);
            var fakeProductB = FakeProduct("B", 20, 1);
            var fakeProductC = FakeProduct("C", 30, 1);
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC };

            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("Low").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Price, 10);
            Assert.AreEqual(resultValue[1].Price, 20);
            Assert.AreEqual(resultValue[2].Price, 30);

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Sort_Products_By_High_Returns_Products_Orded_By_Price_Desc()
        {
            // arrange
            var fakeProductA = FakeProduct("A", 10, 1);
            var fakeProductB = FakeProduct("B", 20, 1);
            var fakeProductC = FakeProduct("C", 30, 1);
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC };

            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("High").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Price, 30);
            Assert.AreEqual(resultValue[1].Price, 20);
            Assert.AreEqual(resultValue[2].Price, 10);

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Sort_Products_By_Name_Ascending_Returns_Products_Orded_By_Name_Ascending()
        {
            // arrange
            var fakeProductA = FakeProduct("B", 10, 1);
            var fakeProductB = FakeProduct("C", 20, 1);
            var fakeProductC = FakeProduct("A", 30, 1);
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC };

            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("Ascending").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Name, "A");
            Assert.AreEqual(resultValue[1].Name, "B");
            Assert.AreEqual(resultValue[2].Name, "C");

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Sort_Products_By_Name_Descending_Returns_Products_Orded_By_Name_Descending()
        {
            // arrange
            var fakeProductA = FakeProduct("B", 10, 1);
            var fakeProductB = FakeProduct("C", 20, 1);
            var fakeProductC = FakeProduct("A", 30, 1);
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC };

            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("Descending").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Name, "C");
            Assert.AreEqual(resultValue[1].Name, "B");
            Assert.AreEqual(resultValue[2].Name, "A");

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Sort_Products_Orded_By_Recommended_Returns_Products_Orded_By_Most_Sold()
        {
            // arrange
            var fakeProductA = FakeProduct("A - Least Sold Product", 10, 1);
            var fakeProductB = FakeProduct("B - Second Most Sold Product", 20, 500);
            var fakeProductC = FakeProduct("C - Third Most Sold Product", 30, 10);
            var fakeProductD = FakeProduct("D - Most Sold Product", 20, 1000);

            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB, fakeProductC, fakeProductD };

            var fakeCustomerHistory1 = FakeCustomerHistory(1, new List<WXDevChallengeService.Models.Product>() { fakeProductA, fakeProductB });
            var fakeCustomerHistory2 = FakeCustomerHistory(2, new List<WXDevChallengeService.Models.Product>() { fakeProductC });
            var fakeCustomerHistory3 = FakeCustomerHistory(3, new List<WXDevChallengeService.Models.Product>() { fakeProductD });
            var fakeCustomerHistories = new List<CustomerHistory>() { fakeCustomerHistory1, fakeCustomerHistory2, fakeCustomerHistory3 };

            onlineStoreServiceMock.Setup(r => r.GetCustomersHistoryAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeCustomerHistories));
            onlineStoreServiceMock.Setup(r => r.GetProductsAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("Recommended").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));
            Assert.AreEqual(resultValue[0].Name, "D - Most Sold Product");
            Assert.AreEqual(resultValue[1].Name, "B - Second Most Sold Product");
            Assert.AreEqual(resultValue[2].Name, "C - Third Most Sold Product");
            Assert.AreEqual(resultValue[3].Name, "A - Least Sold Product");

            onlineStoreServiceMock.Verify(r => r.GetProductsAsync(It.IsAny<string>()), Times.Once);
            onlineStoreServiceMock.Verify(r => r.GetCustomersHistoryAsync(It.IsAny<string>()), Times.Once);
        }
    }
}