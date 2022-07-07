using ApiPersonBook.Services;
using DataBaseLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiPersonBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDataRepository _dataRepository;
        private readonly IUserRepository _userRepository;
        public AuthController(IConfiguration config, IDataRepository dataRepository, IUserRepository userRepository)
        {
            _config = config;
            _dataRepository = dataRepository;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<String> GetAuth([FromBody] ApplicationUser value)
        {
            var validUser = await _userRepository.GetUser(value);
            if (validUser != null)
            {
                var encodedJwt = CreateToken(value);
                return encodedJwt;
            }
            else
            {
                return null;
            }
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<string> Register([FromBody] ApplicationUser value)
        {
            var validUser = await _userRepository.GetUser(value);

            if (validUser == null)
            {
                await _userRepository.CreateUser(value);
                var encodedJwt = CreateToken(value);
                return encodedJwt;
            }
            else
            {
                return BadRequest(new { errorText = "Такой пользователь уже есть в базе" }).ToString();
            }
        }

        [Authorize]
        [Route("createuser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser value)
        {
            var validUser = await _userRepository.GetUser(value);

            if (validUser == null)
            {
                await _userRepository.CreateUser(value);
                return Ok();
            }
            else
            {
                return BadRequest(new { errorText = "Такой пользователь уже есть в базе" });
            }
        }

        [Authorize]
        [Route("deleteuser")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] string value)
        {
            var validUser = await _userRepository.GetUserByName(value);

            if (validUser != null)
            {
                await _userRepository.DeleteUser(validUser.Id);
                await _dataRepository.DeleteAllUserData(value);
                return Ok();
            }
            else
            {
                return BadRequest(new { errorText = "Такого пользователя нет в базе" });
            }
        }


        /// <summary>
        /// Метод генерирует jwt-token
        /// </summary>
        /// <param name="value">ApplicationUser value</param>
        /// <returns>JWT-Token</returns>
        private string CreateToken(ApplicationUser value)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, value.Name),
                };
            ClaimsIdentity identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
