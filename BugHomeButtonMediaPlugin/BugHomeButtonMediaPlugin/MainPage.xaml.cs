using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BugHomeButtonMediaPlugin
{
    public class StartLongRunningTaskMessage { }

    public class StopLongRunningTaskMessage { }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            if (file != null)
                await DisplayAlert("test", file.Path, "OK");
            file?.Dispose();
        }

        private void ServiceStart_OnClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(new StartLongRunningTaskMessage(), "StartLongRunningTaskMessage");
        }

        private void ServiceStop_OnClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(new StopLongRunningTaskMessage(), "StopLongRunningTaskMessage");
        }
    }
}
