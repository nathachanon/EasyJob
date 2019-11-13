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
    public partial class PopAddWork { 
        public PopAddWork()
        {
            InitializeComponent();
            DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(13.2544565, 12.45486),
              Distance.FromKilometers(1)));
            var pin = new Pin
            {
                Position = new Position(13.2544565, 12.45486),
                Label = "สถานที่ทำงาน",
                Address = "testt"
            };

            DetailMap.Pins.Add(pin);
        }
    }
}