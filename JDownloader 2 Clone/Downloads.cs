using System;
using System.Net;
using System.Threading.Tasks;
using ByteSizeLib;
using System.Diagnostics;
using JDownloader_2_Clone.ViewModels;
using JDownloader_2_Clone.UsefulMethods;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.IO;
using Windows.Networking.BackgroundTransfer;
using Windows.Foundation;

namespace JDownloader_2_Clone
{
    public class Downloader
    {
        public async static Task<bool> UrlExists(Uri url){
            bool result = false;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.Timeout = 1200;
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse) await request.GetResponseAsync();
                result = true;
            }
            catch (WebException webException)
            {
                Debug.WriteLine(url + " doesn't exist: " + webException.Message);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public static async Task<Download> DownloadCreator(Uri url)
        {
            Download finalizedDownload = new Download
            {
                //set download URL
                DownloadUrl = url,
                //get file size
                DownloadSize = await FileSize(url),
                //get download name
                DownloadName = await UsefulMethods.UsefulMethods.InputTextDialogAsync("Enter the name of the file: "),
                //get hoster
                Hoster = await HosterName(url),
                //specify saving location
                SaveTo = (String)ApplicationData.Current.LocalSettings.Values["DownloadDirectory"],
            };
            return finalizedDownload;
        }

        public static async void DownloadStart(Download x)
        {
            try
            {
                //StorageFolder destinationFolder = await StorageFolder.GetFolderFromPathAsync((String)ApplicationData.Current.LocalSettings.Values["DownloadDirectory"]);
                StorageFile destinationFile = await DownloadsFolder.CreateFileAsync(x.DownloadName);
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(x.DownloadUrl, destinationFile);
                await download.StartAsync();
                x.Status = "downloading";
                x.Speed = download.Progress;

            } catch (Exception ex)
            {
                
            }
            
        }

        private static async Task<ByteSize> FileSize(Uri url)
        {
            ByteSize fileSize;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;

            using (var response = (HttpWebResponse) await request.GetResponseAsync())
            {
                long len = response.ContentLength;
                fileSize = ByteSize.FromBytes(len);
            }
            return fileSize;
        }

        private static async Task<String> HosterName(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return response.ResponseUri.Host;
            }
        }

        private static async Task<String> TimeRemaining(Download download)
        {
            return "";
        }

    }
}
