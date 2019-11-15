using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopAddWork {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/Work/AddWork";
        private string api_key = "AIzaSyA_v0m54p_3BHjOdaeteyY3VfvqoURhJ8Q";
        private string duration_select = "";
        private double Latitude = 0.0;
        private double Longitude = 0.0;
        private string location_details = "";
        public PopAddWork()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            Latitude = position.Latitude;
            Longitude = position.Longitude;
            string GetAddress = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Latitude.ToString() + "," + Longitude + "&key=" + api_key + "&language=th";

            DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude),
                Distance.FromKilometers(1)));

            using (HttpResponseMessage response = await _client.GetAsync(GetAddress))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        JObject maps_details = JObject.Parse(mycontent);
                        location_details = maps_details["results"][0]["formatted_address"].ToString();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "เกิดข้อผิดพลาดกรุณาลองใหม่", "OK");
                }
            }

            var pin = new Pin
            {
                Position = new Position(Latitude, Longitude),
                Label = "ที่อยู่ปัจจุบัน",
                Address = location_details
            };

            DetailMap.Pins.Add(pin);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var member_id = Application.Current.Properties["member_id"].ToString();
            var work_name = Input_Work_name.Text;
            var work_desc = Input_Work_desc.Text;
            var labor_cost = Input_cost.Text;
            var tel = Input_tel.Text;
            String myDate = DateTime.Now.ToString();

            if (member_id != "" && work_name != "" && work_desc != "" && labor_cost != "" && tel != "" && location_details != "" && Latitude != 0.0 && Longitude != 0.0)
            {
                string sContentType = "application/json";
                var jsonData = "{\"member_id\":\"" + member_id + "\",\"work_name\":\"" + work_name + "\"," +
                    "\"work_desc\":\"" + work_desc + "\"," +
                    "\"tel\":\"" + tel + "\",\"labor_cost\":\"" + labor_cost + "\",\"duration\":\"" + duration_select + "\"," +
                    "\"datetime\":\"" + myDate + "\",\"lat\":\"" + Latitude + "\"," +
                    "\"long\":\"" + Longitude + "\",\"loc_name\":\"" + location_details + "\"}";
                var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

                using (HttpResponseMessage response = await _client.PostAsync(Url, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent contents = response.Content)
                        {
                            string mycontent = await contents.ReadAsStringAsync();
                            JObject add_work = JObject.Parse(mycontent);

                            Frame_main.IsVisible = false;
                            animationSuccess.IsVisible = true;
                            Frame_success.IsVisible = true;
                            Frame_success_text.IsVisible = true;

                            await Task.Delay(2000);

                            PopupNavigation.PopAsync(true);

                            var page = new EasyJob.MenuBar();
                            page.CurrentPage = page.Children[2];
                            Application.Current.MainPage = new NavigationPage(page)
                            {
                                BarBackgroundColor = Color.FromHex("#031765"),
                                BarTextColor = Color.White
                            };
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "เกิดข้อผิดพลาดกรุณาลองใหม่", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "กรอกข้อมูลให้ครบ", "OK");
            }
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            var pc = picker.SelectedIndex;
            switch (pc)
            {
                case 0:
                    duration_select = "1 ชั่วโมง"; break;
                case 1:
                    duration_select = "2 ชั่วโมง"; break;
                case 2:
                    duration_select = "3 ชั่วโมง"; break;
                case 3:
                    duration_select = "6 ชั่วโมง"; break;
                case 4:
                    duration_select = "8 ชั่วโมง"; break;
                case 5:
                    duration_select = "ทั้งวัน"; break;
                case 6:
                    duration_select = "มากกว่า 1 วัน"; break;
                default:
                    duration_select = "ไม่ระบุ"; break;
            }
        }

        private async void DetailMap_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            Latitude = e.Position.Latitude;
            Longitude = e.Position.Longitude;

            string GetAddress = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Latitude.ToString() + "," + Longitude + "&key=" + api_key + "&language=th";

            using (HttpResponseMessage response = await _client.GetAsync(GetAddress))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        JObject maps_details = JObject.Parse(mycontent);
                        location_details = maps_details["results"][0]["formatted_address"].ToString();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "เกิดข้อผิดพลาดกรุณาลองใหม่", "OK");
                }
            }

            var pin = new Pin
            {
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = "สถานที่ทำงาน",
                Address = location_details
            };

            DetailMap.Pins.Clear();
            DetailMap.Pins.Add(pin);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync(true);
        }
    }
}