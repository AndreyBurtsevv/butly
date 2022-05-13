using Bitly.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitly.Core.Services.Interfaces
{
    public interface IUrlService
    {
        public Task<UrlDto> GetById(int id);
        public Task<IList<UrlDto>> GetAll();
        public Task<UrlDto> Create(CreateUrlDto model);
        public void Delete(string key);
    }
}
