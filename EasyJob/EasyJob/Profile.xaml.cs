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
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey("name"))
            {
                Member_Email.Text = Application.Current.Properties["name"].ToString();
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();

            App.Current.MainPage = new NavigationPage(new Login());
        }
    }
}