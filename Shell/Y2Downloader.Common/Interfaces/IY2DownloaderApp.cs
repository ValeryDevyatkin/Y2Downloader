using System.Threading.Tasks;

namespace Y2Downloader.Common.Interfaces
{
    public interface IY2DownloaderApp
    {
        Task RunAsync();
        void Init();
    }
}