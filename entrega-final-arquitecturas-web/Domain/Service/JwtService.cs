using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using entrega_final_arquitecturas_web.Domain;
using entrega_final_arquitecturas_web.DAL.Models;

namespace entrega_final_arquitecturas_web.Domain.Service
{
    public class JwtService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateSalt()
        {
            Console.WriteLine("GenerateSalt ");

            byte[] saltBytes = new byte[16];
            Console.WriteLine("saltBytes ", saltBytes);
            using var rng = RandomNumberGenerator.Create();
            Console.WriteLine("rng", rng);
            rng.GetBytes(saltBytes);
            Console.WriteLine("saltBytes");
            return Convert.ToBase64String(saltBytes);
        }

        public string EncryptSHA256(string texto, string salt)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] combinedBytes = Encoding.UTF8.GetBytes(salt + texto);
            byte[] bytes = SHA256.HashData(combinedBytes);

            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public string GenerarJWT(User usuario)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public string GenerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
