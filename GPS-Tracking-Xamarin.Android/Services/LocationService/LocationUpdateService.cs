using System;
using System.Runtime.Remoting.Contexts;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;
using GPS_Tracking_Xamarin.Droid.Services;
using GPSTrackingXamarin.Abstractions;

[assembly: Dependency(typeof(LocationUpdateService))]
namespace GPS_Tracking_Xamarin.Droid.Services
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationUpdateService : Java.Lang.Object, ILocationUpdateService, ILocationListener
    {
        LocationManager locationManager;

        public void StartService()
        {
            locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);
            locationManager.RequestLocationUpdates(
                provider: LocationManager.NetworkProvider, //GpsProvider for android lvl 9 an below
                minTimeMs: 0,
                minDistanceM: 0,
                listener: this);
        }

        public void CloseService()
        {
            if (locationManager != null)
            {
                locationManager.RemoveUpdates(this);
                locationManager.Dispose();
            }
                     
        }

        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
                LocationChanged(this, args);
            };
        }

        public event EventHandler<ILocationEventArgs> LocationChanged;

        event EventHandler<ILocationEventArgs>
            ILocationUpdateService.LocationChanged
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

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }


    }
}
