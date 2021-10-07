using GPSTrackingXamarin.VM;
using Xamarin.Forms;

namespace GPS_Tracking_Xamarin
{
    public partial class MainPage : ContentPage
    {
        MainViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MainViewModel();

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
