using ClinicManagementAPI.DTOs.Requests.Receptionist;
using ClinicManagementAPI.DTOs.Responses.Receptionist;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class ReceptionistService(IReceptionistRepository receptionistRepository, IUserRepository userRepository, IAuditLogService auditLogService) : IReceptionistService
{
    private readonly IReceptionistRepository _receptionistRepository = receptionistRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public async Task<ReceptionistResponseDto> CreateAsync(ReceptionistCreateRequestDto request)
    {
        var user = await _userRepository.GetByUserIdAsync(request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.UserType != "Receptionist")
            throw new InvalidOperationException("The user is not a receptionist.");

        var existingReceptionist = await _receptionistRepository.GetByUserIdAsync(user.Id);

        if (existingReceptionist != null)
            throw new InvalidOperationException("Receptionist already exists.");

        var receptionist = new Receptionist
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        var createdReceptionist = await _receptionistRepository.CreateAsync(receptionist);

        await _auditLogService.CreateAsync(
            "Create Receptionist",
            "Success",
            $"Receptionist {user.UserId} was created.",
            user.Id);

        return MapToResponse(createdReceptionist);
    }

    public async Task<IEnumerable<ReceptionistResponseDto>> GetAllAsync()
    {
        var receptionists = await _receptionistRepository.GetAllAsync();
        return receptionists.Select(MapToResponse);
    }

    public async Task<ReceptionistResponseDto?> GetByUserIdAsync(string userId)
    {
        var user = await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            return null;

        var receptionist = await _receptionistRepository.GetByUserIdAsync(user.Id);

        if (receptionist == null)
            return null;

        return MapToResponse(receptionist);
    }

    public async Task<bool> UpdateAsync(int id, ReceptionistUpdateRequestDto request)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist == null)
            return false;

        // Load user for audit
        var user = await _userRepository.GetByIdAsync(receptionist.UserId);
        var userBusinessId = user?.UserId ?? "Unknown";

        receptionist.UpdatedAt = DateTime.UtcNow;

        await _receptionistRepository.UpdateAsync(receptionist);

        await _auditLogService.CreateAsync(
            "Update Receptionist",
            "Success",
            $"Receptionist {userBusinessId} was updated.",
            receptionist.UserId);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist == null)
            return false;

        // Load user for audit
        var user = await _userRepository.GetByIdAsync(receptionist.UserId);
        var userBusinessId = user?.UserId ?? "Unknown";

        await _receptionistRepository.DeleteAsync(id);

        await _auditLogService.CreateAsync(
            "Deactivate Receptionist",
            "Success",
            $"Receptionist {userBusinessId} was deactivated.",
            receptionist.UserId);

        return true;
    }

    private static ReceptionistResponseDto MapToResponse(Receptionist receptionist)
    {
        return new ReceptionistResponseDto
        {
            UserId = receptionist.User?.UserId ?? string.Empty,
            FirstName = receptionist.User?.FirstName ?? string.Empty,
            LastName = receptionist.User?.LastName ?? string.Empty,
            Email = receptionist.User?.Email ?? string.Empty,
            CreatedAt = receptionist.CreatedAt,
            UpdatedAt = receptionist.UpdatedAt
        };
    }
}