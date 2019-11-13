using EasyJob.Models;
using Newtonsoft.Json;
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
    public partial class BWorkPopup 
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/GetWorkDetail";
        private Guid work_id;
        public BWorkPopup(string work_ids)
        {
            InitializeComponent();
            work_id = Guid.Parse(work_ids);
        }

        async override protected void OnAppearing()
        {
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
                            lb_work_name.Text = x.work_name;
                            lb_work_desc.Text = x.work_desc;
                            lb_labor_cost.Text = x.labor_cost.ToString();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }

            base.OnAppearing();
        }

    }
}