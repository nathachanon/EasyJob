using Rg.Plugins.Popup.Services;
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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();

        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           
        }
        private void Search_tap(object sender, EventArgs e)
        {
            DisplayAlert("ทดสอบ", keyword.Text, "ตกลง");
        }
    }
}