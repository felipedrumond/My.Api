using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace My.Api.Models
{
    [DataContract]
    public class Quantity
    {
        public string Name { get; set; }

        [JsonPropertyName("quantity")]
        public decimal _Quantity { get; set; }
    }
}