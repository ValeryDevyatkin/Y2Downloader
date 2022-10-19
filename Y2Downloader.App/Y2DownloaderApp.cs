using System;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.App
{
    public class Y2DownloaderApp : IY2DownloaderApp
    {
        private readonly IY2Downloader _downloader;
        private readonly ILinkReader _linkReader;
        private readonly ILogger _logger;
        private readonly IAppSettings _settings;

        public Y2DownloaderApp(ILogger logger, ILinkReader linkReader, IY2Downloader downloader, IAppSettings settings)
        {
            _logger = logger;
            _linkReader = linkReader;
            _downloader = downloader;
            _settings = settings;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    var links = await _linkReader.GetLinksAsync();
                    var downloadResult = await _downloader.DownloadFromLinksAsync(links);

                    if (!downloadResult.IsSuccessful)
                    {
                        await _linkReader.SaveFailedLinksAsync(downloadResult.FailedLinks);
                    }

                    await Task.Delay(_settings.SourceLocationProcessDelay);
                }
                catch (Exception e)
                {
                    await _logger.LogErrorAsync(e);
                }
            }
        }

        public void Init()
        {
            _settings.Init();
            _logger.Init();
            _linkReader.Init();
            _downloader.Init();
        }
    }
}