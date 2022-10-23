using System;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.Services
{
    internal class ConsoleLogger : IClientLogger
    {
        public void LogError(Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine();
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}