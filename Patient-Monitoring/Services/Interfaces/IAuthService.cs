using Patient_Monitoring.DTOs.Authentication;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string message)> RegisterPatient(PatientRegisterDTO patient);

        Task<(bool success, string? message, string? token, string? refreshToken, int? id)> Login(UserLoginDTO user);

        Task<(bool success, string message)> RegisterDoctor(DoctorRegisterDTO patient);
    }
}
