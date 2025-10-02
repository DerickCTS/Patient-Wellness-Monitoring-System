using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;

namespace Patient_Monitoring.Repository.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public RefreshTokenRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
