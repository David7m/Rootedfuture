using Rootedfuture.Models;
using Rootedfuture.Services;
using Rootedfuture.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Rootedfuture
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {

        ZXingScannerPage scanPage;

        private ErrorModel errorModel;
        private bool TaskRunning = false;

        private async void RunQrScanningTask() {
            if (IsAlreadyBusy())
            {
                await runQrScanner();
                TaskDone();
            }
        }
        public ScanPage()
        {
            errorModel = new ErrorModel();
            InitializeComponent();
            ButtonScanQrCode.Clicked += ButtonScanQrCode_Clicked;
            BindingContext = errorModel;
            errorModel.Status = "Please scan QR code";

            MessagingCenter.Subscribe<TreeDetails>(this, "ScanAnotherOne", (sender) =>
            {
                Navigation.PopAsync();
                RunQrScanningTask();
            });
        }


        private void ButtonScanQrCode_Clicked(object sender, EventArgs e)
        {
            RunQrScanningTask();
        }

        private async Task runQrScanner()
        {
            List<BarcodeFormat> formats = new List<BarcodeFormat>();
            formats.Add(BarcodeFormat.QR_CODE);

            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = true,
                TryHarder = true,
                PossibleFormats = formats
            };


            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                    await Navigation.PushModalAsync(new Loader());
                    TreeData treeData = await ApiService.GetTreeInfoAsync(result.Text);

                    if (treeData != null && !treeData.error)
                    {
                        //for some reson here you have to make this lines in MainThread , you did it above but the below working on Task so to update UI you should implment 
                        //in UI MainThread
                        if (treeData.treeNotSold)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                errorModel.Status = "Please scan QR code";
                                await Navigation.PushAsync(new NotSoldTreeView(treeData));
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                errorModel.Status = "Please scan QR code";
                                await Navigation.PushAsync(new TreeDetails(treeData));
                            });
                            //Will work ^_^
                        }

                        await Navigation.PopModalAsync();

                    }
                    else
                    {
                        await Navigation.PopModalAsync();
                        errorModel.Status = "Invalid code, please scan an other one";
                    }
                });
            };
            await Navigation.PushModalAsync(scanPage);
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
