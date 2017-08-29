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
        //bool check = true;
        string nproyecto;
        Token token;
        private ActivityViewModels viewModel;

        public Tareas(string nProyecto, Token token)
        {
            this.nproyecto = nProyecto;
            this.token = token;
            InitializeComponent();

            try
            {
                BindingContext = viewModel = new ActivityViewModels(nproyecto);
                lblProject.Text = viewModel.ActividadesList[0].activityInformation.projectName;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (App.Current.Properties.ContainsKey("checkIn"))
                        App.Current.Properties.Remove("checkIn");

                    await this.DisplayAlert("Error", ex.InnerException.Message, "OK");
                    await this.Navigation.PopAsync(); // or anything else
                });
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            btnSend.IsEnabled = false;
            try
            {
                var list = viewModel.GetSelected(); //get activities checked
                //at least one element selected
                if (list.Count > 0)
                {
                    actLoading.IsRunning = true;

                    var position = await CrossGeolocator.Current.GetPositionAsync();
                    string latitud = position.Latitude.ToString();
                    string longitud = position.Longitude.ToString();

                    List<HistorialActivity> dataCheckIn = new List<HistorialActivity>();

                    foreach (var lista in list)
                    {
                        HistorialActivity info = new HistorialActivity();
                        info.activityID = lista.activityInformation.idActivity.ToString();
                        info.checkInCheckOut = true;
                        info.userID = this.token.UserIDCRM;
                        if (App.Current.Properties.ContainsKey("latitudeCheckIn"))
                            info.latitude = (string)App.Current.Properties["latitudeCheckIn"];

                        if (App.Current.Properties.ContainsKey("longitudeCheckIn"))
                            info.longitude = (string)App.Current.Properties["longitudeCheckIn"];

                        if (App.Current.Properties.ContainsKey("dateCheckIn"))
                            info.date = (DateTime)App.Current.Properties["dateCheckIn"];

                        dataCheckIn.Add(info);
                    }

                    HistorialServices service = new HistorialServices();
                    //if (check)
                    //{
                    await service.PutAsyncCheckIn(dataCheckIn);
                    //await DisplayAlert("Check in", String.Format("Has realizado check in a las {0} ", fecha.ToString("hh:mm")), "OK");
                    //check = false;
                    //btnSend.Text = "Check out";
                    //}
                    //else
                    //{

                    List<HistorialActivity> dataCheckOut = new List<HistorialActivity>();
                    DateTime fecha = DateTime.Now;
                    foreach (HistorialActivity obj in dataCheckIn)
                    {
                        HistorialActivity data = new HistorialActivity();
                        data.activityID = obj.activityID;
                        data.checkInCheckOut = false;
                        data.date = fecha;
                        data.latitude = obj.latitude;
                        data.longitude = obj.longitude;
                        data.userID = obj.userID;
                        dataCheckOut.Add(data);
                    }

                    string message = await service.PutAsyncCheckOut(dataCheckOut);
                    await DisplayAlert("Check out", String.Format("Has realizado check out a las {0}. El tiempo transcurrido fue de: {1}", fecha.ToString("HH:mm"), message), "OK");
                    App.Current.Properties["checkIn"] = false;
                    await Navigation.PopAsync();
                    actLoading.IsRunning = false;
                }
                else
                {
                    await DisplayAlert("Error", "Favor de marcar como terminada al menos una actividad", "OK");
                }
                btnSend.IsEnabled = true;
            }
            catch (Exception ex)
            {
                actLoading.IsRunning = false;
                btnSend.IsEnabled = true;
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