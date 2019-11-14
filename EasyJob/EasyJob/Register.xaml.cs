using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyJob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private MediaFile px;
        public Register()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
          
            
            if (Email_Input.Text == null || Password_Input.Text == null || CPassword_Input.Text == null || name_Input.Text == null || Surname_Input.Text == null || Tel_Input.Text == null)
            {
                await DisplayAlert("Register", "กรอกข้อมูลให้ครบ", "Ok");
            }
            else if (Password_Input.Text != CPassword_Input.Text)
            {
                await DisplayAlert("Register", "กรุณากรอก Password ให้ตรงกัน", "Ok");
            }
            else
            {
                HttpClient client = new HttpClient();
                var url = "http://139.180.129.212/api/Member/Register";

                var content = new MultipartFormDataContent();
                var imageContent = new StreamContent(px.GetStream());
                StringContent name = new StringContent(name_Input.Text);
                StringContent surname = new StringContent(Surname_Input.Text);
                StringContent email = new StringContent(Email_Input.Text);
                StringContent tel = new StringContent(Tel_Input.Text);
                StringContent password = new StringContent(Password_Input.Text);
                content.Add(imageContent, "UploadedImage" , "image.jpg");
                content.Add(name, "name");
                content.Add(surname, "surname");
                content.Add(email, "email");
                content.Add(tel, "tel");
                content.Add(password, "password");

                using (HttpResponseMessage response = await client.PostAsync(url, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await DisplayAlert("Success", "สมัครสมาชิกสำเร็จ", "ตกลง");
                        App.Current.MainPage = new NavigationPage(new Login());
                    }
                    else
                    {
                        await DisplayAlert("Error", "สมัครสมาชิกไม่สำเร็จ ไฟล์รูปใหญ่เกินไป !", "ตกลง");
                    }
                }
            }
        }

        private void ToLogin(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Login());
        }

        async void Button_Clicked_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not Supported", "เครื่องของคุณไม่รองรับฟังชั่นนี้", "OK");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if(selectedImage == null)
            {
                await DisplayAlert("Error", "โปรดเลือกรูปก่อน", "OK");
                return;
            }

            px = selectedImageFile;

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
        }
    }
}
