using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteSizeLib;
using Windows.ApplicationModel;

namespace JDownloader_2_Clone.ViewModels
{
    public class DownloadViewModel {
        private ObservableCollection<Download> downloads = new ObservableCollection<Download>();
        private Download defaultDownload = new Download();
        public Download DefaultDownload { get { return this.defaultDownload; } }
        public ObservableCollection<Download> Downloads { get { return this.downloads; } }

        public DownloadViewModel()
        {
            this.Downloads.Add(new Download()
            {
                DownloadName = "name",
                DownloadSize = ByteSize.FromBits(45411),
                Hoster = "hoster",
                Status = "status",
                Speed = "speed",
                ETA = "ETA",
                BytesLoaded = "5",
                SaveTo = "SaveTo"
            });

            this.Downloads.Add(new Download()
            {
                DownloadName = "name2",
                DownloadSize = ByteSize.FromBits(45411),
                Hoster = "hoster",
                Status = "status",
                Speed = "speed",
                ETA = "ETA",
                BytesLoaded = "5",
                SaveTo = "SaveTo"
            });
        }
    }
    
    public class Download{
        public string DownloadName { get; set; }
        public ByteSize DownloadSize { get; set; }
        public string Hoster { get; set; }
        public string Status { get; set; }
        public string Speed { get; set; }
        public string ETA { get; set; }
        public string BytesLoaded { get; set; }
        public string SaveTo { get; set; }

        public Download()
        {
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
