using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.LinkReader.Disc.Services
{
    public class DiscLinkReader : ILinkReader
    {
        private const string RootFolderName = "Downloads";
        private const string SourceFileFilter = "*.txt";
        private const string SourceLocationFolderName = "SRC";
        private const string PostProcessingFolderName = "SRC_POST";
        private const string FailedLinksFileName = "FailedLinks.txt";
        private string _failedLinksFile;
        private string _postProcessingDirectory;
        private string _sourceLocationDirectory;


        public void Init()
        {
            SetPath();
        }

        public async Task<ISet<string>> GetLinksAsync()
        {
            var links = new HashSet<string>();

            if (Directory.Exists(_sourceLocationDirectory))
            {
                var files = Directory.GetFiles(_sourceLocationDirectory, SourceFileFilter);

                foreach (var file in files)
                {
                    var lines = File.ReadAllLines(file);

                    foreach (var link in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        if (!links.Contains(link))
                        {
                            links.Add(link);
                        }
                    }

                    var fileName = Path.GetFileName(file);
                    var movedFile = Path.Combine(_postProcessingDirectory, fileName);
                    File.Move(file, movedFile);
                }
            }
            else
            {
                Directory.CreateDirectory(_sourceLocationDirectory);
            }

            return links;
        }

        public async Task SaveFailedLinksAsync(ISet<string> links)
        {
            if (!Directory.Exists(_postProcessingDirectory))
            {
                Directory.CreateDirectory(_postProcessingDirectory);
            }

            var stringBuilder = new StringBuilder();

            foreach (var link in links)
            {
                stringBuilder.Append(link);
            }

            using var streamWriter = File.AppendText(_failedLinksFile);
            await streamWriter.WriteAsync(stringBuilder.ToString());
        }

        private void SetPath()
        {
            var baseRootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            var rootPath = Path.Combine(baseRootPath, RootFolderName);
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            _sourceLocationDirectory = Path.Combine(rootPath, assemblyName, SourceLocationFolderName);
            _postProcessingDirectory = Path.Combine(rootPath, assemblyName, PostProcessingFolderName);
            _failedLinksFile = Path.Combine(rootPath, assemblyName, FailedLinksFileName);
        }
    }
}