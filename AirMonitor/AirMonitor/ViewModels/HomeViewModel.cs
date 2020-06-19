﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using AirMonitor.Models;
using AirMonitor.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;


        private ICommand _goToDetailsCommand;


        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;

            _ = Initialize();
        }

        public ICommand GoToDetailsCommand => _goToDetailsCommand ??= new Command(OnGoToDetails);

        private IList<Installation> _installations;

        public IList<Installation> Installations
        {
            get => _installations;
            set => SetProperty(ref _installations, value);
        }

        private bool _isDownloading;

        public bool IsDownloading
        {
            get => _isDownloading;
            set => SetProperty(ref _isDownloading, value);
        }

        private Location _location;

        private async Task Initialize()
        {
            _location = await GetLocation();
            IsDownloading = true;
            await GetData();
        }

        private async Task GetData()
        {
            var current = (await App.DatabaseHelper.GetCurrentMeasurementsAsync()).FirstOrDefault()?.TillDateTime;
            List<Installation> installationList;
            bool isUpdateRequired;
            if (current == null)
            {
                installationList = (await GetInstallations(_location, maxResults: 7)).ToList();
                await App.DatabaseHelper.SaveAsync(installationList);
                isUpdateRequired = true;
            }
            else if (((TimeSpan) (DateTime.UtcNow - current)).TotalHours >= 1)
            {
                installationList = (await GetInstallations(_location, maxResults: 7)).ToList();
                await App.DatabaseHelper.SaveAsync(installationList);
                isUpdateRequired = true;
            }
            else
            {
                installationList = (await App.DatabaseHelper.GetInstallationsAsync()).ToList();
                isUpdateRequired = false;
            }
            
            await GetMeasurements(installationList, isUpdateRequired);
            IsDownloading = false;
            Installations = new List<Installation>(installationList);
        }

        private async Task GetMeasurements(IEnumerable<Installation> installations, bool isUpdateRequired = true)
        {
            if (installations == null)
            {
                Debug.WriteLine("No installation data.");
                return;
            }

            var measurements = new List<Measurements>();

            foreach (var installation in installations)
            {
                if (isUpdateRequired)
                {
                    var query = GetQuery(new Dictionary<string, object>
                    {
                        {"includeWind", false},
                        {"indexType", "AIRLY_CAQI"},
                        {"installationId", installation.Id}
                    });

                    var url = GetAirlyApiUrl(App.AirlyApiMeasurementUrl, query);

                    var response = await GetHttpResponseAsync<Measurements>(url);
                    response.Installation = installation;
                    measurements.Add(response);
                    installation.Measurements = response;
                }
                else
                {
                    var measurement = await App.DatabaseHelper.GetMeasurementByInstallationAsync(installation.Id);
                    measurement.Installation = installation;
                    measurement.Current =
                        await App.DatabaseHelper.GetCurrentMeasurementByInstallationAsync(installation.Id);
                    installation.Measurements = measurement;
                }
            }
            if(isUpdateRequired)
                await App.DatabaseHelper.SaveAsync(measurements);
        }

        private async Task<IEnumerable<Installation>> GetInstallations(Location location, double maxDistanceInKm = 3,
            int maxResults = -1)
        {
            if (location == null)
            {
                Debug.WriteLine("No location data.");
                return null;
            }

            _isDownloading = true;

            var query = GetQuery(new Dictionary<string, object>
            {
                {"lat", location.Latitude},
                {"lng", location.Longitude},
                {"maxDistanceKM", maxDistanceInKm},
                {"maxResults", maxResults}
            });
            var url = GetAirlyApiUrl(App.AirlyApiInstallationUrl, query);


            var response = await GetHttpResponseAsync<IEnumerable<Installation>>(url);
            return response;
        }


        private string GetAirlyApiUrl(string path, string query)
        {
            var builder = new UriBuilder(App.AirlyApiUrl) {Port = -1};
            builder.Path += path;
            builder.Query = query;
            var url = builder.ToString();


            return url;
        }


        private string GetQuery(IDictionary<string, object> args)
        {
            if (args == null) return null;


            var query = HttpUtility.ParseQueryString(string.Empty);


            foreach (var arg in args)
                if (arg.Value is double number)
                    query[arg.Key] = number.ToString(CultureInfo.InvariantCulture);
                else
                    query[arg.Key] = arg.Value?.ToString();


            return query.ToString();
        }


        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient {BaseAddress = new Uri(App.AirlyApiUrl)};


            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            client.DefaultRequestHeaders.Add("apikey", App.AirlyApiKey);
            return client;
        }


        private async Task<T> GetHttpResponseAsync<T>(string url)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync(url);


                if (response.Headers.TryGetValues("X-RateLimit-Limit-day", out var dayLimit) &&
                    response.Headers.TryGetValues("X-RateLimit-Remaining-day", out var dayLimitRemaining))
                    Debug.WriteLine(
                        $"Day limit: {dayLimit?.FirstOrDefault()}, remaining: {dayLimitRemaining?.FirstOrDefault()}");


                switch ((int) response.StatusCode)
                {
                    case 200:
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(content);
                        return result;
                    case 429: // too many requests
                        Debug.WriteLine("Too many requests");
                        break;
                    default:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Response error: {errorContent}");
                        return default;
                }
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }


            return default;
        }


        private async Task<Location> GetLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            return location;
        }

        private void OnGoToDetails(object parameters)
        {
            _navigation.PushAsync(new DetailsPage((Installation) parameters));
        }
    }
}