using ClinicManagementAPI.DTOs.Requests.Patient;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.DTOs.Responses.Patient;
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

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<PatientResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Patient created successfully.",
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
        var result = await _patientService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<PatientResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage = "Patients retrieved successfully.",
                Data = result
            });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _patientService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Patient not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<PatientResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage = "Patient retrieved successfully.",
                Data = result
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
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage = "Patient not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Patient updated successfully.",
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
        var result = await _patientService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Patient not found.",
                    Data = null
                });
        }

        return Ok(new ApiResponse<object>
            {
                ResponseCode = "00",
                ResponseMessage = "Patient deactivated successfully.",
                Data = null
            });
    }

}