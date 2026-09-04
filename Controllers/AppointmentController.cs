using ClinicManagementAPI.DTOs.Requests.Appointment;
using ClinicManagementAPI.DTOs.Responses.Appointment;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
{
   private readonly IAppointmentService _appointmentService = appointmentService;
   
    [HttpPost]
    public async Task<IActionResult> Create(AppointmentCreateRequestDto request)
    {
        try
        {
            var result = await _appointmentService.CreateAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<AppointmentResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Appointment created successfully.",
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
        var result = await _appointmentService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<AppointmentResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage = "Appointments retrieved successfully.",
                Data = result
            });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _appointmentService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Appointment not found.",
                    Data = null
                });
        }

        return Ok(new ApiResponse<AppointmentResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage = "Appointment retrieved successfully.",
                Data = result
            });
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        AppointmentUpdateRequestDto request)
    {
        try
        {
            var result =
                await _appointmentService.UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage = "Appointment not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Appointment updated successfully.",
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
        try
        {
            var result = await _appointmentService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage =
                            "Appointment not found or already cancelled.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage = "Appointment cancelled successfully.",
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
    
}