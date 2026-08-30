using ClinicManagementAPI.DTOs.Requests.Auth;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Registration successful. OTP has been sent.",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
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

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "OTP verified successfully.",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(
        ResendOtpRequestDto request)
    {
        try
        {
            var result = await _authService.ResendOtpAsync(request);

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "OTP resent successfully.",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Login successful.",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequestDto request)
    {
        try
        {
            var result = await _authService.ChangePasswordAsync(request);

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Password changed successfully.",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }
}