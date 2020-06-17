using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace AirMonitor.Models
{
    public class MeasurementEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CurrentId { get; set; }
        public string History { get; set; }
        public string Forecast { get; set; }
        public int InstallationId { get; set; }

        public MeasurementEntity()
        {
            
        }

        public MeasurementEntity(Measurements measurements)
        {
            History = JsonConvert.SerializeObject(measurements.History);
            Forecast = JsonConvert.SerializeObject(measurements.Forecast);
            InstallationId = measurements.Installation.Id;
        }
    }
}