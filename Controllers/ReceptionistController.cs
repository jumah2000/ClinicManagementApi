using ClinicManagementAPI.DTOs.Requests.Receptionist;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistController (IReceptionistService receptionistService) : ControllerBase
{
    private readonly IReceptionistService _receptionistService =
        receptionistService;

    [HttpPost]
    public async Task<IActionResult> Create(
        ReceptionistCreateRequestDto request)
    {
        try
        {
            var result = await _receptionistService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Receptionist created successfully.",
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
        var result = await _receptionistService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Receptionists retrieved successfully.",
            data = result
        });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result =
            await _receptionistService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Receptionist not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Receptionist retrieved successfully.",
            data = result
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        ReceptionistUpdateRequestDto request)
    {
        try
        {
            var result =
                await _receptionistService.UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Receptionist not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Receptionist updated successfully.",
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
        var result =
            await _receptionistService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Receptionist not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Receptionist deactivated successfully.",
            data = (object?)null
        });
    }
}