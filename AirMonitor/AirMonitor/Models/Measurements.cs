using System.Collections.Generic;

namespace AirMonitor.Models
{
    public class Measurements
    {
        public AveragedValues Current { get; set; }
        public IList<AveragedValues> History { get; set; }
        public IList<AveragedValues> Forecast { get; set; }
        public Installation Installation { get; set; }
    }
}