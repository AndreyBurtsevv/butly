using AutoMapper;
using Bitly.Core.Data;
using Bitly.Core.Data.Entities;
using Bitly.Core.Dto;
using Bitly.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bitly.Core.Services.Implementations
{
    public class UrlService : IUrlService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public UrlService(AppDbContext dbContext, IHttpContextAccessor accessor, IMapper mapper)
        {
            _context = dbContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public Task<UrlDto> Create(CreateUrlDto model)
        {
            var newUrl = new Url();
            newUrl.FullUrl = model.FullUrl;
            newUrl.User = GetCurrentUser();
            newUrl.Key = RandomString();
            _context.Urls.Add(newUrl);
            _context.SaveChanges();
            return Task.FromResult(_mapper.Map<UrlDto>(newUrl));
        }

        private string RandomString()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var key = new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            while (_context.Urls.Any(x => x.Key == key))
                key = new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return key;
        }

        public void Delete(string key)
        {
            var url = _context.Urls.FirstOrDefault(x => x.Key == key);
            var user = GetCurrentUser();

            if (url.UserId != user.Id)
                throw new Exception("Claim corrupted.");

            _context.Urls.Remove(url);
            _context.SaveChanges();
        }

        public Task<IList<UrlDto>> GetAll()
        {
            var user = GetCurrentUser();
            var data = _context.Urls.Where(x => x.UserId == user.Id).ToList();
            return Task.FromResult(_mapper.Map<IList<UrlDto>>(data));
        }

        public Task<UrlDto> GetById(int id)
        {
            var user = GetCurrentUser();
            var data = _context.Urls.FirstOrDefault(x => x.Id == id);

            if (data == null || data.UserId != user.Id)
                throw new Exception("Model not found");

            return Task.FromResult(_mapper.Map<UrlDto>(data));
        }

        private User GetCurrentUser()
        {
            var claimId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var successfully = int.TryParse(claimId, out var id);
            if (!successfully)
                throw new Exception("Claim corrupted.");
            return _context.Users.SingleOrDefault(x => x.Id == id);
        }
    }
}
