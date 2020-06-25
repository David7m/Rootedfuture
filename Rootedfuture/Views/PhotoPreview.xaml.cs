using Plugin.Media.Abstractions;
using Rootedfuture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rootedfuture.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPreview : ContentPage
    {
        public MediaFile PhotoPrev;
        public int TreeId = 0;
        private bool TaskRunning = false;
        private int treeReserve;
        public PhotoPreview(MediaFile photoPreview, int treeId, int reserve = 0)
        {
            InitializeComponent();
            PhotoPrev = photoPreview;
            takenPhoto.Source = ImageSource.FromStream(() => {
                var stream = photoPreview.GetStream();
                return stream;
            });
            TreeId = treeId;
            treeReserve = reserve;
            ButtonRetakePhoto.Clicked += ButtonRetakePhoto_Clicked;
            ButtonAcceptPhoto.Clicked += ButtonAcceptPhoto_Clicked;
        }

        private async void ButtonAcceptPhoto_Clicked(object sender, EventArgs e)
        {
            if (IsAlreadyBusy())
            {
                await Navigation.PushModalAsync(new Loader());
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(PhotoPrev.GetStream()), "\"imageFile\"", $"\"{PhotoPrev.Path}\"");
                content.Add(new StringContent(TreeId.ToString()), "treeId");
                content.Add(new StringContent(treeReserve.ToString()), "reserve");
                await ApiService.SendImageToServer(content);
                if (treeReserve == 1)
                {
                    MessagingCenter.Send<PhotoPreview>(this, "ReloadPhotoNotPlanted");
                }
                else
                {

                    MessagingCenter.Send<PhotoPreview>(this, "ReloadPhoto");
                }
                TaskDone();
            }
            
        }

        private void ButtonRetakePhoto_Clicked(object sender, EventArgs e)
        {
            if (treeReserve==1)
            {

                MessagingCenter.Send<PhotoPreview>(this, "RetakePhotoNotPlanted");
            }
            else
            {
                MessagingCenter.Send<PhotoPreview>(this, "RetakePhoto");
            }
        }

        private bool IsAlreadyBusy()
        {
            if (TaskRunning)
            {
                return false;
            }
            else
            {
                TaskRunning = true;
                return true;
            }
        }

        private void TaskDone()
        {
            TaskRunning = false;
        }
    }
}