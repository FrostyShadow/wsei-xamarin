using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SQLite;

namespace AirMonitor.Models
{
    public class Value
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public double? MeasurementValue { get; set; }

        public Value()
        {
            
        }
    }
}