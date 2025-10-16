using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByEmailAsync(string email);
        Task<Admin> AddAdminAsync(Admin admin);
        // This is primarily for one-time seed data or initial setup, 
        // but it's good practice to have a registration method.
    }
}
