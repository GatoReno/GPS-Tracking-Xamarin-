using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GPSTrackingXamarin.Abstractions;
using GPSTrackingXamarin.Abstractions.WifiAbstractions;
using GPSTrackingXamarin.Models;
using Microcharts;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GPSTrackingXamarin.VM
{
    public class MainViewModel : BaseViewModel
    {
        #region props
        private int _wifiStr;
        private bool _chartVisibility;
        private string _long, _lat, _;
        public string Longitude
        {
            get
            {
                return _long;
            }
            set
            {
                _long = value;

                OnPropertyChanged("Longitude");
            }
        }
        public string Latitude
        {
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;

                OnPropertyChanged("Latitude");
            }
        }
        public int WifiStr
        {
            get => _wifiStr;
            set
            {
                _wifiStr = value;
                OnPropertyChanged("WifiStr");
            }
        }
        public bool ChartVisibility
        {
            get => _chartVisibility;
            set
            {
                _chartVisibility = value;
                OnPropertyChanged("ChartVisibility");
            }
        }

        public ObservableCollection<TrackPoint> trackPoints { get; set; }
        //public ObservableCollection<Microcharts.ChartEntry> chartEntries { get; set; }

        public List<ChartEntry> chartEntries { get; set; }


        private LineChart _lineChart;
        public LineChart LineChartWifi
        {
            get => _lineChart;
            set
            {
                _lineChart = value;
                OnPropertyChanged("LineChart");
            }
        }

        #endregion

        private ILocationUpdateService _LocationUpdateService;
        private IWifiTracker _wifiTracker;

        public MainViewModel(ILocationUpdateService locationUpdateService, IWifiTracker wifiTracker)
        {
            ChartVisibility = false;
            chartEntries = new List<ChartEntry>();
            trackPoints = new ObservableCollection<TrackPoint>();
            trackPoints.Reverse<TrackPoint>();
            LineChartWifi = new LineChart();            
            _LocationUpdateService = locationUpdateService;
            _LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged;
            _wifiTracker = wifiTracker;
            _wifiTracker.GetWifiSignalRecived += SetSignalStenght;
        }

        private void SetSignalStenght(object sender, IWifiSignalEventArgs e)
        {
            WifiStr = e.WifiSignalStrRecived;
            TrackPoint t = new TrackPoint()
            { Latitude = Latitude, Longitude = Longitude, WifiStr = $"{e.WifiSignalStrRecived}" };
            trackPoints.Add(t);
            
            chartEntries.Add(new Microcharts.ChartEntry(e.WifiSignalStrRecived)
            {
                Color = SKColor.Parse("#FF4081"),
                Label = " ",
                ValueLabel = " "
            });
            LineChartWifi.Entries = chartEntries;
            StopWifiTracker();
        }

        public async void GetPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (status == PermissionStatus.Granted)
            {
                GetCurrentLocation();
            }
            else
            {
                var getPermissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (getPermissions == PermissionStatus.Granted)
                {
                    GetCurrentLocation();
                }
            }
        }

        public void StopGPS()
        {
            _LocationUpdateService.CloseService();
            StopWifiTracker();
            DrawChart();
        }

        private void DrawChart()
        {
            ChartVisibility = true;
            var chartEntriesLinearChart = new List<ChartEntry>();
            if (chartEntries.Count > 0)
            {
                
                foreach (var item in chartEntries)
                {
                    chartEntriesLinearChart.Add(item);
                }

                LineChartWifi = new LineChart()
                {
                    Entries = chartEntriesLinearChart,
                    LabelTextSize = 30f,
                    IsAnimated = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                };
            }

        }

        public void StopWifiTracker()
        {
            _wifiTracker.StopScanWifi();
        }

        private void GetCurrentLocation()
        {
            _LocationUpdateService.StartService();

        }

        private void LocationUpdateService_LocationChanged(object sender, ILocationEventArgs e)
        {
            Latitude = e.Latitude.ToString();
            Longitude = e.Longitude.ToString();
            _wifiTracker.StarScanWifi();

        }

    }
}
