using Bitly.Core.Data;
using Bitly.Core.Data.Entities;
using Bitly.Core.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bitly.Core.Services
{
    public class TokenProvider
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public TokenProvider(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public Task<Token> CreateTokensAsync(User user)
        {
            var lifetimeSec = _config.GetSection("Token").GetValue<int>("LifeTimeAccess");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Token:Secret"]);
            var claims = new Claim[]
                          {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                          };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(lifetimeSec),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            _context.UserTokens.Add(new Microsoft.AspNetCore.Identity.IdentityUserToken<int>()
            {
                UserId = user.Id,
                Value = accessToken,
                LoginProvider = "TokenProvider",
                Name = "access_token"
            });
            _context.SaveChanges();
            return Task.FromResult(new Token
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(lifetimeSec).ToUnixTimeSeconds(),
            });
        }
    }
}
