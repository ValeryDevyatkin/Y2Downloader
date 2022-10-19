namespace Y2Downloader.Common.Interfaces
{
    public interface IIoCManager
    {
        void RegisterTypes();
        void RegisterSettings<TSettings>() where TSettings : IAppSettings;
        IY2DownloaderApp GetApp();
    }
}