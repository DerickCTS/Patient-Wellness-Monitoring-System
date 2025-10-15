using System.Security.Claims;

namespace Patient_Monitoring.Services.Interface
{
    public interface IJwtService
    {
        string GenerateToken(string id, string email, string role);
        ClaimsPrincipal ValidateToken(string token);
    }
}
