using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
          
            
            if (Email_Input.Text == null || Password_Input.Text == null || CPassword_Input.Text == null || name_Input.Text == null || Surname_Input.Text == null || Tel_Input.Text == null)
            {
                await DisplayAlert("Register", "กรอกข้อมูลให้ครบ", "Ok");
            }
            else if (Password_Input.Text != CPassword_Input.Text)
            {
                await DisplayAlert("Register", "กรุณากรอก Password ให้ตรงกัน", "Ok");
            }
        }

        private void ToLogin(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
