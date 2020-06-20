using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirMonitor.Models;
using AirMonitor.ViewModels;
using Android.Content;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Installation installation)
        {
            BindingContext = new DetailsViewModel(installation);
            InitializeComponent();
        }

        public DetailsPage(Measurements measurements)
        {
            BindingContext = new DetailsViewModel(measurements);
            InitializeComponent();
        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("What is CAQI?",
                "The CAQI (Common Air Quality Index) is a number on a scale from 1 to 100, where a low value means good air quality and a high value means bad air quality." +
                "\nThe index is defined in both hourly and daily versions, and separately near roads (a \"roadside\" or \"traffic\" index) or away from roads (a \"background\" index).", "Close");
        }
    }
}
