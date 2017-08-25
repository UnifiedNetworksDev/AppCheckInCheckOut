using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CHK_INCHK_OUT.Model;
using Plugin.Geolocator;
using CHK_INCHK_OUT.Services;

namespace CHK_INCHK_OUT.Views
{
   // [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tareas : ContentPage
    {
        bool check = true;
        string nproyecto;
        Token token;
        string latitud;
        string longitud;
        private ActivityViewModels viewModel;
       
        public Tareas(string nProyecto , Token token)
        {
            this.nproyecto = nProyecto;
            this.token = token;
            InitializeComponent();
            
            try
            {
                BindingContext = viewModel = new ActivityViewModels(nproyecto);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await this.DisplayAlert("Error", ex.InnerException.Message, "OK");
                    await this.Navigation.PopAsync(); // or anything else
                });
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                actLoading.IsRunning = true;
                var list = viewModel.GetSelected(); //get activities checked

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync();
                latitud = position.Latitude.ToString();
                longitud = position.Longitude.ToString();
                DateTime fecha = DateTime.Now;

                List<HistorialActivity> data = new List<HistorialActivity>();

                foreach (var lista in list)
                {
                    HistorialActivity info = new HistorialActivity();
                    info.activityID = lista.activityInformation.idActivity.ToString();
                    info.checkInCheckOut = check;
                    info.userID = this.token.UserIDCRM;
                    info.latitude = latitud;
                    info.longitude = longitud;
                    data.Add(info);
                }

                HistorialServices service = new HistorialServices();
                if (check)
                {
                    await service.PutAsyncCheckIn(data);
                    await DisplayAlert("Check in", String.Format("Has realizado check in a las {0} ", fecha.ToString("hh:mm")), "OK");
                    check = false;
                    btnSend.Text = "Check out";
                }
                else
                {
                    string message = await service.PutAsyncCheckOut(data);
                    await DisplayAlert("Check out", String.Format("Has realizado check out a las {0}. El tiempo transcurrido fue de: {1}", fecha.ToString("hh:mm"), message), "OK");
                    btnSend.Text = "Check in";
                    await Navigation.PopAsync();
                }
                actLoading.IsRunning = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error", "OK");
            }

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
           
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync().ConfigureAwait(false);
        }
    }
}