using GPSTrackingXamarin.VM;
using GPSTrackingXamarin.Abstractions;
using Xamarin.Forms;

namespace GPS_Tracking_Xamarin
{
    public partial class MainPage : ContentPage
    {
        MainViewModel _viewModel;
        public ILocationUpdateService _LocationUpdateService;
        public MainPage()
        {
            InitializeComponent();
            _LocationUpdateService = DependencyService.Resolve<ILocationUpdateService>();
            BindingContext = _viewModel = new MainViewModel(_LocationUpdateService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.GetPermissions();
        }

        async void StopServiceButton_Clicked(System.Object sender, System.EventArgs e)
        {
            _viewModel.StopGPS();
            await DisplayAlert("GPS", "GPS Service Stoped", "Ok");
        }
    }
}
