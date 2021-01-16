using System.Collections.Generic;

namespace WXDevChallengeService.Models
{
    public class CustomerHistory
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }
}