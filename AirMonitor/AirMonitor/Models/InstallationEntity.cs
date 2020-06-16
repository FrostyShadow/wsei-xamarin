using Newtonsoft.Json;

namespace AirMonitor.Models
{
    public class InstallationEntity
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public double Elevation { get; set; }
        public bool IsAirlyInstallation { get; set; }
        public int SponsorId { get; set; }
        public int MeasurementsId { get; set; }

        public InstallationEntity()
        {
            
        }

        public InstallationEntity(Installation installation)
        {
            Location = JsonConvert.SerializeObject(installation.Location);
            Address = JsonConvert.SerializeObject(installation.Address);
            Elevation = installation.Elevation;
            IsAirlyInstallation = installation.IsAirlyInstallation;
            //SponsorId = installation.Sponsor.Id;
            //MeasurementsId = new MeasurementEntity(installation.Measurements).Id;
        }
    }
}