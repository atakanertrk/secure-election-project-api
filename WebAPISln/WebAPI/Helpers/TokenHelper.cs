using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class TokenHelper
    {
        IConfiguration _config;
        public TokenHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJSONWebToken(int userId, bool isAdmin)
        {
            string whichPrivateKey;
            if (isAdmin)
            {
                whichPrivateKey = "AdminKey";
            }
            else
            {
                whichPrivateKey = "VoterKey";
            }
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config[$"Jwt:{whichPrivateKey}"]));
            // with securityKey, we create secure tokens which no one can generate without knowing the "Key"
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,userId.ToString()),
                // Guid generates unique string
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}
