using Unity;
using Y2Downloader.App;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.Logger.Disc.Services;

namespace Y2Downloader.IoC.Unity
{
    public class IoCManager : IIoCManager
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public void RegisterTypes()
        {
            _container
               .RegisterSingleton<ILogger, DiscLogger>()
               .RegisterSingleton<IY2DownloadService, IY2DownloadService>()
               .RegisterSingleton<IY2DownloaderApp, Y2DownloaderApp>()
                ;
        }

        public void RegisterSettings<TSettings>() where TSettings : IAppSettings
        {
            _container.RegisterSingleton<IAppSettings, TSettings>();
        }

        public IY2DownloaderApp GetApp() => _container.Resolve<IY2DownloaderApp>();
    }
}