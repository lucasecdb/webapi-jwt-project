using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using JwtTestProject.Config;
using JwtTestProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtTestProject.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public AuthController(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel userLogin)
        {
            // authentication should go here and return an
            // Unauthorized() if the credentials aren't valid

            var identity = new ClaimsIdentity(
                new GenericIdentity(userLogin.Username, "Login"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userLogin.Username)
                }
            );

            var createdTime = DateTime.Now;
            var expirationTime = createdTime +
                                 TimeSpan.FromDays(_tokenConfigurations.Days);

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createdTime,
                Expires = expirationTime
            });

            var token = handler.WriteToken(securityToken);

            return Accepted(new
            {
                created = createdTime.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
            });
        }
    }
}