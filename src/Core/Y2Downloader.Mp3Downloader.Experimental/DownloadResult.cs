﻿namespace Y2Downloader.Mp3Downloader.Experimental;

using Common.Interfaces;

public class DownloadResult : IDownloadResult
{
    public int DownloadedFileCount { get; set; }

    public ISet<string> FailedLinks { get; } = new HashSet<string>();

    public bool IsSuccessful { get; set; }
}