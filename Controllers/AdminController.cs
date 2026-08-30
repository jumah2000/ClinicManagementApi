using ClinicManagementAPI.DTOs.Requests.Admin;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    private readonly IAdminService _adminService = adminService;

    [HttpPost]
    public async Task<IActionResult> Create(
        AdminCreateRequestDto request)
    {
        try
        {
            var result = await _adminService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Admin created successfully.",
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
            return Conflict(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _adminService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Admins retrieved successfully.",
            data = result
        });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _adminService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Admin not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Admin retrieved successfully.",
            data = result
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
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Admin not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Admin updated successfully.",
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _adminService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Admin not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Admin deactivated successfully.",
            data = (object?)null
        });
    }
}