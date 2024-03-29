﻿using EasyJob.Models;
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
    public partial class WorkPopup
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/GetWorkDetail";
        private const string Url2 = "http://139.180.129.212/api/work_start";
        private const string Url3 = "http://139.180.129.212/api/work_finish";
        private const string Url_img = "http://139.180.129.212/Member_image/";
        private Guid work_id;
        public WorkPopup(string work_ids)
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
                        foreach(var x in work_list)
                        {
                            worker_name.Text = x.worker_name;
                            worker_tel.Text = x.worker_tel;
                            worker_img.Source = Url_img + x.worker_profile;
                            lb_work_name.Text = x.work_name;
                            lb_work_desc.Text = x.work_desc;
                            lb_labor_cost.Text = x.labor_cost.ToString() +" บาท";
                            

                            if(x.job_status == "ว่าง")
                            {
                                Title.TextColor = Color.FromHex("#D52A2A");
                                Button_Process.Text = x.job_status;
                                Button_Process.BackgroundColor = Color.Red;
                                Button_Process.IsEnabled = false;
                            }else if(x.job_status == "มีผู้รับงานแล้ว")
                            {
                                Title.TextColor = Color.FromHex("#D5852A");
                                Title.Text = "มีผู้รับงานเรียบร้อย";
                                Button_Process.Text = "เริ่มงาน";
                                Button_Process.BackgroundColor = Color.Green;
                                Button_Process.IsEnabled = true;
                            }else if(x.job_status == "เริ่มงาน")
                            {
                                Title.TextColor = Color.FromHex("#3A2AD5");
                                Title.Text = "อยู่ระหว่างการทำงาน";
                                Button_Process.Text = "เสร็จงาน";
                                Button_Process.BackgroundColor = Color.Gold;
                                Button_Process.IsEnabled = true;
                            }
                            else
                            {
                                
                                Button_Process.Text = "งานนี้เสร็จแล้ว";
                                Button_Process.BackgroundColor = Color.White;
                                Button_Process.IsEnabled = false;
                            }
                        }
                        await Task.Delay(400);
                        animationView.IsVisible = false;
                        info.IsVisible = true;
                    }
                }
                else
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", "Member id is null", "ตกลง");
                }
            }

            base.OnAppearing();
        }

        private async void Job_Process(object sender, EventArgs e)
        {
            var member_id = Application.Current.Properties["member_id"].ToString();
            var type = Button_Process.Text;
            string sContentType = "application/json";
            var jsonData = "{\"work_id\":\"" + work_id + "\",\"member_id\":\"" + member_id + "\"}";
            var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

            if(type == "เริ่มงาน")
            {
                using (HttpResponseMessage response = await _client.PostAsync(Url2, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await DisplayAlert("Success", "เริ่มงานเรียบร้อย", "ตกลง");

                        PopupNavigation.PopAsync(true);

                        var page = new EasyJob.MenuBar();
                        page.CurrentPage = page.Children[2];
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
            } else if(type == "เสร็จงาน")
            {
                using (HttpResponseMessage response = await _client.PostAsync(Url3, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await DisplayAlert("Success", "งานที่โพสต์เสร็จแล้ว", "ตกลง");

                        PopupNavigation.PopAsync(true);

                        var page = new EasyJob.MenuBar();
                        page.CurrentPage = page.Children[2];
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
}