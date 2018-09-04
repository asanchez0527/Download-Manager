using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteSizeLib;
using Windows.ApplicationModel;
using Windows.Storage;

namespace JDownloader_2_Clone.ViewModels
{
    public class DownloadViewModel {
        private ObservableCollection<Download> downloads = new ObservableCollection<Download>();
        private Download defaultDownload = new Download();
        public Download DefaultDownload { get { return this.defaultDownload; } }
        public ObservableCollection<Download> Downloads { get { return this.downloads; } }
    }
    
    public class Download{
        public Uri DownloadUrl { get; set; }
        public String DownloadName { get; set; }
        public ByteSize DownloadSize { get; set; }
        public String Hoster { get; set; }
        public String Status { get; set; }
        public String Speed { get; set; }
        public String ETA { get; set; }
        public String BytesLoaded { get; set; }
        public String SaveTo { get; set; }

        public Download()
        {
            //random data to test display
            this.DownloadName = "Car";
            this.DownloadSize = ByteSize.FromBits(50000);
            this.Hoster = "Google Drive";
            this.Status = "Online";
            this.Speed = "5";
            this.ETA = "1 minute";
            this.BytesLoaded = "50 mb";
            this.SaveTo = "Downloads";
        }
    }
}
