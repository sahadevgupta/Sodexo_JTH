using Sodexo_JTH.Models;
using Sodexo_JTH.ViewModels;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Sodexo_JTH.Views
{
    public partial class DailyOrderDetailPage : ContentPage
    {
        DailyOrderDetailPageViewModel _viewModel;
        public DailyOrderDetailPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as DailyOrderDetailPageViewModel;
            _viewModel._iNavigation = Navigation;
        }

        private void ScanImageTapped(object sender, System.EventArgs e)
        {

            var SelectedItem = ((ImageButton)sender).BindingContext as mstr_mealdelivered;
            _viewModel.SelectedOrderDetail = SelectedItem;
            ZXingScannerPage scan = new ZXingScannerPage();
            // await Navigation.PushAsync(scan);
            Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                scan.IsScanning = false;
                ZXing.BarcodeFormat barcodeFormat = result.BarcodeFormat;
                string type = barcodeFormat.ToString();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //Navigation.PopAsync();
                    await Navigation.PopAsync();
                    string scanned_orderid = result.Text;
                    Image cmd = sender as Image;
                    _viewModel.ScanDeliveredOD(scanned_orderid, SelectedItem.OrderedID.ToString());
                });
            };

        }
    }
}
