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

namespace JDownloader_2_Clone
{

    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            Loaded += SettingsPageLoaded;
        }

        private void SettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;   
            if (AppSettings.Values.ContainsKey("DownloadDirectory"))
            {
                CurrentDirectory.Text = (String)AppSettings.Values["DownloadDirectory"];
            }

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
            if (input.CompareTo("") == 0) { }
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

        private void SettingsPageNavigator_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void DownloadsPageNavigator_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void DownloadFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker downloadPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };

            downloadPicker.FileTypeFilter.Add("*");

            StorageFolder directory = await downloadPicker.PickSingleFolderAsync();
            ApplicationData.Current.LocalSettings.Values["DownloadDirectory"] = directory.Path;
            CurrentDirectory.Text = (String)ApplicationData.Current.LocalSettings.Values["DownloadDirectory"];
        }

    }
}
