using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> FetchRefreshToken(string token);
    }
}
