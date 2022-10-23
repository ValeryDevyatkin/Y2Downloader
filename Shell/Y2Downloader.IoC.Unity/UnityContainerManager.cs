using Unity;
using Y2Downloader.App;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.LinkReader.Disc.Services;
using Y2Downloader.Logger.Disc.Services;
using Y2Downloader.MusicDownloader.Y2Mate.Services;

namespace Y2Downloader.IoC.Unity
{
    public class UnityContainerManager : IIoCManager
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public void RegisterTypes()
        {
            _container
               .RegisterSingleton<ILogger, DiscLogger>()
               .RegisterSingleton<ILinkReader, DiscLinkReader>()
               .RegisterSingleton<IY2Downloader, Y2MateDownloader>()
               .RegisterSingleton<IY2DownloaderApp, Y2DownloaderApp>()
                ;
        }

        public void RegisterSettings<TSettings>() where TSettings : IAppSettings
        {
            _container.RegisterSingleton<IAppSettings, TSettings>();
        }

        public void RegisterClientLogger<TLogger>() where TLogger : IClientLogger
        {
            _container.RegisterSingleton<IClientLogger, TLogger>();
        }

        public IY2DownloaderApp GetApp() => _container.Resolve<IY2DownloaderApp>();
    }
}