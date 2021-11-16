using GPSTrackingXamarin.Abstractions;
using GPSTrackingXamarin.Abstractions.WifiAbstractions;
using GPSTrackingXamarin.VM;
using Xamarin.Forms;

namespace GPS_Tracking_Xamarin
{
    public partial class MainPage : ContentPage
    {
        MainViewModel _viewModel;
        ILocationUpdateService _locationService;
        IWifiTracker _wifiTracker;
        public MainPage()
        {
            InitializeComponent();
            _locationService = DependencyService.Resolve<ILocationUpdateService>();
            _wifiTracker = DependencyService.Resolve<IWifiTracker>();
            BindingContext = _viewModel = new MainViewModel(_locationService,_wifiTracker);

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
        }
    }
}
