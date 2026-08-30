using ClinicManagementAPI.DTOs.Requests.Admin;
using ClinicManagementAPI.DTOs.Responses.Admin;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class AdminService(IAdminRepository adminRepository, IUserRepository userRepository, IAuditLogService auditLogService) : IAdminService
{
    private readonly IAdminRepository _adminRepository = adminRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public async Task<AdminResponseDto> CreateAsync(
        AdminCreateRequestDto request)
    {
        var user = await _userRepository.GetByUserIdAsync(request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.UserType != "Admin")
            throw new InvalidOperationException(
                "The user is not an admin.");

        var existingAdmin =
            await _adminRepository.GetByUserIdAsync(user.Id);

        if (existingAdmin != null)
            throw new InvalidOperationException(
                "Admin already exists.");

        var admin = new Admin
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        var createdAdmin =
            await _adminRepository.CreateAsync(admin);

        await _auditLogService.CreateAsync(
            "Create Admin",
            "Success",
            $"Admin {user.UserId} was created.",
            user.Id);

        return MapToResponse(createdAdmin);
    }

    public async Task<IEnumerable<AdminResponseDto>> GetAllAsync()
    {
        var admins = await _adminRepository.GetAllAsync();

        return admins.Select(MapToResponse);
    }

    public async Task<AdminResponseDto?> GetByUserIdAsync(
        string userId)
    {
        var user =
            await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            return null;

        var admin =
            await _adminRepository.GetByUserIdAsync(user.Id);

        if (admin == null)
            return null;

        return MapToResponse(admin);
    }

    public async Task<bool> UpdateAsync(
        int id,
        AdminUpdateRequestDto request)
    {
        var admin =
            await _adminRepository.GetByIdAsync(id);

        if (admin == null)
            return false;

        var user =
            await _userRepository.GetByIdAsync(admin.UserId);

        var userBusinessId =
            user?.UserId ?? "Unknown";

        admin.UpdatedAt = DateTime.UtcNow;

        await _adminRepository.UpdateAsync(admin);

        await _auditLogService.CreateAsync(
            "Update Admin",
            "Success",
            $"Admin {userBusinessId} was updated.",
            admin.UserId);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var admin =
            await _adminRepository.GetByIdAsync(id);

        if (admin == null)
            return false;

        var user =
            await _userRepository.GetByIdAsync(admin.UserId);

        var userBusinessId =
            user?.UserId ?? "Unknown";

        await _adminRepository.DeleteAsync(id);

        await _auditLogService.CreateAsync(
            "Deactivate Admin",
            "Success",
            $"Admin {userBusinessId} was deactivated.",
            admin.UserId);

        return true;
    }

    private static AdminResponseDto MapToResponse(
        Admin admin)
    {
        return new AdminResponseDto
        {
            UserId = admin.User?.UserId ?? string.Empty,
            FirstName = admin.User?.FirstName ?? string.Empty,
            LastName = admin.User?.LastName ?? string.Empty,
            Email = admin.User?.Email ?? string.Empty,
            CreatedAt = admin.CreatedAt,
            UpdatedAt = admin.UpdatedAt
        };
    }

}