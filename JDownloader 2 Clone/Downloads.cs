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
            //initialize boolean to false
            bool result = false;

            //create a webrequest to the specified URL to retrieve the headers, then time out
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.Timeout = 1200;
            //initialize response variable
            HttpWebResponse response = null;

            try
            {
                //try to get a webresponse from the url, then set boolean to true
                response = (HttpWebResponse) await request.GetResponseAsync();
                result = true;
            }
            catch (WebException webException)
            {
                //print out error to debugger
                Debug.WriteLine(url + " doesn't exist: " + webException.Message);
            }
            finally
            {
                if (response != null)
                {
                    //close response if its not null, otherwise don't worry about it
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
                DownloadName = (await UsefulMethods.UsefulMethods.InputTextDialogAsync("Enter the name of the file: ")) + (await FileExtension(url)),

                //get hoster
                Hoster = await HosterName(url),

                //specify saving location
                SaveTo = (String)ApplicationData.Current.LocalSettings.Values["DownloadDirectory"],
            };
            return finalizedDownload;
        }

        //initialize global variables for download manager
        DownloadOperation downloadOperation;
        CancellationTokenSource cancellationToken;
        BackgroundDownloader downloader = new BackgroundDownloader();

        //downloads a file given the Download file
        public async Task<StorageFile> DownloadFileAsync(Download download)
        {
            //initiate a blank file
            StorageFile file = null;
            //ensure that provided Download is not empty
            if (download != null)
            {
                //create file in downloads folder, ensure file renames if there's a duplicate
                file = await DownloadsFolder.CreateFileAsync(download.DownloadName, CreationCollisionOption.GenerateUniqueName);
                //create download operation
                downloadOperation = downloader.CreateDownload(download.DownloadUrl, file);
                //initiate cancellation token
                cancellationToken = new CancellationTokenSource();

                try
                {
                    //attempt to download file
                    download.Status = "downloading";
                    await downloadOperation.StartAsync().AsTask(cancellationToken.Token);
                    download.Status = "complete";
                } catch (TaskCanceledException)
                {
                    //if download fails delete file and release downloadOperation
                    await downloadOperation.ResultFile.DeleteAsync();
                    downloadOperation = null;
                    download.Status = "cancelled";
                }
            }
            //if download is successful, return the downloaded file.
            return file;
        }

        //cancels a download operation
        public void CancelDownload()
        {
            cancellationToken.Cancel();
            cancellationToken.Dispose();
        }

        //pauses a download operation
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

        //resume a download operation
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

        //download progress reporter, currently not implemented.
        private static void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //return the total size of the file being download in the appropriate units
        private static async Task<ByteSize> FileSize(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;

            using (var response = (HttpWebResponse) await request.GetResponseAsync())
            {
                return ByteSize.FromBytes(response.ContentLength);
            }
        }

        //retrieve the name of the download hoster.
        private static async Task<String> HosterName(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return response.ResponseUri.Host;
            }
        }

        //retrieve the file type
        private static async Task<String> FileExtension(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            {
                return response.ContentType;
            }
        }

    }
}
