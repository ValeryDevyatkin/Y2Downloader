using System.Collections.Generic;

namespace Y2Downloader.Common.Interfaces
{
    public interface IDownloadResult
    {
        bool IsSuccessful { get; }

        ISet<string> FailedLinks { get; }
    }
}