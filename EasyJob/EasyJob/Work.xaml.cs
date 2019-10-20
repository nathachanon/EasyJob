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
    public partial class Work : ContentPage
    {
        public Work()
        {
            InitializeComponent();
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new RWorkPopup());
        }
        private void Tap(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new RWorkPopup());
        }
    }
}