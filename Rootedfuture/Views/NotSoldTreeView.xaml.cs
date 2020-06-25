using Plugin.Media;
using Rootedfuture.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rootedfuture.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotSoldTreeView : ContentPage
    {

        private bool TaskRunning = false;
        private MediaFile capturedPhoto;
        private int activeTreeId;

        public NotSoldTreeView(TreeData treeData)
        {
            InitializeComponent();

            BindingContext = treeData;
            activeTreeId = treeData.id;
            Button_GoBackToScanPage.Clicked += Button_GoBackToScanPage_Clicked;
            ButtonTakePicture.Clicked += ButtonTakePicture_Clicked;

            MessagingCenter.Subscribe<DistanceNotice>(this, "DistanceAcceptedNotPlanted", (sender) =>
            {
                RunCamera();

            });


            MessagingCenter.Subscribe<PhotoPreview>(this, "RetakePhotoNotPlanted", (sender) =>
            {
                RunCamera();
            });

            MessagingCenter.Subscribe<PhotoPreview>(this, "ReloadPhotoNotPlanted", (sender) =>
            {
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
            });

        }

        private void RunCamera()
        {
            if (IsAlreadyBusy())
            {
                Navigation.PopModalAsync();
                TakePicture();
            }
        }


        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Camera", "Camera not available", "OK");
                return;
            }
            capturedPhoto = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        CompressionQuality = 75
                    }
             );
            if (capturedPhoto != null)
            {
                await Navigation.PushModalAsync(new PhotoPreview(capturedPhoto, activeTreeId, 1));
            }
            TaskDone();
        }


        private async void ButtonTakePicture_Clicked(object sender, EventArgs e)
        {
            if (IsAlreadyBusy())
            {
                await Navigation.PushModalAsync(new DistanceNotice(false));
                TaskDone();
            }
        }

        private void Button_GoBackToScanPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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