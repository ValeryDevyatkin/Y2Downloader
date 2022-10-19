using System;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.IoC.Unity;

namespace Y2Downloader
{
    internal class Program
    {
        private static void SetDefaultConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private static void SetErrorConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        private static async Task Main(string[] args)
        {
            try
            {
                SetDefaultConsoleColor();

                IIoCManager container = new UnityContainerManager();
                container.RegisterTypes();
                container.RegisterSettings<AppSettings>();

                var app = container.GetApp();
                app.Init();
                await container.GetApp().RunAsync();
            }
            catch (Exception e)
            {
                SetErrorConsoleColor();
                Console.WriteLine($"{e}\n{e.Message}\n{e.StackTrace}");
                SetDefaultConsoleColor();
            }

            Console.WriteLine("Press 'Enter' to exit.");
            Console.ReadLine();
        }
    }
}