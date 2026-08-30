using ClinicManagementAPI.DTOs.Requests.Patient;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientService patientService) : ControllerBase
{
    private readonly IPatientService _patientService = patientService;

    [HttpPost]
    public async Task<IActionResult> Create(
        PatientCreateRequestDto request)
    {
        try
        {
            var result = await _patientService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Patient created successfully.",
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
        var result = await _patientService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Patients retrieved successfully.",
            data = result
        });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _patientService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Patient not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Patient retrieved successfully.",
            data = result
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        PatientUpdateRequestDto request)
    {
        try
        {
            var result = await _patientService.UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Patient not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Patient updated successfully.",
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
        var result = await _patientService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Patient not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Patient deactivated successfully.",
            data = (object?)null
        });
    }
}