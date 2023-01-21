using Ionta.OSC.App.Dtos;
using Ionta.OSC.App.Services.HashingPassword;
using Ionta.OSC.Domain;
using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.ToolKit.Store;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IOscStorage _storage;
        private readonly AuthOptions _authOptions;
        public AuthService(IOscStorage storage, AuthOptions authOptions)
        {
            _storage = storage;
            _authOptions = authOptions;
        }
        public async Task<JWTDto> Handle(string login, string password)
        {

            var user = _storage.Users.Single(u => u.Name.ToLower() == login.ToLower());
            var x = HashingPasswordService.Hash(password);
            if (user.Password != x) throw new Exception("Password Error");

            return new JWTDto() { JWT = GenerateJwt(user) };
        }

        private string GenerateJwt(User user)
        {
            var secretKey = _authOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Email",user.Email),
                new Claim("Sub", user.Id.ToString())
            };

            var token = new JwtSecurityToken(_authOptions.Issure,
                _authOptions.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_authOptions.TokenLifeTime),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
