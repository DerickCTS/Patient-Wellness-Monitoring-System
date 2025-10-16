using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterPatient(PatientRegisterDTO patient);

        Task<(bool success, string? message)> Login(UserLoginDTO user);

        Task<bool> RegisterDoctor(DoctorRegisterDTO patient);

        Task<AdminResponseDTO> AdminLogin(AdminLoginDTO loginDto);
    }
}
