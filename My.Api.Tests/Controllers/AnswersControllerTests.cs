using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using My.Api.Controllers;
using My.Api.Models;
using My.Api.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WXDevChallengeService.Services.OnlineStore;

namespace My.Api.Tests.Controllers
{
    [TestClass]
    public class AnswersControllerTests : TestsBase
    {
        private Mock<IOnlineStoreService> onlineStoreServiceMock;
        private Mock<ILogger<AnswersController>> loggerMock;
        private Mock<IConfiguration> configuration;
        private AnswersController controller;

        [TestInitialize()]
        public void Startup()
        {
            // mock the IConfiguration for the controller
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c[It.Is<string>(s => s == "CandidateToken")]).Returns("token");
            configuration.SetupGet(c => c[It.Is<string>(s => s == "CandidateName")]).Returns("candidateName");

            onlineStoreServiceMock = new Mock<IOnlineStoreService>(MockBehavior.Strict);
            loggerMock = new Mock<ILogger<AnswersController>>(MockBehavior.Default);

            controller = new AnswersController(onlineStoreServiceMock.Object, loggerMock.Object, configuration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Unresolved_OnlineStoreService_Throws_Exception()
        {
            // act
            new AnswersController(null, loggerMock.Object, configuration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Unresolved_Logger_Throws_Exception()
        {
            // act
            new AnswersController(onlineStoreServiceMock.Object, null, configuration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Unresolved_Config_Throws_Exception()
        {
            // act
            new AnswersController(onlineStoreServiceMock.Object, loggerMock.Object, null);
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
        public void GetTrolleyTotal_With_Invalid_Data_Returns_BadRequest()
        {
            // arrange
            var trolley = new Trolley()
            {
                Products = new List<My.Api.Models.Product>() { new Product() { Name = "avocado", Price = 20 } },
                Quantities = new List<Quantity>() { new Quantity() { Name = "berries", _Quantity = 1 } },
                Specials = new List<Special>() { }
            };

            // act
            var result = controller.GetTrolleyTotal(trolley);
            var badRequest = result as BadRequestObjectResult;

            // assert
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("Trolley failed to calculate its total.", badRequest.Value);
        }

        [TestMethod]
        public void GetTrolleyTotal_With_Invalid_Data_Logs_Error()
        {
            // arrange
            var trolley = new Trolley()
            {
                Products = new List<My.Api.Models.Product>() { new Product() { Name = "avocado", Price = 20 } },
                Quantities = new List<Quantity>() { new Quantity() { Name = "berries", _Quantity = 1 } },
                Specials = new List<Special>() { }
            };

            // act
            var result = controller.GetTrolleyTotal(trolley);

            // assert
            loggerMock.VerifyLogging("Trolley failed to calculate its total.", LogLevel.Error);
        }

        [TestMethod]
        public void Sort_Products_Returns_Sorted_Products()
        {
            // arrange
            var fakeProductsList = new List<WXDevChallengeService.Models.Product>() { };
            onlineStoreServiceMock.Setup(r => r.GetSortedProductsAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(fakeProductsList));

            // act
            var result = controller.Sort("Low").Result;
            var okResult = result as OkObjectResult;
            var resultValue = (okResult.Value as IEnumerable<WXDevChallengeService.Models.Product>).ToList();

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<WXDevChallengeService.Models.Product>));

            onlineStoreServiceMock.Verify(r => r.GetSortedProductsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Invalid_Sort_Option_Returns_BadRequest()
        {
            // arrange
            onlineStoreServiceMock.Setup(r => r.GetSortedProductsAsync(It.IsAny<string>(), "invalid_sort_option"))
                .Throws(new OnlineStoreServiceException());

            // act
            var result = controller.Sort("invalid_sort_option").Result;
            var badRequest = result as BadRequestObjectResult;

            // assert
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("Invalid sorting option.", badRequest.Value);
        }

        [TestMethod]
        public void Invalid_Sort_Option_Logs_Error()
        {
            // arrange
            onlineStoreServiceMock.Setup(r => r.GetSortedProductsAsync(It.IsAny<string>(), "invalid_sort_option"))
                .Throws(new OnlineStoreServiceException());

            // act
            var result = controller.Sort("invalid_sort_option").Result;

            // assert
            loggerMock.VerifyLogging("Invalid sorting option.", LogLevel.Error);
        }
    }
}