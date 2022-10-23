using System;

namespace Y2Downloader.Common.Interfaces
{
    public interface IClientLogger
    {
        void LogError(Exception e);
        void LogInfo(string message);
    }
}