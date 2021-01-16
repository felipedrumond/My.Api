using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WXDevChallengeService.Models;

namespace WXDevChallengeService.Services.OnlineStore
{
    public class OnlineStoreService : IOnlineStoreService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public OnlineStoreService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<List<CustomerHistory>> GetCustomersHistoryAsync(string userToken)
        {
            var url = $"shopperHistory?token={userToken}";

            var streamTask = _httpClient.GetStreamAsync(url);

            var customersHistory = await JsonSerializer.DeserializeAsync<List<CustomerHistory>>(await streamTask, options);

            return customersHistory;
        }

        public async Task<List<Product>> GetProductsAsync(string userToken)
        {
            var url = $"products?token={userToken}";

            var streamTask = _httpClient.GetStreamAsync(url);

            var products = await JsonSerializer.DeserializeAsync<List<Product>>(await streamTask, options);

            return products;
        }

        public async Task<string> GetTrolleyTotalAsync(object trolley, string userToken)
        {
            var url = $"trolleyCalculator?token={userToken}";

            var json = JsonSerializer.Serialize(trolley, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var streamTask = await _httpClient.PostAsync(url, content);
            var result = await streamTask.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<List<Product>> GetRecommendedProductsAsync(string userToken)
        {
            var products = await this.GetProductsAsync(userToken);
            var customersHistory = await this.GetCustomersHistoryAsync(userToken);

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