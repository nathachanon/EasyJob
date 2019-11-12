using EasyJob.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyJob
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/allWork_blank";
        private ObservableCollection<Work_all> work_all;
        public MainPage()
        {
            InitializeComponent();          
        }
        async override protected void OnAppearing()
        {
            ItemlistView.IsVisible = false;
            //Loading.IsVisible = true;

            string sContentType = "application/json";
            var member_ids = Application.Current.Properties["member_id"].ToString();
            var jsonData = "{\"member_id\":\"" + member_ids + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        List<Work_all> work_list = JsonConvert.DeserializeObject<List<Work_all>>(mycontent);
                        work_all = new ObservableCollection<Work_all>(work_list);
                        ItemlistView.ItemsSource = work_all;

                        //Loading.IsVisible = false;
                        ItemlistView.IsVisible = true;
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }
            
            base.OnAppearing();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = (TappedEventArgs)e;
            var work_id = args.Parameter;

            await PopupNavigation.Instance.PushAsync(new PopWorkDetail(work_id.ToString()));
        }
    }
}
