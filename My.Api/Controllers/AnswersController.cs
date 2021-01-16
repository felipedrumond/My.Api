using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using My.Api.Models;
using My.Api.Models.Users;
using My.Api.Trolleys;
using System.Collections.Generic;
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
        private readonly string _userToken;
        private readonly string _candidateName;

        public AnswersController(IOnlineStoreService onlineStoreService, [FromServices] IConfiguration config)
        {
            _onlineStoreService = onlineStoreService;
            _candidateName = config["CandidateName"];
            _userToken = config["CandidateToken"];
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

                case "Recommended":
                    {
                        var recommendedProducts = await GetRecommendedProducts(products);
                        return Ok(recommendedProducts);
                    }

                // test does not mention what to do in case there is no sortOption: either return products with no sorting or BadRequest
                default: return Ok(products);
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

        private async Task<List<WXDevChallengeService.Models.Product>> GetRecommendedProducts(List<WXDevChallengeService.Models.Product> products)
        {
            var customersHistory = await this._onlineStoreService.GetCustomersHistoryAsync(_userToken);

            // the resource api doesn't provide a product.id, therefore I'm sorting by name
            var productsPopularity = customersHistory
                .SelectMany(h => h.Products)
                .GroupBy(p => p.Name)
                .Select(g => new ProductPopularity
                {
                    Name = g.Key,
                    AmountSold = g.Sum(a => a.Quantity)
                })
                .OrderBy(pp => pp.AmountSold)
                .Select(pp => pp.Name)
                .ToList();

            var productsOrderedByPopularity = products
                .OrderByDescending(p => productsPopularity.IndexOf(p.Name))
                .ToList();

            return productsOrderedByPopularity;
        }
    }
}