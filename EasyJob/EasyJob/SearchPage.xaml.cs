﻿using EasyJob.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private const string Url = "http://139.180.129.212/api/work/search";
        private ObservableCollection<Work_dis> work_all;
        private string api_key = "AIzaSyA_v0m54p_3BHjOdaeteyY3VfvqoURhJ8Q";
        public SearchPage()
        {
            InitializeComponent();
            ItemlistView.IsVisible = false;
            animationView.IsVisible = false;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = (TappedEventArgs)e;
            var work_id = args.Parameter;

            await PopupNavigation.Instance.PushAsync(new PopWorkDetail(work_id.ToString()));
        }
        override protected void OnAppearing()
        {
            if (keyword.Text == null)
            {
                ItemlistView.IsVisible = false;
                animationView.IsVisible = false;
            }
        }
        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if(animationView.IsVisible == true)
            {
                await DisplayAlert("Error", "กรุณารอก่อน ระบบกำลังค้นหา", "ตกลง");
            }
            else
            {
                if (keyword.Text != null && keyword.Text != "")
                {
                    ItemlistView.IsVisible = false;
                    animationView.IsVisible = true;
                    //Loading.IsVisible = true;
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync();
                    var Latitude = position.Latitude;
                    var Longitude = position.Longitude;
                    var search_text = keyword.Text;

                    string sContentType = "application/json";
                    var member_ids = Application.Current.Properties["member_id"].ToString();
                    var jsonData = "{\"member_id\":\"" + member_ids + "\",\"name\":\"" + search_text + "\"}";
                    var content = new StringContent(jsonData.ToString(), Encoding.UTF8, sContentType);

                    using (HttpResponseMessage response = await _client.PostAsync(Url, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (HttpContent contents = response.Content)
                            {
                                string mycontent = await contents.ReadAsStringAsync();
                                List<Work_dis> work_list = JsonConvert.DeserializeObject<List<Work_dis>>(mycontent);
                                List<Work_dis> subjects = new List<Work_dis>();

                                var latlong = "";
                                for (var j = 0; j < work_list.Count; j++)
                                {
                                    latlong += work_list[j].lat + "," + work_list[j].@long + "|";
                                }

                                string GetAddress = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + Latitude + "," + Longitude + "&destinations=" + latlong + "&mode=driving&language=th-TH&sensor=false&key=AIzaSyA_v0m54p_3BHjOdaeteyY3VfvqoURhJ8Q&language=th";
                                using (HttpResponseMessage response2 = await _client.GetAsync(GetAddress))
                                {
                                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        using (HttpContent contents2 = response2.Content)
                                        {
                                            string mycontent2 = await contents2.ReadAsStringAsync();
                                            JObject maps_details = JObject.Parse(mycontent2);
                                            for (var i = 0; i < work_list.Count; i++)
                                            {
                                                subjects.Add(new Work_dis
                                                {
                                                    work_name = work_list[i].work_name,
                                                    work_desc = work_list[i].work_desc,
                                                    work_id = work_list[i].work_id,
                                                    labor_cost = work_list[i].labor_cost,
                                                    distance = maps_details["rows"][0]["elements"][i]["distance"]["text"].ToString()
                                                });
                                            }

                                            work_all = new ObservableCollection<Work_dis>(subjects);
                                            ItemlistView.ItemsSource = work_all;
                                            Search_count.Text = "พบ " + work_list.Count.ToString() + " รายการ";
                                            //Loading.IsVisible = false;
                                            animationView.IsVisible = false;
                                            ItemlistView.IsVisible = true;
                                        }
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
                else
                {
                    ItemlistView.IsVisible = false;
                    animationView.IsVisible = false;
                    Search_count.Text = "พบ 0 รายการ";
                    await DisplayAlert("Error", "ช่องค้นหาไม่สามารถเป็นค่าว่างได้", "ตกลง");
                }
            }
            
        }
    }
}