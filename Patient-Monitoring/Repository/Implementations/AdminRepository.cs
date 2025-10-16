using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using System;
namespace Patient_Monitoring.Repository.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly PatientMonitoringDbContext _context; // Assuming AppDbContext is your main EF Core context

        public AdminRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an Admin user by their email address.
        /// </summary>
        /// <param name="email">The email of the Admin.</param>
        /// <returns>The Admin object or null if not found.</returns>
        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (admin == null) 
                throw new InvalidOperationException($"Admin with email '{email}' not found.");
            return admin;
        }

        /// <summary>
        /// Adds a new Admin to the database.
        /// </summary>
        /// <param name="admin">The Admin object to add.</param>
        /// <returns>The added Admin object.</returns>
        public async Task<Admin> AddAdminAsync(Admin admin)
        {
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }
    }
}
