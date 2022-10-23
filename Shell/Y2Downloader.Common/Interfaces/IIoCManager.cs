namespace Y2Downloader.Common.Interfaces
{
    public interface IIoCManager
    {
        void RegisterTypes();
        void RegisterSettings<TSettings>() where TSettings : IAppSettings;
        void RegisterClientLogger<TLogger>() where TLogger : IClientLogger;
        IY2DownloaderApp GetApp();
    }
}