using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirMonitor.Models
{
    public class Measurements
    {
        public AveragedValues Current { get; set; }
        public IList<AveragedValues> History { get; set; }
        public IList<AveragedValues> Forecast { get; set; }
        public Installation Installation { get; set; }

        public Measurements()
        {
            
        }

        public Measurements(MeasurementEntity measurementEntity)
        {
            History = JsonConvert.DeserializeObject<IList<AveragedValues>>(measurementEntity.History);
            Forecast = JsonConvert.DeserializeObject<IList<AveragedValues>>(measurementEntity.Forecast);
        }
    }
}