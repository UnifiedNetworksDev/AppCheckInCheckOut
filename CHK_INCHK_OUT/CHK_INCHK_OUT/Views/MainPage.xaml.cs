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
                //insert menu in stack and remove login
                Navigation.InsertPageBefore(new Menu(token), this);
                await Navigation.PopAsync().ConfigureAwait(false);
                actLoading.IsRunning = false;
                BtnLogin.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                actLoading.IsRunning = false;
                BtnLogin.IsEnabled = true;
            }
        }
    }
}
