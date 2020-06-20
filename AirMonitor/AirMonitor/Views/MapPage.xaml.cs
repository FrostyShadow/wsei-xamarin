using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirMonitor.Models;
using AirMonitor.ViewModels;
using Java.Time.Temporal;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }

        private void Pin_OnInfoWindowClicked(object sender, PinClickedEventArgs e)
        {
            var context = (HomeViewModel)BindingContext;
            var parameter = (Pin) sender;
            var mapLocation = (MapLocation) parameter.BindingContext;
            context.GoToDetailsFromMapCommand.Execute(mapLocation.InstallationId);
        }
    }
}