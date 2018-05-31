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

        public DownloadViewModel ViewModel { get; set; }

        private async void Help_OnlineHelp_Click(object sender, RoutedEventArgs e)
        {
            String UriToLaunch = @"http://jdownloader.org/knowledge/index?s=lng_en";
            var uri = new Uri(UriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (!success)
            {
                ContentDialog UriLaunchError = new ContentDialog
                {
                    Title = "Error",
                    Content = "Could not open url. Try again.",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult errorResult = await UriLaunchError.ShowAsync();
            }
        }

        private async void Help_Changelog_Click(object sender, RoutedEventArgs e)
        {
            String UriToLaunch = @"https://svn.jdownloader.org/projects/jd/activity";
            var uri = new Uri(UriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (!success)
            {
                ContentDialog UriLaunchError = new ContentDialog
                {
                    Title = "Error",
                    Content = "Could not open url. Try again.",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult errorResult = await UriLaunchError.ShowAsync();
            }
        }

        private async void Help_Contribute_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog donate = new ContentDialog
            {
                Title = "The JDownloader project needs your help!",
                Content = "If you are a satisfied user of JDownloader, please think about contributing to this project. JDownloader is the result of daily hard work since more than 8 years. We need your help to keep it free of charge, free of advertising, free of installer bundles and to improve JDownloader even more. Moreover, donating is a good way to tell us what modules we should focus our work on.",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Continue",
                DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await donate.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                String UriToLaunch = @"https://my.jdownloader.org/contribute/#/?ref=jdownloader";
                var uri = new Uri(UriToLaunch);
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
                if (!success)
                {
                    ContentDialog UriLaunchError = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Could not open url. Try again.",
                        CloseButtonText = "Ok"
                    };

                    ContentDialogResult errorResult = await UriLaunchError.ShowAsync();
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
            if (input.CompareTo("") == 0)
            {
            }
            else
            {
                bool isUrl = Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (isUrl)
                {
                    bool LinkExists = await Downloader.UrlExists(new Uri(input));

                    if (LinkExists)
                    {
                        ViewModel.Downloads.Add(await Downloader.DownloadCreator(new Uri(input)));
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
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SettingsPageNavigator_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
    }
}
