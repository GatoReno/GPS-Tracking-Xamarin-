using System;

namespace GPSTrackingXamarin.Abstractions
{
    public interface ILocationUpdateService
    {
        void StartService();
        void CloseService();
        event EventHandler<ILocationEventArgs> LocationChanged;
    }

    public interface ILocationEventArgs
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
