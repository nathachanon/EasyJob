using EasyJob.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/GetWorkDetail";
        private const string Url2 = "http://139.180.129.212/api/Work/GetJob";
        private string api_key = "AIzaSyA_v0m54p_3BHjOdaeteyY3VfvqoURhJ8Q";
        private Guid work_id;
        public PopWorkDetail(string work_ids)
        {
            InitializeComponent();
            work_id = Guid.Parse(work_ids);
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            var Latitude = position.Latitude;
            var Longitude = position.Longitude;

            string GetAddress = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Latitude.ToString() + "," + Longitude + "&key=" + api_key + "&language=th";
            string sContentType = "application/json";
            var jsonData = "{\"work_id\":\"" + work_id + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        List<Work_Detail> work_list = JsonConvert.DeserializeObject<List<Work_Detail>>(mycontent);

                        var latlong = "";

                        for (var j = 0; j < work_list.Count; j++)
                        {
                            latlong += work_list[j].lat + "," + work_list[j].@long + "|";
                        }

                        string GetAddress2 = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + Latitude + "," + Longitude + "&destinations=" + latlong + "&mode=driving&language=th-TH&sensor=false&key=AIzaSyA_v0m54p_3BHjOdaeteyY3VfvqoURhJ8Q&language=th";
                        using (HttpResponseMessage response2 = await _client.GetAsync(GetAddress2))
                        {
                            if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                using (HttpContent contents2 = response2.Content)
                                {
                                    string mycontent2 = await contents2.ReadAsStringAsync();
                                    JObject maps_details = JObject.Parse(mycontent2);
                                    var map_d = maps_details["rows"][0]["elements"][0]["distance"]["text"].ToString();

                                    foreach (var x in work_list)
                                    {
                                        lb_owner_name.Text = x.job_owner_name.ToString();
                                        lb_owner_tel.Text = x.tel + "";
                                        lb_work_name.Text = x.work_name.ToString();
                                        lb_work_desc.Text = x.work_desc.ToString();
                                        lb_work_duration.Text = x.duration.ToString();
                                        lb_labor_cost.Text = x.labor_cost.ToString();
                                        lb_loc.Text = x.loc_name.ToString();
                                        lb_distance.Text = map_d;

                                        DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(x.lat, x.@long),
                            Distance.FromKilometers(1)));
                                        var pin = new Pin
                                        {
                                            Position = new Position(x.lat, x.@long),
                                            Label = "สถานที่ทำงาน",
                                            Address = x.loc_name
                                        };

                                        var pin2 = new Pin
                                        {
                                            Position = new Position(Latitude, Longitude),
                                            Label = "ที่อยู่ปัจจุบันของคุณ"
                                        };

                                        DetailMap.Pins.Add(pin);
                                        DetailMap.Pins.Add(pin2);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }
        }

        private async void GetJob_Clicked(object sender, EventArgs e)
        {
            var member_id = Application.Current.Properties["member_id"].ToString();

            string sContentType = "application/json";
            var jsonData = "{\"work_id\":\"" + work_id + "\",\"member_id\":\"" + member_id + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url2, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Success", "รับงานเรียบร้อย", "ตกลง");

                    PopupNavigation.PopAsync(true);

                    var page = new EasyJob.MenuBar();
                    page.CurrentPage = page.Children[0];
                    Application.Current.MainPage = new NavigationPage(page)
                    {
                        BarBackgroundColor = Color.FromHex("#031765"),
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    await DisplayAlert("Error", "เกิดข้อผิดพลาดบางอย่าง", "ตกลง");
                }
            }
        }
    }
}