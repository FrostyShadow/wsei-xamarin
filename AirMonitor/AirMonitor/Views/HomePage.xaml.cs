using System;
using System.Collections.Generic;
using AirMonitor.Models;
using AirMonitor.ViewModels;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var context = (HomeViewModel) BindingContext;
            var parameter = (Installation) e.Item;
            ((ListView)sender).SelectedItem = null;
            if (parameter.Measurements == null)
            {
                DisplayAlert("No data", "No measurement data for selected installation.", "Cancel");
                return;
            }
            context.GoToDetailsCommand.Execute(parameter);
        }
    }
}
