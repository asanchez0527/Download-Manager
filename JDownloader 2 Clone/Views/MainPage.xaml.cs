using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JDownloader_2_Clone.ViewModels;
using JDownloader_2_Clone.UsefulMethods;

namespace JDownloader_2_Clone
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new DownloadViewModel();
        }

        public void MainPageLoaded(object sender, RoutedEventArgs e)
        {
        }

        public DownloadViewModel ViewModel { get; set; }

        private async void Help_OnlineHelp_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(@"http://jdownloader.org/knowledge/index?s=lng_en");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (!success)
            {
                UsefulMethods.UsefulMethods.ErrorMessage("Could not open URL. Please try again.");
            }
        }

        private async void Help_Changelog_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(@"https://svn.jdownloader.org/projects/jd/activity");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (!success)
            {
                UsefulMethods.UsefulMethods.ErrorMessage("Could not open URL. Please try again.");
            }
        }

        private async void Help_Contribute_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog donate = new ContentDialog
            {
                Title = "Donation",
                Content = "This application is the result of various months of work and to help keep " +
                "it free of charge, donations to the developper would be greatly valued.",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Continue",
                DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await donate.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Uri uri = new Uri(@"https://my.jdownloader.org/contribute/#/?ref=jdownloader");
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
                if (!success)
                {
                    UsefulMethods.UsefulMethods.ErrorMessage("Could not open URL. Please try again.");
                }
            }
        }

        private void File_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void AddDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string input = await UsefulMethods.UsefulMethods.InputTextDialogAsync("Download Collector");
            if (input.CompareTo("") == 0){}
            else
            {
                bool isUrl = Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (isUrl)
                {
                    bool LinkExists = await Downloader.UrlExists(new Uri(input));

                    if (LinkExists)
                    {
                        ViewModel.Downloads.Add(await Downloader.DownloadCreator(new Uri(input)));
                        //Downloader.DownloadStart(ViewModel.Downloads[ViewModel.Downloads.Count - 1]);
                    }
                    else
                    {
                        UsefulMethods.UsefulMethods.ErrorMessage("Url does not exist.");
                    }
                }
                else
                {
                    UsefulMethods.UsefulMethods.ErrorMessage("Please enter a valid URL.");
                }
            }
        }

        private void Settings_Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void DownloadsPageNavigator_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SettingsPageNavigator_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void DataGrid_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
