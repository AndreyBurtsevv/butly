using Bitly.Core.Data;
using Bitly.Core.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly.Core.Services.Implementations
{
    public class RedirectService : IRedirectService
    {
        private readonly AppDbContext _context;

        public RedirectService(AppDbContext context)
        {
            _context = context;
        }

        public Task<string> RedirectByKey(string key)
        {
            var url = _context.Urls.FirstOrDefault(x => x.Key == key);
            if (url == null)
                throw new Exception("Url not found.");
            return Task.FromResult(url.FullUrl);
        }
    }
}
