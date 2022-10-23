using System;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.IoC.Unity;
using Y2Downloader.Services;

namespace Y2Downloader
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                IIoCManager container = new UnityContainerManager();
                container.RegisterClientLogger<ConsoleLogger>();
                container.RegisterSettings<AppSettings>();
                container.RegisterTypes();

                var app = container.GetApp();
                app.Init();
                await container.GetApp().RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("(>-:[CRITICAL ERROR]:-<)");
                Console.WriteLine(e);
            }

            Console.WriteLine("Press 'Enter' to exit.");
            Console.ReadLine();
        }
    }
}