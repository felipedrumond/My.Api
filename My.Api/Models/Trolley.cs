using System.Collections.Generic;

namespace My.Api.Models
{
    public class Trolley
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<Quantity> Quantities { get; set; }
    }
}