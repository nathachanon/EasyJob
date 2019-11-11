using EasyJob.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PWorkPopup 
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/Work/AddWork";
        private string duration_select = "";
        public PWorkPopup()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var member_id = Application.Current.Properties["member_id"].ToString();
            var work_name = Input_Work_name.Text;
            var work_desc = Input_Work_desc.Text;
            var labor_cost = Input_cost.Text;
            var tel = Input_tel.Text;
            var lat = Input_lat.Text;
            var longt = Input_long.Text;
            var loc_name = "null";
            String myDate = DateTime.Now.ToString();

            string sContentType = "application/json";
            var jsonData = "{\"member_id\":\"" + member_id + "\",\"work_name\":\"" + work_name + "\"," +
                "\"work_desc\":\"" + work_desc + "\"," +
                "\"tel\":\"" + tel + "\",\"labor_cost\":\"" + labor_cost + "\",\"duration\":\"" + duration_select + "\"," +
                "\"datetime\":\"" + myDate + "\",\"lat\":\"" + lat + "\"," +
                "\"long\":\"" + longt + "\",\"loc_name\":\"" + loc_name + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        JObject add_work = JObject.Parse(mycontent);

                        await DisplayAlert("Success", "โพสต์งานสำเร็จ", "OK");

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
                    DisplayAlert("Error", "เกิดข้อผิดพลาดกรุณาลองใหม่", "OK");
                }
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
    }
}