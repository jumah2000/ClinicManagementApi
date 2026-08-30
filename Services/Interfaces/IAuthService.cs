using ClinicManagementAPI.DTOs.Requests.Auth;
using ClinicManagementAPI.DTOs.Responses.Auth;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IAuthService
{
        Task<bool> RegisterAsync(RegisterRequestDto request);

        Task<bool> VerifyOtpAsync(VerifyOtpRequestDto request);

        Task<bool> ResendOtpAsync(ResendOtpRequestDto request);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request);
}