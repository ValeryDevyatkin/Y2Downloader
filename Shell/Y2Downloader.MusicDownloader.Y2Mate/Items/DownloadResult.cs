using System.Collections.Generic;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.MusicDownloader.Y2Mate.Items
{
    public class DownloadResult : IDownloadResult
    {
        public int DownloadedFileCount { get; set; }
        public bool IsSuccessful { get; set; }
        public ISet<string> FailedLinks { get; set; } = new HashSet<string>();
    }
}