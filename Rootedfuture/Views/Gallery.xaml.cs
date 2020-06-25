using Rootedfuture.Models;
using Rootedfuture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rootedfuture.Views
{
   
    public partial class Gallery : ContentPage
    {
        private GalleryPhotoModel GalleryPhotoModel = new GalleryPhotoModel();
        public Gallery(int Id)
        {
            InitializeComponent();
            ButtonBackToInfoTop.Clicked+= ButtonBackToInfoBtm_Clicked;
            ButtonBackToInfoBtm.Clicked += ButtonBackToInfoBtm_Clicked;
            BindingContext = GalleryPhotoModel;
            GalleryPhotoModel.GalleryDataLoaded = false;
            imagesList.ItemTapped += ImagesList_ItemTapped;

            LoadTreeGallery(Id);

        }

        private void ImagesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            return;
        }

        private void ButtonBackToInfoBtm_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            GalleryPhotoModel.GalleryDataLoaded = false;
        }

        private async void LoadTreeGallery(int Id)
        {
            var resultData = await ApiService.GetTreeGallery(Id);
            if (resultData != null)
            {
                if (!resultData.Any()) {
                    GalleryPhotoModel.GalleryIsEmpty = true;
                }
                else
                {
                    GalleryPhotoModel.GalleryDataLoaded = true;
                    GalleryPhotoModel.GalleryIsEmpty = false;
                    GalleryPhotoModel.GalleryPhotoList = resultData;
                }
               


               
                await Navigation.PopModalAsync();
            }
        }
    }
}