using JDownloader_2_Clone.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JDownloader_2_Clone
{

    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
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
                    ContentDialog error = new ContentDialog();
                    error.Content = "URL does not exist.";
                    error.Title = "Error";
                    error.IsSecondaryButtonEnabled = false;
                    error.PrimaryButtonText = "Ok";

                    ContentDialogResult errorResult = await error.ShowAsync();
                }
            }
            else
            {
                ContentDialog error = new ContentDialog();
                error.Content = "Please enter a valid URL.";
                error.Title = "Error";
                error.IsSecondaryButtonEnabled = false;
                error.PrimaryButtonText = "Ok";

                ContentDialogResult errorResult = await error.ShowAsync();
            }
        }

        private async void DownloadFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            FolderPicker downloadPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };

            downloadPicker.FileTypeFilter.Add("*");

            StorageFolder directory = await downloadPicker.PickSingleFolderAsync();

            if (localSettings.Values["DownloadDirectory"] == null)
            {
                localSettings.Values["DownloadDirectory"] = directory.Path;
            }

            CurrentDirectory.Text = localSettings.Values["DownloadDirectory"].ToString();
        }
    }
}
