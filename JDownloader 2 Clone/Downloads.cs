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
using System.Threading;

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

        DownloadOperation downloadOperation;
        CancellationTokenSource cancellationToken;
        BackgroundDownloader downloader = new BackgroundDownloader();

        public async Task<StorageFile> DownloadFileAsync(Download download, StorageFolder DownloadLocation)
        {
            StorageFile file = null;
            if (DownloadLocation != null)
            {
                file = await DownloadLocation.CreateFileAsync(download.DownloadName, CreationCollisionOption.GenerateUniqueName);
                downloadOperation = downloader.CreateDownload(download.DownloadUrl, file);
                cancellationToken = new CancellationTokenSource();

                try
                {
                    download.Status = "downloading";
                    await downloadOperation.StartAsync().AsTask(cancellationToken.Token);
                    download.Status = "complete";
                } catch (TaskCanceledException)
                {
                    await downloadOperation.ResultFile.DeleteAsync();
                    downloadOperation = null;
                    download.Status = "cancelled";
                }
            }
            return file;
        }

        public void CancelDownload()
        {
            cancellationToken.Cancel();
            cancellationToken.Dispose();
        }

        public void PauseDownload(Download download)
        {
            download.Status = "paused";
            try
            {
                downloadOperation.Pause();
            } catch (InvalidOperationException)
            {
                UsefulMethods.UsefulMethods.ErrorMessage("Couldn't pause the download.");
            }
        }

        public void ResumeDownload(Download download)
        {
            download.Status = "downloading";
            try
            {
                downloadOperation.Resume();
            } catch (InvalidOperationException)
            {
                UsefulMethods.UsefulMethods.ErrorMessage("Couldn't resume the download.");
            }
        }

        private static void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
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

    }
}
