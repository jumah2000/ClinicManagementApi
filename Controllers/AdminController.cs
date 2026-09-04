using ClinicManagementAPI.DTOs.Requests.Admin;
using ClinicManagementAPI.DTOs.Responses.Admin;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService adminService) : ControllerBase
{
   private readonly IAdminService _adminService = adminService;

    [HttpPost]
    public async Task<IActionResult> Create(AdminCreateRequestDto request)
    {
        try
        {
            var result = await _adminService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<AdminResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Admin created successfully.",
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
            return Conflict(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _adminService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<AdminResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage = "Admins retrieved successfully.",
                Data = result
            });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _adminService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Admin not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<AdminResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage = "Admin retrieved successfully.",
                Data = result
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        AdminUpdateRequestDto request)
    {
        try
        {
            var result = await _adminService.UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage = "Admin not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Admin updated successfully.",
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _adminService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Admin not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<object>
            {
                ResponseCode = "00",
                ResponseMessage = "Admin deactivated successfully.",
                Data = null
            });
    }
    
}