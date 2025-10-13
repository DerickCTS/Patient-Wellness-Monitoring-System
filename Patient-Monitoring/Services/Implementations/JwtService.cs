using Microsoft.IdentityModel.Tokens;
using Patient_Monitoring.Data;
using Patient_Monitoring.Enums;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Patient_Monitoring.Services.Implementations
{
    public class JwtService2 : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        //private readonly string _clientId;
        private readonly int _accessTokenExpiry;
        private readonly int _refreshTokenExpiry;
        private readonly IRefreshTokenRepository _repository;
        public JwtService2(IConfiguration config, IRefreshTokenRepository repository)
        {
            _key = config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"); ;
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience"); ;
            _accessTokenExpiry = int.Parse(config["Jwt:AccessTokenExpiryMinutes"] ?? "60");
            _refreshTokenExpiry = int.Parse(config["Jwt:RefreshTokenExpiryDays"] ?? "7");
            _repository = repository;
            //_clientId = config["ClientId"] ?? throw new ArgumentNullException("ClientId");
        }

        #region Generate Access Token
        public string GenerateAccessToken(dynamic user, string role, out string jwtId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyBytes = Convert.FromBase64String("kGv/rT4iXb+ZcE7qFjJ8pL9sW0uYvN3xH6aV2dC5bO4=");
            var key = new SymmetricSecurityKey(keyBytes);

            jwtId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.PatientID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role)

                //// Custom claim specifying the client id (helps identify which client requested the token)
                //new Claim("client_id", client.ClientId)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiry),
                SigningCredentials = creds,
                Issuer = _issuer, 
                Audience = _audience,
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region Generate Refresh Token
        public async Task<string> GenerateRefreshToken(string jwtId, string userId, UserType userType)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            RefreshToken newRefreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                JwtId = jwtId,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpiry),
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                UserType = userType,
                IsRevoked = false,
                RevokedAt = null
            };

            await _repository.AddRefreshTokenAsync(newRefreshToken);

            return newRefreshToken.Token;
        }
        #endregion
    } 
}
