using EasyJob.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        private readonly HttpClient _client = new HttpClient();
        private ObservableCollection<Member> mem_data;
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            string url = "http://139.180.129.212/api/Member/Login";
            string sContentType = "application/json";
            var jsonData = "{\"email\":\"" + Email_Input.Text + "\",\"password\":\"" + Password_Input.Text + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        List<Member> mem_list = JsonConvert.DeserializeObject<List<Member>>(mycontent);
                        mem_data = new ObservableCollection<Member>(mem_list);

                        Application.Current.Properties["name"] = mem_data[0].name;
                        Application.Current.Properties["member_id"] = mem_data[0].member_id;

                        await DisplayAlert("สำเร็จ", "Login Success", "ตกลง");

                        App.Current.MainPage = new NavigationPage(new MenuBar())
                        {
                            BarBackgroundColor = Color.FromHex("#031765"),
                            BarTextColor = Color.White,
                        };
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Email หรือ Password ไม่ถูกต้อง", "ตกลง");
                }
            }
        }

    }
}