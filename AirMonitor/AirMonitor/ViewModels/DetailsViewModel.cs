using System.Linq;
using AirMonitor.Models;
using Android.Content.Res;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace AirMonitor.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        private string _caqiDescription =
            "Możesz bezpiecznie wyjść z domu bez swojej maski anty-smogowej i nie bać się o swoje zdrowie.";

        private string _caqiTitle = "Świetna jakość!";

        private int _caqiValue = 57;

        private Color _caqiColor = Color.FromHex("#6BC926");

        private double _humidityValue = 0.95;

        private double _pm10Percent = 135;

        private double _pm10Value = 67;

        private double _pm25Percent = 137;

        private double _pm25Value = 34;

        private double _pressureValue = 1027;


        public int CaqiValue
        {
            get => _caqiValue;
            set => SetProperty(ref _caqiValue, value);
        }

        public string CaqiTitle
        {
            get => _caqiTitle;
            set => SetProperty(ref _caqiTitle, value);
        }

        public string CaqiDescription
        {
            get => _caqiDescription;
            set => SetProperty(ref _caqiDescription, value);
        }

        public Color CaqiColor
        {
            get => _caqiColor;
            set => SetProperty(ref _caqiColor, value);
        }

        public double Pm25Value
        {
            get => _pm25Value;
            set => SetProperty(ref _pm25Value, value);
        }

        public double Pm25Percent
        {
            get => _pm25Percent;
            set => SetProperty(ref _pm25Percent, value);
        }

        public double Pm10Value
        {
            get => _pm10Value;
            set => SetProperty(ref _pm10Value, value);
        }

        public double Pm10Percent
        {
            get => _pm10Percent;
            set => SetProperty(ref _pm10Percent, value);
        }

        public double HumidityValue
        {
            get => _humidityValue;
            set => SetProperty(ref _humidityValue, value);
        }

        public double PressureValue
        {
            get => _pressureValue;
            set => SetProperty(ref _pressureValue, value);
        }

        private Installation _installation;

        public DetailsViewModel(Installation installation)
        {
            _installation = installation;
            UpdateValues();
        }

        private void UpdateValues()
        {
            var caqi = _installation.Measurements.Current.Indexes.FirstOrDefault(c => c.Name == "AIRLY_CAQI");
            if (caqi == null) return;
            var values = _installation.Measurements.Current.Values;
            var standards = _installation.Measurements.Current.Standards;
            CaqiValue = (int) Math.Floor(caqi.Value ?? 0.00d);
            CaqiTitle = caqi.Description;
            CaqiDescription = caqi.Advice;
            CaqiColor = Color.FromHex(caqi.Color);
            Pm25Value = values.FirstOrDefault(c => c.Name == "PM25")?.MeasurementValue ?? 0.0;
            Pm10Value = values.FirstOrDefault(c => c.Name == "PM10")?.MeasurementValue ?? 0.0;
            Pm25Percent = standards.FirstOrDefault(c => c.Pollutant == "PM25")?.Percent ?? 0.0;
            Pm10Percent = standards.FirstOrDefault(c => c.Pollutant == "PM10")?.Percent ?? 0.0;
            HumidityValue = Math.Floor(values.FirstOrDefault(c => c.Name == "HUMIDITY")?.MeasurementValue ?? 0.0);
            PressureValue = values.FirstOrDefault(c => c.Name == "PRESSURE")?.MeasurementValue ?? 0.0;
        }
    }
}