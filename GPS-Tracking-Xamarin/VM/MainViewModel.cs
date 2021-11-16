using System;
using System.Collections.ObjectModel;
using GPSTrackingXamarin.Abstractions;
using GPSTrackingXamarin.Abstractions.WifiAbstractions;
using GPSTrackingXamarin.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GPSTrackingXamarin.VM
{
    public class MainViewModel : BaseViewModel
    {
        #region props
        private int _wifiStr;
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

        public ObservableCollection<TrackPoint> trackPoints { get; set; }

        #endregion

        private ILocationUpdateService _LocationUpdateService;
        private IWifiTracker _wifiTracker;

        public MainViewModel(ILocationUpdateService locationUpdateService, IWifiTracker wifiTracker)
        {
            trackPoints = new ObservableCollection<TrackPoint>();

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
            _wifiTracker.StopScanWifi();
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
