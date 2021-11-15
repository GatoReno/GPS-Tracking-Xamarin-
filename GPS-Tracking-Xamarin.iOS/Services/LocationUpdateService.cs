using System;
using CoreLocation;
using GPS_Tracking_Xamarin.iOS.Services;
using GPSTrackingXamarin.Abstractions;

[assembly: Xamarin.Forms.Dependency(typeof(LocationUpdateService))]
namespace GPS_Tracking_Xamarin.iOS.Services
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationUpdateService : ILocationUpdateService
    {
        CLLocationManager locationManager;

        public LocationUpdateService()
        {
            locationManager = new CLLocationManager();
            locationManager.LocationsUpdated +=
                (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var locations = e.Locations;
                    var strLocation = locations[locations.Length - 1].Coordinate.Latitude.ToString();

                    strLocation = strLocation + "," + locations[locations.Length - 1].Coordinate.Longitude.ToString();

                    LocationEventArgs args = new LocationEventArgs();
                    args.Latitude = locations[locations.Length - 1].Coordinate.Latitude;
                    args.Longitude = locations[locations.Length - 1].Coordinate.Longitude;

                    LocationChanged(this, args);
                };
        }

        public event EventHandler<ILocationEventArgs> LocationChanged;

        event EventHandler<ILocationEventArgs> ILocationUpdateService.LocationChanged
        {
            add
            {
                LocationChanged += value;
            }
            remove
            {
                LocationChanged -= value;
            }
        }

        public void StartService()
        {
            locationManager.StartUpdatingLocation();
        }

        public void CloseService()
        {
            locationManager.StopUpdatingLocation();
        }

        ~LocationUpdateService()
        {
            locationManager.StopUpdatingLocation();
        }
    }
}
