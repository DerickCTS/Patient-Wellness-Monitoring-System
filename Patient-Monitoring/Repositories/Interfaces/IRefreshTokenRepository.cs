using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
    }
}
