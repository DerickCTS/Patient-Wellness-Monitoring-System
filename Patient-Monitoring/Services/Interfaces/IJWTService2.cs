using Patient_Monitoring.Enums;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IJWTService2
    {
        //string GenerateAccessToken(dynamic user, IList<string> roles, out string jwtId, Client client);

        string GenerateAccessToken(dynamic user, string role, out string jwtId);

        //RefreshToken GenerateRefreshToken(string ipAddress, string jwtId, Client client, int userId);
        Task<string> GenerateRefreshToken(string jwtId, string userId, UserType userType);
    }
}
