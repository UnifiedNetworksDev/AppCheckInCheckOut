﻿using System;
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
        public Menu()
        {
            this.token = PropertiesOperations.GetTokenProperties();
            InitializeComponent();
        }

        void HandleResult(ZXing.Result result)
        {
            var msg = "No Bardecode";
            if (result != null)
            {
                msg = "Barcode:" + result.Text + "(" + result.BarcodeFormat + ")";

                proyectoN.Text = result.Text;
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
                actLoading.IsRunning = false;
            }
            catch (Exception ex)
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
                        actLoading.IsRunning = true;
                        ProjectService service = new ProjectService();
                        await service.ValidateProject(proyectoN.Text.Trim());

                        DateTime checkIn = DateTime.Now;

                        var position = await CrossGeolocator.Current.GetPositionAsync();
                        App.Current.Properties["latitudeCheckIn"] = position.Latitude.ToString();
                        App.Current.Properties["longitudeCheckIn"] = position.Longitude.ToString();
                        App.Current.Properties["dateCheckIn"] = checkIn;

                        await DisplayAlert("Check in", String.Format("Has realizado check in a las {0} ", checkIn.ToString("hh:mm")), "OK");
                        await Navigation.PushAsync(new Tareas(proyectoN.Text, token));
                        actLoading.IsRunning = false;
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

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            PropertiesOperations.RemoveProperties();
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync().ConfigureAwait(false);
        }
    }
}