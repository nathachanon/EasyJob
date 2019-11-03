using EasyJob.model;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
         
        }

        void Button_Clicked(object sender, EventArgs e)
        {           
            var boxEnt = boxEn.Text;
            var boxEnt1 = boxEn1.Text;
            DisplayAlert("selected", boxEnt, boxEnt1, "OK");
        }

    }
}