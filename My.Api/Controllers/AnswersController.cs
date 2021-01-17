using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using My.Api.Models;
using My.Api.Models.Users;
using My.Api.Trolleys;
using System;
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
            try
            {
                var sortedProducts = await this._onlineStoreService.GetSortedProductsAsync(_userToken, sortOption);
                return Ok(sortedProducts);
            }
            catch (OnlineStoreServiceException e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("trolleyTotal")]
        public IActionResult GetTrolleyTotal(Trolley trolley)
        {
            try
            {
                var trolleyCalculator = new TrolleyCalculator(trolley);
                var total = trolleyCalculator.CalculateTotal();
                return Ok(total);
            }
            catch (TrolleyCalculationException e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("trolleyTotal_From_OnlineStoreService")]
        public async Task<IActionResult> GetTrolleyTotal_From_OnlineStoreService(Trolley trolley)
        {
            try
            {
                var trolleyTotalResult = await this._onlineStoreService.GetTrolleyTotalAsync(trolley, _userToken);
                return Ok(trolleyTotalResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return StatusCode(500);
            }
        }
    }
}