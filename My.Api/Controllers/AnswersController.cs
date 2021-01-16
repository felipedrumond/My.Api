using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using My.Api.Models;
using My.Api.Models.Users;
using My.Api.Trolleys;
using System;
using System.Linq;
using System.Threading.Tasks;
using WXDevChallengeService.Services.OnlineStore;

namespace My.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IOnlineStoreService _onlineStoreService;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly string _userToken;
        private readonly string _candidateName;

        public AnswersController(IOnlineStoreService onlineStoreService, ILogger<AnswersController> logger, [FromServices] IConfiguration config)
        {
            _onlineStoreService = onlineStoreService ?? throw new ArgumentNullException(nameof(onlineStoreService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _candidateName = _config["CandidateName"];
            _userToken = _config["CandidateToken"];
        }

        [HttpGet]
        [Route("user")]
        public IActionResult GetUser()
        {
            var user = new User()
            {
                Name = _candidateName,
                Token = _userToken
            };

            return Ok(user);
        }

        [HttpGet]
        [Route("sort")]
        public async Task<IActionResult> Sort([FromQuery] string sortOption)
        {
            if (sortOption == "Recommended")
            {
                var recommendedProducts = await this._onlineStoreService.GetRecommendedProductsAsync(_userToken);
                return Ok(recommendedProducts);
            }

            var products = await this._onlineStoreService.GetProductsAsync(_userToken);

            switch (sortOption)
            {
                case "Low":
                    return Ok(products.OrderBy(p => p.Price));

                case "High":
                    return Ok(products.OrderByDescending(p => p.Price));

                case "Ascending":
                    return Ok(products.OrderBy(p => p.Name));

                case "Descending":
                    return Ok(products.OrderByDescending(p => p.Name));

                // Requirements does not mention what to do in case there is no sortOption.
                // Therefore, I am logging and return BadRequest so that I have more
                // test scenarios to cover.
                default:
                    {
                        var message = "Invalid sorting option.";
                        this._logger.LogError(message);

                        return BadRequest(message);
                    }
            }
        }

        [HttpPost]
        [Route("trolleyTotal")]
        public IActionResult GetTrolleyTotal(Trolley trolley)
        {
            var trolleyCalculator = new TrolleyCalculator(trolley);
            var total = trolleyCalculator.CalculateTotal();

            return Ok(total);
        }

        [HttpPost]
        [Route("trolleyTotal_From_OnlineStoreService")]
        public async Task<IActionResult> GetTrolleyTotal_From_OnlineStoreService(Trolley trolley)
        {
            var trolleyTotalResult = await this._onlineStoreService.GetTrolleyTotalAsync(trolley, _userToken);
            return Ok(trolleyTotalResult);
        }
    }
}