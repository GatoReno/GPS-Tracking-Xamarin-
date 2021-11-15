using System;
namespace GPSTrackingXamarin.Abstractions.WifiAbstractions
{
    public interface IWifiTracker
    {
        void StarScanWifi();
        void StopScanWifi();
        double GetSignalStenght();
        event EventHandler<IWifiSignalEventArgs> GetWifiSignalRecived;
    }

    public interface IWifiSignalEventArgs
    {
        int WifiSignalStrRecived { get; set; }        
    }
}
