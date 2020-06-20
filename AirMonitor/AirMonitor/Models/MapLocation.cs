using Xamarin.Forms.Maps;

namespace AirMonitor.Models
{
    public class MapLocation
    {
        public int InstallationId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Position Position { get; set; }
    }
}