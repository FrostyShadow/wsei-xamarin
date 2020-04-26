using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AirMonitor.Models
{
    public class Value
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public double MeasurementValue { get; set; }
    }
}