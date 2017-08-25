using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode;
using Plugin.Geolocator;
using CHK_INCHK_OUT.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CHK_INCHK_OUT.Services;

namespace CHK_INCHK_OUT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        Token token;
        public Menu(Token token)
        {
            this.token = token;
            InitializeComponent();
        }

        void HandleResult(ZXing.Result result)
        {
            var msg = "No Bardecode";
            if (result != null)
            {
                msg = "Barcode:" + result.Text + "(" + result.BarcodeFormat + ")";

                proyectoN.Text = result.Text;
                actLoading.IsRunning = false;
            }
            else
            {
                actLoading.IsRunning = false;
            }
        }
        private async void Scanner_Tapped(object sender, EventArgs e)
        {
            try
            {
                var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
                options.PossibleFormats = new List<ZXing.BarcodeFormat>()
                {
                    ZXing.BarcodeFormat.EAN_8, ZXing.BarcodeFormat.EAN_13
                };
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan(options);

                actLoading.IsRunning = true;
                HandleResult(result);
            }
            catch 
            {

            }

        }

        private async void Send_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (proyectoN.Text != null)
                {
                    if (proyectoN.Text.Trim() != string.Empty)
                    {
                        ProjectService service = new ProjectService();
                        await service.ValidateProject(proyectoN.Text.Trim());
                        await Navigation.PushAsync(new Tareas(proyectoN.Text , token));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Ingrese o escanee el número de proyecto", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Ingrese o escanee el número de proyecto", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                actLoading.IsRunning = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync().ConfigureAwait(false);
        }
    }
}