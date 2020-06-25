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
    public partial class DistanceNotice : ContentPage
    {


        private bool sendToMain = true;
        public DistanceNotice(bool sendType)
        {
            InitializeComponent();
            sendToMain = sendType;
            TakePictureButton.Clicked += TakePictureButton_Clicked;
        }

        private void TakePictureButton_Clicked(object sender, EventArgs e)
        {
            if (sendToMain)
            {
                MessagingCenter.Send<DistanceNotice>(this, "DistanceAccepted");
            }
            else
            {
                MessagingCenter.Send<DistanceNotice>(this, "DistanceAcceptedNotPlanted");
            }
        }


  
    }
}