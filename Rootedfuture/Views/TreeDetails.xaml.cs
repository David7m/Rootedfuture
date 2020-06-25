using Plugin.Media;
using Plugin.Media.Abstractions;
using Rootedfuture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rootedfuture.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TreeDetails : ContentPage
    {

        private int activeTreeId = 0;
        private MediaFile capturedPhoto;

        private bool TaskRunning = false;

        private void RunCamera() {
            if (IsAlreadyBusy())
            {
                Navigation.PopModalAsync();
                TakePicture();
            }
        }
        public TreeDetails(TreeData treeData)
        {

            InitializeComponent();

            MessagingCenter.Subscribe<PhotoPreview>(this, "RetakePhoto", (sender) =>
            {
                RunCamera();
            });

            MessagingCenter.Subscribe<PhotoPreview>(this, "ReloadPhoto", (sender) =>
            {
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
            });

            MessagingCenter.Subscribe<DistanceNotice>(this, "DistanceAccepted", (sender) =>
            {
                 RunCamera();
               
            });

            ButtonShowGallery.Clicked += ButtonShowGallery_Clicked;
            ButtonScanAnotherTree.Clicked += ButtonScanAnotherTree_Clicked;
            ButtonTakePicture.Clicked += ButtonTakePicture_Clicked;
            BindingContext = treeData;
            activeTreeId = treeData.id;

           
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
                await Navigation.PushModalAsync(new PhotoPreview(capturedPhoto, activeTreeId));
            }
            TaskDone();
        }

        private async void ButtonTakePicture_Clicked(object sender, EventArgs e)
        {
            if (IsAlreadyBusy())
            {
                await Navigation.PushModalAsync(new DistanceNotice(true));
                TaskDone();
            }
        }

        private void ButtonScanAnotherTree_Clicked(object sender, EventArgs e)
        {

           MessagingCenter.Send<TreeDetails>(this, "ScanAnotherOne");
        }

        private async void ButtonShowGallery_Clicked(object sender, EventArgs e)
        {
            if (IsAlreadyBusy())
            {
                await Navigation.PushModalAsync(new Loader());
                await Navigation.PushAsync(new Gallery(activeTreeId));
                TaskDone();
            }
        }

        private bool IsAlreadyBusy() {
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

        private void TaskDone() {
            TaskRunning = false;
        }
    }
}