using System.Threading.Tasks;

namespace Bitly.Core.Services.Interfaces
{
    public interface IRedirectService
    {
        public Task<string> RedirectByKey(string key);
    }
}
