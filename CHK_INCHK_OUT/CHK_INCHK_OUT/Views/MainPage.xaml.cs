using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CHK_INCHK_OUT.Views;
using CHK_INCHK_OUT.Model;

namespace CHK_INCHK_OUT
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (App.Current.Properties.ContainsKey("logged") && ((bool)App.Current.Properties["logged"]))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await this.RemoveLogin();
                });
            }
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                BtnLogin.IsEnabled = false;
                actLoading.IsRunning = true;

                Services.UserServices postUer = new Services.UserServices();
                Model.Login data = new Model.Login(UserLg.Text, PassLg.Text);
                Token token = await postUer.PostUserAsync(data);
                //App.Current.Properties["token"] = token;
                PropertiesOperations.SetTokenProperties(token);
                await this.RemoveLogin();
                //await Navigation.PushAsync(new Menu());
                actLoading.IsRunning = false;
                BtnLogin.IsEnabled = true;
                App.Current.Properties["logged"] = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                actLoading.IsRunning = false;
                BtnLogin.IsEnabled = true;
            }
        }

        private async Task RemoveLogin()
        {
            try
            {
                //insert menu in stack and remove login
                Navigation.InsertPageBefore(new Menu(), this);
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
