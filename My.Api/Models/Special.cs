using System.Collections.Generic;

namespace My.Api.Models
{
    public class Special
    {
        public List<Quantity> Quantities { get; set; }
        public decimal Total { get; set; }
    }
}
