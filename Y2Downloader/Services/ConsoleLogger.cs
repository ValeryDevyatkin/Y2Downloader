using System;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.Services
{
    internal class ConsoleLogger : IClientLogger
    {
        public void LogError(string title, Exception e)
        {
            Console.WriteLine(title);
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