using System;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.App
{
    public class Y2DownloaderApp : IY2DownloaderApp
    {
        private readonly IClientLogger _clientLogger;
        private readonly IY2Downloader _downloader;
        private readonly ILinkReader _linkReader;
        private readonly ILogger _logger;
        private readonly IAppSettings _settings;

        public Y2DownloaderApp(
            ILogger logger,
            IClientLogger clientLogger,
            ILinkReader linkReader,
            IY2Downloader downloader,
            IAppSettings settings)
        {
            _logger = logger;
            _clientLogger = clientLogger;
            _linkReader = linkReader;
            _downloader = downloader;
            _settings = settings;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                _clientLogger.LogInfo("Cycle started.");

                try
                {
                    var links = await _linkReader.GetLinksAsync();

                    _clientLogger.LogInfo($"Link count: {links.Count}.");

                    var downloadResult = await _downloader.DownloadFromLinksAsync(links);

                    if (!downloadResult.IsSuccessful)
                    {
                        _clientLogger.LogInfo($"Failed link count: {downloadResult.FailedLinks.Count}.");

                        await _linkReader.SaveFailedLinksAsync(downloadResult.FailedLinks);
                    }
                }
                catch (Exception e)
                {
                    _clientLogger.LogError(e);
                    await _logger.LogErrorAsync(e);
                }

                _clientLogger.LogInfo("Cycle ended.");

                await Task.Delay(_settings.SourceLocationProcessDelay);
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