using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Patient_Monitoring.Data;
using Patient_Monitoring.Enums;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Patient_Monitoring.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        //private readonly string _clientId;
        private readonly int _accessTokenExpiry;
        private readonly int _refreshTokenExpiry;
        private readonly IRefreshTokenRepository _repository;
        public JwtService(IConfiguration config, IRefreshTokenRepository repository)
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
        public string GenerateAccessToken(dynamic userInfo, string role, out string jwtId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyBytes = Convert.FromBase64String(_key);
            var key = new SymmetricSecurityKey(keyBytes);

            jwtId = Guid.NewGuid().ToString();

            string userId = (role == "Patient") ? userInfo.PatientId.ToString() : userInfo.DoctorId.ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
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
        public async Task<string> GenerateRefreshToken(string jwtId, int userId, UserType userType)
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


        //public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken)
        //{
        //    var existingToken = await _repository.FetchRefreshToken(refreshToken);
           
        //    if (existingToken == null || existingToken.IsRevoked || existingToken.Expires <= DateTime.UtcNow)
        //        return null; 
                             
        //    existingToken.IsRevoked = true;
        //    existingToken.RevokedAt = DateTime.UtcNow;

        //    var role = existingToken.UserType.ToString();
        //    string accessToken;
          

        //    if (role == UserType.Patient.ToString())
        //    {
        //        accessToken = GenerateAccessToken(user, role, )
        //    }

        //    var accessToken = GenerateAccessToken(user, role, out string newJwtId);
            
        //    var newRefreshToken = GenerateRefreshToken(newJwtId, );
        //    // Store the new refresh token in the database
        //    _dbContext.RefreshTokens.Add(newRefreshToken);
        //    await _dbContext.SaveChangesAsync();
        //    // Read access token expiration duration from config or default to 15 minutes
        //    var accessTokenExpiryMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var val) ? val : 15;
        //    // Return the new tokens and expiry info
        //    return new AuthResponseDTO
        //    {
        //        AccessToken = accessToken,
        //        RefreshToken = newRefreshToken.Token,
        //        AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes)
        //    };
        //}
    } 
}
