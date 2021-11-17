using System;
using System.Collections.Generic;
using GPSTrackingXamarin.Abstractions;
using GPSTrackingXamarin.Abstractions.WifiAbstractions;
using GPSTrackingXamarin.VM;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace GPS_Tracking_Xamarin
{
    public partial class MainPage : ContentPage
    {
        MainViewModel _viewModel;
        ILocationUpdateService _locationService;
        IWifiTracker _wifiTracker;

        LineChart _chart;
        List<ChartEntry> entries;

        public MainPage()
        {
            InitializeComponent();
            _locationService = DependencyService.Resolve<ILocationUpdateService>();
            _wifiTracker = DependencyService.Resolve<IWifiTracker>();
            BindingContext = _viewModel = new MainViewModel(_locationService,_wifiTracker);
            _chart = new LineChart();
            _chart.Entries = _viewModel.chartEntries;
            this.chartView.Chart = _chart;
        }
        

        void mockData_Clicked(System.Object sender, System.EventArgs e)
        {
            var random = new Random();
            if (_chart == null)
            {
                _chart = new LineChart();
                entries = new List<ChartEntry>();

            }

            for (var i = 0; i<10; i++)
            {
                entries.Add(new Microcharts.ChartEntry(random.Next(0, 100))
                {
                    Color = SKColor.Parse("#4286f4"),
                    Label = "x",
                    ValueLabel = "100%"
                });
            }
           

         

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        async void StopServiceButton_Clicked(System.Object sender, System.EventArgs e)
        {
            _viewModel.StopGPS();            
            await DisplayAlert("GPS", "GPS Service Stoped", "Ok");
        }

        void startBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            _viewModel.GetPermissions();
        }

        void cleanBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            _viewModel.trackPoints.Clear();
            _viewModel.ChartVisibility = false;
        }

       
    }
}
