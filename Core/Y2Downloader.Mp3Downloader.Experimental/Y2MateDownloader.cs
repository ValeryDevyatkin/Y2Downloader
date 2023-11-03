﻿namespace Y2Downloader.Mp3Downloader.Experimental;

using System.Reflection;

using Common;
using Common.Helpers;
using Common.Interfaces;

using MediaToolkit;
using MediaToolkit.Model;

using VideoLibrary;

public class Y2MateDownloader : IY2Downloader
{
    private const string AudioFileDownloadFolderName = "Download";

    private const string AudioFileExtension = "mp3";

    private const string RootFolderName = "Downloads";

    private readonly IClientLogger _clientLogger;

    private readonly ILogger _logger;

    private string? _downloadPath;

    public Y2MateDownloader(ILogger logger, IClientLogger clientLogger)
    {
        _logger = logger;
        _clientLogger = clientLogger;
    }

    public async Task<IDownloadResult> DownloadFromLinksAsync(ISet<string> links)
    {
        var result = new DownloadResult { IsSuccessful = true };
        var downloadPath = ExceptionBuilder.GetNotEmptyStringOrThrowException(_downloadPath);
        var fileNameSet = new HashSet<string>();
        var linkNumber = 1;

        foreach (var link in links)
        {
            try
            {
                var match = RegexPool.FileId().Match(link);
                string? videoId = null;

                if (match is { Success: true, Groups.Count: 4 })
                {
                    videoId = match.Groups[2].Value;

                    if (string.IsNullOrEmpty(videoId))
                    {
                        videoId = match.Groups[3].Value;
                    }
                }

                if (string.IsNullOrWhiteSpace(videoId))
                {
                    throw new Exception($"Video ID was not found.\r\nSource link: [{link}].");
                }

                var youtube = YouTube.Default;
                var video = await youtube.GetVideoAsync(link);

                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }

                var fileNameWithNoSpecSymbols = RegexPool.SpecSymbol().Replace(video.Title, "");

                if (fileNameSet.Contains(fileNameWithNoSpecSymbols))
                {
                    _clientLogger.LogInfo($"File duplicate was found for link: {linkNumber} - [{link}].");
                }
                else
                {
                    var saveFilePath = Path.Combine(downloadPath, fileNameWithNoSpecSymbols);
                    await File.WriteAllBytesAsync(saveFilePath, await video.GetBytesAsync());

                    var inputFile = new MediaFile { Filename = saveFilePath };
                    var outputFile = new MediaFile { Filename = $"{saveFilePath}.{AudioFileExtension}" };

                    using var engine = new Engine();
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);

                    fileNameSet.Add(fileNameWithNoSpecSymbols);

                    _clientLogger.LogInfo($"Link downloaded: {linkNumber} - [{link}].");
                }
            }
            catch (Exception e)
            {
                result.IsSuccessful = false;
                result.FailedLinks.Add(link);

                _clientLogger.LogError(link, e);
                await _logger.LogErrorAsync(link, e);
            }

            linkNumber++;
        }

        result.DownloadedFileCount = fileNameSet.Count;

        return result;
    }

    public void Init()
    {
        SetPath();
    }

    private void SetPath()
    {
        var rootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Path.GetDirectoryName));

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Assembly.GetEntryAssembly));

        _downloadPath = Path.Combine(rootPath, RootFolderName, assemblyName, AudioFileDownloadFolderName);
    }
}