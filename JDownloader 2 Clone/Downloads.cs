using System;
using System.Net;
using System.Threading.Tasks;
using ByteSizeLib;
using System.Diagnostics;
using JDownloader_2_Clone.ViewModels;
using JDownloader_2_Clone.UsefulMethods;
using Windows.UI.Xaml.Controls;

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
            //Get File Size
            Download finalizedDownload = new Download
            {
                DownloadSize = await FileSize(url),
                DownloadName = await UsefulMethods.UsefulMethods.InputTextDialogAsync("Enter the name of the file: "),
                Hoster = await HosterName(url)
            };
            return finalizedDownload;
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
