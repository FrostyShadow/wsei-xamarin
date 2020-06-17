using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace AirMonitor.Models
{
    public class Installation
    {
        public int Id { get; set; }
        public Coordinates Location { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        [JsonProperty(PropertyName = "airly")]
        public bool IsAirlyInstallation { get; set; }
        public Sponsor Sponsor { get; set; }
        public Measurements Measurements { get; set; }

        public Installation()
        {
            
        }

        public Installation(InstallationEntity installationEntity)
        {
            Id = installationEntity.Id;
            Location = JsonConvert.DeserializeObject<Coordinates>(installationEntity.Location);
            Address = JsonConvert.DeserializeObject<Address>(installationEntity.Address);
            Elevation = installationEntity.Elevation;
            IsAirlyInstallation = installationEntity.IsAirlyInstallation;
            Measurements = App.DatabaseHelper.GetMeasurementAsync(installationEntity.MeasurementsId).Result;
        }
    }
}