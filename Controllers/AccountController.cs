using Bitly.Core.Data.Entities;
using Bitly.Core.Dto;
using Bitly.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenProvider _tokenProvider;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, TokenProvider tokenProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration registrator)
        {
            if (ModelState.IsValid)
            {
                User user = new() { UserName = registrator.UserName };

                if (registrator.Email != null)
                {
                    user.Email = registrator.Email;
                }

                IdentityResult response = await _userManager.CreateAsync(user, registrator.Password);

                if (response.Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest(response.Errors.ToList()[0].Code);
                }
            }
            return BadRequest("User data is not correct");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var response = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, true, false);

                if (response.Succeeded)
                {
                    var user = _userManager.Users.First(x => x.UserName == login.UserName);
                    return Ok(await _tokenProvider.CreateTokensAsync(user));
                }
                else
                {
                    return BadRequest("Authorization denied");
                }
            }
            return BadRequest("User data is not correct");
        }
    }
}
