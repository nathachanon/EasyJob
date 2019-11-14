using EasyJob.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class Profile : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private const string url = "http://139.180.129.212/api/work/post_get_process_finish_me";
        public Profile()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            var member_ids = Application.Current.Properties["member_id"].ToString();

            if (Application.Current.Properties.ContainsKey("name"))
            {
                Member_Email.Text = Application.Current.Properties["name"].ToString();
                profile_img.Source = "http://139.180.129.212/Member_image/"+Application.Current.Properties["profile"].ToString();
            }

            string sContentType = "application/json";
            var jsonData = "{\"member_id\":\"" + member_ids + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            using (HttpResponseMessage response = await _client.PostAsync(url, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (HttpContent contents = response.Content)
                    {
                        string mycontent = await contents.ReadAsStringAsync();
                        JObject me_work_list = JObject.Parse(mycontent);
                        List<Jobs> get_job_process = JsonConvert.DeserializeObject<List<Jobs>>(me_work_list["get_job_process"].ToString());
                        List<Jobs> get_job_finish = JsonConvert.DeserializeObject<List<Jobs>>(me_work_list["get_job_finish"].ToString());
                        List<Jobs> post_job_process = JsonConvert.DeserializeObject<List<Jobs>>(me_work_list["post_job_process"].ToString());
                        List<Jobs> post_job_finish = JsonConvert.DeserializeObject<List<Jobs>>(me_work_list["post_job_finish"].ToString());
                        List<Job> get_job = JsonConvert.DeserializeObject<List<Job>>(me_work_list["get_job"].ToString());
                        List<Job> post_job = JsonConvert.DeserializeObject<List<Job>>(me_work_list["post_job"].ToString());

                        lb_getjob_count.Text = get_job.Count.ToString();
                        lb_postjob_count.Text = post_job.Count.ToString();
                    }
                }
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();

            App.Current.MainPage = new NavigationPage(new Login());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetJobPage());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var page = new EasyJob.MenuBar();
            page.CurrentPage = page.Children[2];
            Application.Current.MainPage = new NavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#031765"),
                BarTextColor = Color.White
            };
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetJobProcess());
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetJobSuccess());
        }
    }
}