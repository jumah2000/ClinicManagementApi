using ClinicManagementAPI.DTOs.Requests.Auth;
using ClinicManagementAPI.DTOs.Responses.Auth;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class AuthService(IUserRepository userRepository, IOtpRepository otpRepository, IEmailService emailService, ISmsService smsService,
    IAuditLogService auditLogService) : IAuthService
{
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IOtpRepository _otpRepository = otpRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly ISmsService _smsService = smsService;
        private readonly IAuditLogService _auditLogService = auditLogService;

        // =========================
        // REGISTER
        // =========================
        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                await _auditLogService.CreateAsync("Registration", "Failed",
                    "Registration failed because the email already exists.");

                return false;
            }

            var userId = await GenerateUserIdAsync(request.UserType);

            var user = new User
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),

                UserType = request.UserType,

                IsVerified = false,
                IsActive = false,

                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Generate 6-digit OTP
            var otpCode = GenerateOtp();

            var otp = new Otp
            {
                UserId = user.Id,
                Code = otpCode,
                IsUsed = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow
            };

            await _otpRepository.CreateAsync(otp);

            // Send OTP through email and SMS
            await _emailService.SendOtpAsync(user.Email, otpCode);

            await _smsService.SendOtpAsync(user.PhoneNumber, otpCode);

            await _auditLogService.CreateAsync("Registration", "Success",
                "User registered successfully.", user.Id);

            return true;
        }

        // =========================
        // VERIFY OTP
        // =========================
        public async Task<bool> VerifyOtpAsync(
            VerifyOtpRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            var otp = await _otpRepository.GetLatestUnusedOtpAsync(user.Id);

            if (otp == null)
            {
                await _auditLogService.CreateAsync("OTP Verification", "Failed",
                    "No unused OTP was found.", user.Id);

                return false;
            }

            // Check expiration
            if (otp.ExpiresAt <= DateTime.UtcNow)
            {
                await _auditLogService.CreateAsync("OTP Expiration", "Failed",
                    "OTP has expired.", user.Id);

                return false;
            }

            // Check OTP
            if (otp.Code != request.Otp)
            {
                await _auditLogService.CreateAsync("OTP Verification", "Failed",
                    "Incorrect OTP.", user.Id);

                return false;
            }

            // Mark OTP as used
            otp.IsUsed = true;
            otp.VerifiedAt = DateTime.UtcNow;

            await _otpRepository.UpdateAsync(otp);

            // Activate and verify user
            user.IsVerified = true;
            user.IsActive = true;
            user.VerifiedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            await _auditLogService.CreateAsync("OTP Verification", "Success",
                "OTP verified successfully.", user.Id);

            return true;
        }

        // =========================
        // RESEND OTP
        // =========================
        public async Task<bool> ResendOtpAsync(
            ResendOtpRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            // Invalidate previous unused OTPs
            await _otpRepository.InvalidatePreviousOtpsAsync(user.Id);

            // Generate new OTP
            var otpCode = GenerateOtp();

            var otp = new Otp
            {
                UserId = user.Id,
                Code = otpCode,
                IsUsed = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow
            };

            await _otpRepository.CreateAsync(otp);

            // Send new OTP
            await _emailService.SendOtpAsync(
                user.Email,
                otpCode);

            await _smsService.SendOtpAsync(
                user.PhoneNumber,
                otpCode);

            await _auditLogService.CreateAsync(
                "Resend OTP",
                "Success",
                "A new OTP was generated and sent.",
                user.Id);

            return true;
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            if (!user.IsVerified)
            {
                throw new UnauthorizedAccessException(
                    "Please verify your account before logging in.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Your account is inactive.");
            }

            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
            {
                await _auditLogService.CreateAsync(
                    "Login",
                    "Failed",
                    "Invalid password.",
                    user.Id);

                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            await _auditLogService.CreateAsync(
                "Login",
                "Success",
                "User logged in successfully.",
                user.Id);

            return new LoginResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType,
                LastLoginAt = user.LastLoginAt
            };
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        public async Task<bool> ChangePasswordAsync(
            ChangePasswordRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email);

            if (user == null)
            {
                return false;
            }

            var currentPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.CurrentPassword,
                    user.PasswordHash);

            if (!currentPasswordValid)
            {
                await _auditLogService.CreateAsync(
                    "Change Password",
                    "Failed",
                    "Current password is incorrect.",
                    user.Id);

                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            await _auditLogService.CreateAsync(
                "Change Password",
                "Success",
                "Password changed successfully.",
                user.Id);

            return true;
        }

        // =========================
        // GENERATE USER ID
        // =========================
        private async Task<string> GenerateUserIdAsync(
            string userType)
        {
            var normalizedUserType = userType.Trim().ToLower();

            var prefix = normalizedUserType switch
            {
                "patient" => "PAT",
                "doctor" => "DOC",
                "receptionist" => "REC",
                "admin" => "ADM",

                _ => throw new ArgumentException(
                    "Invalid user type.")
            };

            var latestUser =
                await _userRepository.GetLatestUserByTypeAsync(
                    normalizedUserType);

            var nextNumber = 1;

            if (latestUser != null &&
                !string.IsNullOrWhiteSpace(latestUser.UserId))
            {
                var parts = latestUser.UserId.Split('-');

                if (parts.Length == 3 &&
                    int.TryParse(parts[2], out var lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}-{DateTime.UtcNow.Year}-{nextNumber:D6}";
        }

        // =========================
        // GENERATE OTP
        // =========================
        private static string GenerateOtp()
        {
            return Random.Shared
                .Next(100000, 1000000)
                .ToString();
        }
    }
