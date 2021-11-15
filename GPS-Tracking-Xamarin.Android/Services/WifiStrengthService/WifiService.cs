using System;
using Android.Net.Wifi;
using GPS_Tracking_Xamarin.Droid.Services.WifiStrengthService;
using GPSTrackingXamarin.Abstractions.WifiAbstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(WifiService))]
namespace GPS_Tracking_Xamarin.Droid.Services.WifiStrengthService
{


    public class WifiSignalEventArgs : EventArgs, IWifiSignalEventArgs
    {
        public int WifiSignalStrRecived { get ; set; }
    }

    public class WifiService : Java.Lang.Object, IWifiTracker
    {
        WifiManager wifiManager;

        public WifiService()
        {
        }

        public event EventHandler<IWifiSignalEventArgs> GetWifiSignalRecived;

        event EventHandler<IWifiSignalEventArgs>
            IWifiTracker.GetWifiSignalRecived
        {
            add
            {
                GetWifiSignalRecived += value;
            }
            remove
            {
                GetWifiSignalRecived -= value;
            }
        }


        public double GetSignalStenght()
        {
            double _wifiLevel = 0.0;
            if (wifiManager == null)
            {
                wifiManager =
                (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);

                var connectioninfo = wifiManager.ConnectionInfo;

                _wifiLevel = WifiManager.CalculateSignalLevel(connectioninfo.Rssi, 100);
            }

            return _wifiLevel;
        }

       

        public void StarScanWifi()
        {
            if (wifiManager == null)
            {
                wifiManager =
                (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);

                var wifiList = wifiManager.ScanResults;
                foreach (var item in wifiList)
                {
                    var wifiLevel = WifiManager.CalculateSignalLevel(item.Level, 100);
                    Console.WriteLine($"Wifi SSID: {item.Ssid} - Strengh: {wifiLevel}");

                    WifiSignalEventArgs args = new WifiSignalEventArgs() {
                         WifiSignalStrRecived = wifiLevel
                    };

                    GetWifiSignalRecived(this,args);
                }                
            }

        }

        public void StopScanWifi()
        {
            if (wifiManager != null)
            {
                wifiManager.Dispose();
            }
        }
    }
}
