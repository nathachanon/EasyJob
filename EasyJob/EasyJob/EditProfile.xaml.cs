using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfile 
    {
        private readonly HttpClient _client = new HttpClient();
        private const string url = "http://139.180.129.212/api/Member/edit";
        public EditProfile()
        {
            InitializeComponent();
            profile_img.Source = Application.Current.Properties["profile"].ToString();
        }

        private void Button_Exit(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync(true);
        }

        private async void Button_Save(object sender, EventArgs e)
        {
            var names = name.Text;
            var surnames = surname.Text;
            var tels = tel.Text;
            if (names != null && surnames != null && tels != null)
            {
                var member_ids = Application.Current.Properties["member_id"].ToString();
                string sContentType = "application/json";
                var jsonData = "{\"member_id\":\"" + member_ids + "\",\"name\":\"" + names + "\",\"surname\":\"" + surnames + "\",\"tel\":\"" + tels + "\"}";
                var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

                using (HttpResponseMessage response = await _client.PostAsync(url, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Frame_main.IsVisible = false;
                        animationSuccess.IsVisible = true;
                        Frame_success.IsVisible = true;
                        Frame_success_text.IsVisible = true;

                        await Task.Delay(2000);

                        PopupNavigation.PopAsync(true);

                        var page = new EasyJob.MenuBar();
                        page.CurrentPage = page.Children[3];
                        Application.Current.MainPage = new NavigationPage(page)
                        {
                            BarBackgroundColor = Color.FromHex("#031765"),
                            BarTextColor = Color.White
                        };
                    }
                    else
                    {
                        await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                    }
                }
            }
            else
            {
                await DisplayAlert("เกิดข้อผิดพลาด", "กรอกข้อมูลให้ครบ", "ตกลง");
            }
        }
    }
}