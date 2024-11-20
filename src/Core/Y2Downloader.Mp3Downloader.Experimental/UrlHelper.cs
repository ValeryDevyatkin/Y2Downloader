using Y2Downloader.Common;

namespace Y2Downloader.Mp3Downloader.Experimental;

internal static class UrlHelper
{
    public static string GetVideoId(string link)
    {
        var match = RegexPool.VideoId().Match(link);
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

        return videoId;
    }
}