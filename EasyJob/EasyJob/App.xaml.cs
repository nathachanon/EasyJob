﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MenuBar();
            MainPage = new NavigationPage(new MenuBar())
            {
                BarBackgroundColor = Color.FromHex("#031765"),
                BarTextColor = Color.White,
                
                
                
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
