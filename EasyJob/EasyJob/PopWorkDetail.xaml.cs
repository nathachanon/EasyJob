using EasyJob.Models;
using Newtonsoft.Json;
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
        private Guid work_id;
        public PopWorkDetail(string work_ids)
        {
            InitializeComponent();
            work_id = Guid.Parse(work_ids);
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();
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
                        foreach (var x in work_list)
                        {
                            lb_owner_name.Text = x.job_owner_name.ToString();
                            lb_owner_tel.Text = x.tel+"";
                            lb_work_name.Text = x.work_name.ToString();
                            lb_work_desc.Text = x.work_desc.ToString();
                            lb_work_duration.Text = x.duration.ToString();
                            lb_labor_cost.Text = x.labor_cost.ToString();
                            lb_loc.Text = x.loc_name.ToString();

                            /* เรียก Location จาก GPS
                            var locator = CrossGeolocator.Current;
                            var position = await locator.GetPositionAsync();
                            var lat = position.Latitude;
                            var @long = position.Longitude;
                            */

                            DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(x.lat, x.@long),
                Distance.FromKilometers(1)));
                            var pin = new Pin
                            {
                                Position = new Position(x.lat, x.@long),
                                Label = "สถานที่ทำงาน",
                                Address = x.loc_name
                            };

                            DetailMap.Pins.Add(pin);
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