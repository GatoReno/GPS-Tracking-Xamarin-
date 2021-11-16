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
        Chart BarChartSample;

        public MainPage()
        {
            InitializeComponent();
            _locationService = DependencyService.Resolve<ILocationUpdateService>();
            _wifiTracker = DependencyService.Resolve<IWifiTracker>();
            BindingContext = _viewModel = new MainViewModel(_locationService,_wifiTracker);

            //BarChartSample = new LineChart() { Entries = _viewModel.chartEntries };
            //this.chartView.Chart = BarChartSample;           

        }



        LineChart _chart;
        List<ChartEntry> entries = null;

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
            /*
            var entries = new[] {
                new Microcharts.ChartEntry(100){
                    Color = SKColor.Parse("#4286f4"),
                    Label = "x",
                    ValueLabel = "100%"
                },
                new Microcharts.ChartEntry(90){
                    Color = SKColor.Parse("#ba1079"),
                    Label = "x",
                    ValueLabel = "90%"
                },
                new Microcharts.ChartEntry(70){
                    Color = SKColor.Parse("#5ae273"),
                    Label = "x",
                    ValueLabel = "70%"
                },
            };
            */



            _chart.Entries = entries;
            this.chartView.Chart = _chart;

          //  chartView.

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
