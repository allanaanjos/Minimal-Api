using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MinimalApi.Configure
{
    public class GerarToken
    {
        private readonly IConfiguration _configuration;

        public GerarToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userEmail, string userPerfil)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim("Email", userEmail),
                    new Claim("Perfil", userPerfil)
                },
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}