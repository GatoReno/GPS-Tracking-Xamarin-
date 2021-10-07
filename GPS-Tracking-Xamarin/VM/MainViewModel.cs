using System;
using GPSTrackingXamarin.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GPSTrackingXamarin.VM
{
    public class MainViewModel : BaseViewModel
    {
        #region props
        private string _long, _lat;
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
        #endregion

        public ILocationUpdateService _LocationUpdateService;

        public MainViewModel()
        {
            _LocationUpdateService = DependencyService.Resolve<ILocationUpdateService>();
            _LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged;
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
        }

    }
}
