using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.MusicDownloader.Y2Mate.Items;
using Y2Sharp.Youtube;

namespace Y2Downloader.MusicDownloader.Y2Mate.Services
{
    public class Y2MateDownloader : IY2Downloader
    {
        private const string AudioFileDownloadFolderName = "Download";
        private const string RootFolderName = "Downloads";
        private const string AudioFileQuality = "320";
        private const string AudioFileExtension = "mp3";
        private const string SpecSymbolRegex = @"[^a-zA-Z0-9_. \(\)\[\]-]+";
        private static string _downloadPath;
        private readonly Regex _fileIdRegex = new Regex(@"^.*watch\?v=((.*)\&.*|(.*))$");
        private readonly ILogger _logger;

        public Y2MateDownloader(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<IDownloadResult> DownloadFromLinksAsync(ISet<string> links)
        {
            var result = new DownloadResult
            {
                IsSuccessful = true
            };

            foreach (var link in links)
            {
                try
                {
                    var match = _fileIdRegex.Match(link);
                    string videoId = null;

                    if (match.Success)
                    {
                        if (match.Groups.Count == 4)
                        {
                            videoId = match.Groups[2].Value;

                            if (string.IsNullOrEmpty(videoId))
                            {
                                videoId = match.Groups[3].Value;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(videoId))
                    {
                        await _logger.LogErrorAsync(
                            new Exception($"Video ID was not found.\r\nSource link: [{link}]."));

                        throw new NotImplementedException();
                    }

                    await Video.GetInfo(videoId);
                    var video = new Video();

                    if (!Directory.Exists(_downloadPath))
                    {
                        Directory.CreateDirectory(_downloadPath);
                    }

                    var fileNameWithNoSpecSymbols =
                        Regex.Replace(video.Title, SpecSymbolRegex, "", RegexOptions.Compiled);

                    await video.DownloadAsync(
                        Path.Combine(_downloadPath, $"{fileNameWithNoSpecSymbols}.{AudioFileExtension}"),
                        AudioFileExtension, AudioFileQuality);
                }
                catch (Exception e)
                {
                    result.IsSuccessful = false;
                    result.FailedLinks.Add(link);

                    await _logger.LogErrorAsync(e);
                }
            }

            return result;
        }

        public void Init()
        {
            SetPath();
        }

        private static void SetPath()
        {
            var rootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            _downloadPath = Path.Combine(rootPath, RootFolderName, assemblyName, AudioFileDownloadFolderName);
        }
    }
}