using System.Collections.Generic;
using System.ComponentModel;

namespace Rootedfuture.Models
{
    public class GalleryPhotoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       List<GalleryItem> galleryItems = new List<GalleryItem>();


        private bool _galleryDataLoaded = false;

        public bool galleryIsEmpty = false;





        public bool GalleryIsEmpty
        {
            get => galleryIsEmpty;
            set
            {
                galleryIsEmpty = value;
                OnPropertyChanged(nameof(GalleryIsEmpty));
            }
        }

        public bool GalleryDataLoaded
        {
            get => _galleryDataLoaded;
            set
            {
                _galleryDataLoaded = value;
                OnPropertyChanged(nameof(GalleryDataLoaded));
            }
        }

       
        public List<GalleryItem> GalleryPhotoList
        {
            get => galleryItems;
            set
            {
                galleryItems = value;
                OnPropertyChanged(nameof(GalleryPhotoList));
            }
        }

        private void OnPropertyChanged(string name)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
