using System.Collections.Generic;
using System.Threading.Tasks;

namespace Y2Downloader.Common.Interfaces
{
    public interface ILinkReader
    {
        void Init();
        Task<ISet<string>> GetLinksAsync();
        Task SaveFailedLinksAsync(ISet<string> links);
    }
}