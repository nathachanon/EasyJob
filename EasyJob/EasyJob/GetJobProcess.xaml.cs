﻿using EasyJob.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetJobProcess : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/work_getprocess";
        private ObservableCollection<Job> my_work_get_process;
        public GetJobProcess()
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
                        my_work_get_process = new ObservableCollection<Job>(work_list);
                        ItemlistView.ItemsSource = my_work_get_process;

                        job_count.Text = work_list.Count.ToString() + " รายการ";
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }
        }
        private void Tap(object sender, EventArgs e)
        {

        }

    }
}