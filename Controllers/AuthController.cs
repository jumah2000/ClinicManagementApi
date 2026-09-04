using ClinicManagementAPI.DTOs.Requests.Auth;
using ClinicManagementAPI.DTOs.Responses.Auth;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<bool>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Registration successful. OTP has been sent.",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        VerifyOtpRequestDto request)
    {
        try
        {
            var result = await _authService.VerifyOtpAsync(request);

            return Ok(
                new ApiResponse<bool>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "OTP verified successfully.",
                    Data = result
                });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequestDto request)
    {
        try
        {
            var result = await _authService.ResendOtpAsync(request);

            return Ok(
                new ApiResponse<bool>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "OTP resent successfully.",
                    Data = result
                });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);

            return Ok(
                new ApiResponse<LoginResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Login successful.",
                    Data = result
                });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequestDto request)
    {
        try
        {
            var result =
                await _authService.ChangePasswordAsync(request);

            return Ok(
                new ApiResponse<bool>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Password changed successfully.",
                    Data = result
                });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
}