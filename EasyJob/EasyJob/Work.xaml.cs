using EasyJob.Models;
using Newtonsoft.Json;
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
    public partial class Work : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/work_post";
        private const string Url2 = "http://139.180.129.212/api/GetWorkDetail";
        private ObservableCollection<Job> my_work_post;
        public Work()
        {
            InitializeComponent();
            
        }
        async override protected void OnAppearing()
        {
            base.OnAppearing();

            var member_ids = Application.Current.Properties["member_id"].ToString();
            string sContentType = "application/json";
            var jsonData = "{\"member_id\":\"" + member_ids + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        List<Job> work_list = JsonConvert.DeserializeObject<List<Job>>(mycontent);
                        my_work_post = new ObservableCollection<Job>(work_list);
                        ItemlistView.ItemsSource = my_work_post;

                        job_count.Text = work_list.Count.ToString() + " รายการ";
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PWorkPopup());
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = (TappedEventArgs)e;
            var work_id = args.Parameter;

            string sContentType = "application/json";
            var jsonData = "{\"work_id\":\"" + work_id + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(Url2, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        List<Work_Detail> work_list = JsonConvert.DeserializeObject<List<Work_Detail>>(mycontent);
                        foreach (var x in work_list)
                        {

                            if (x.job_status == "ว่าง")
                            {
                                PopupNavigation.Instance.PushAsync(new BWorkPopup(work_id.ToString()));
                            }
                            else
                            {
                                PopupNavigation.Instance.PushAsync(new WorkPopup(work_id.ToString()));
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
    }
}