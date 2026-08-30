using ClinicManagementAPI.DTOs.Requests.Appointment;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
{
    private readonly IAppointmentService _appointmentService =
        appointmentService;

    // POST: api/appointment
    [HttpPost]
    public async Task<IActionResult> Create(
        AppointmentCreateRequestDto request)
    {
        try
        {
            var result = await _appointmentService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Appointment created successfully.",
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


    // GET: api/appointment
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _appointmentService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Appointments retrieved successfully.",
            data = result
        });
    }


    // GET: api/appointment/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _appointmentService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Appointment not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Appointment retrieved successfully.",
            data = result
        });
    }


    // PUT: api/appointment/5
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
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Appointment not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Appointment updated successfully.",
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


    // DELETE: api/appointment/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _appointmentService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Appointment not found or already cancelled.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Appointment cancelled successfully.",
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
}