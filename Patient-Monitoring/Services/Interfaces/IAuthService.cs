using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string message)> RegisterPatient(PatientRegisterDTO patient);

        Task<(bool success, string? message, string? token, string refreshToken)> Login(UserLoginDTO user);

        Task<(bool success, string message)> RegisterDoctor(DoctorRegisterDTO patient);
    }
}
