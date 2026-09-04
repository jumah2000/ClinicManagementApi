using ClinicManagementAPI.DTOs.Requests.Doctor;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.DTOs.Responses.Doctor;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DoctorController(IDoctorService doctorService) : ControllerBase
{
     private readonly IDoctorService _doctorService = doctorService;
     
    [HttpPost]
    public async Task<IActionResult> Create(DoctorCreateRequestDto request)
    {
        try
        {
            var result = await _doctorService.CreateAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<DoctorResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Doctor created successfully.",
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
        var result = await _doctorService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<DoctorResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage = "Doctors retrieved successfully.",
                Data = result
            });
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _doctorService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Doctor not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<DoctorResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage = "Doctor retrieved successfully.",
                Data = result
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
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage = "Doctor not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Doctor updated successfully.",
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
        var result = await _doctorService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Doctor not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<object>
            {
                ResponseCode = "00",
                ResponseMessage = "Doctor deactivated successfully.",
                Data = null
            });
    }
    
}