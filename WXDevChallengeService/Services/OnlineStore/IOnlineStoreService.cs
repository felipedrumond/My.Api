using System.Collections.Generic;
using System.Threading.Tasks;
using WXDevChallengeService.Models;

namespace WXDevChallengeService.Services.OnlineStore
{
    public interface IOnlineStoreService
    {
        Task<List<CustomerHistory>> GetCustomersHistoryAsync(string userToken);

        Task<List<Product>> GetProductsAsync(string userToken);

        Task<string> GetTrolleyTotalAsync(object trolley, string userToken);

        Task<List<Product>> GetSortedProductsAsync(string userToken, string sortOption);
    }
}