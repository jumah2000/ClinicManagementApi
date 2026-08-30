using ClinicManagementAPI.DTOs.Requests.Doctor;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DoctorController(IDoctorService doctorService) : ControllerBase
{
    private readonly IDoctorService _doctorService = doctorService;

    [HttpPost]
    public async Task<IActionResult> Create(
        DoctorCreateRequestDto request)
    {
        try
        {
            var result = await _doctorService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Doctor created successfully.",
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
        var result = await _doctorService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Doctors retrieved successfully.",
            data = result
        });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _doctorService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Doctor not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Doctor retrieved successfully.",
            data = result
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        DoctorUpdateRequestDto request)
    {
        try
        {
            var result = await _doctorService.UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Doctor not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Doctor updated successfully.",
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
        var result = await _doctorService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Doctor not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Doctor deactivated successfully.",
            data = (object?)null
        });
    }
}