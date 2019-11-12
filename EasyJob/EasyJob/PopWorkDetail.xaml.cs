using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopWorkDetail 
    {
        public PopWorkDetail()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(13.916899681091309, 100.49309539794922),
                Distance.FromKilometers(1)));

            var pin = new Pin
            {
                Position = new Position(13.916899681091309, 100.49309539794922),
                Label = "สถานที่ทำงาน",
                Address = "เอา loc_name มาโชว์ "
            };

            DetailMap.Pins.Add(pin);
        }
    }
}