using System;
namespace GPSTrackingXamarin.Abstractions.WifiAbstractions
{
    public interface IWifiTracker
    {
        void StarScanWifi();
        void StopScanWifi();
        double GetSignalStenght();
        IWifiCastReciver Handler { get; set; }
    }

    public interface IWifiCastReciver
    {
        void AddWifiSignalRecived(double wifiSignalStrenght);
    }
}
