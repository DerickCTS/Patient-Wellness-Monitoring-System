using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;

namespace Patient_Monitoring.Repositories.Implementations
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

        public async Task<RefreshToken?>FetchRefreshToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
