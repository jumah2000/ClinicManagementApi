using ClinicManagementAPI.DTOs.Requests.Receptionist;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.DTOs.Responses.Receptionist;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistController (IReceptionistService receptionistService) : ControllerBase
{
    private readonly IReceptionistService _receptionistService = receptionistService;

    [HttpPost]
    public async Task<IActionResult> Create(ReceptionistCreateRequestDto request)
    {
        try
        {
            var result =
                await _receptionistService.CreateAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<ReceptionistResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Receptionist created successfully.",
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
        var result =
            await _receptionistService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<ReceptionistResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage =
                    "Receptionists retrieved successfully.",
                Data = result
            });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result =
            await _receptionistService.GetByUserIdAsync(userId);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage =
                        "Receptionist not found.",
                    Data = null
                });
        }

        return Ok(new ApiResponse<ReceptionistResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage =
                    "Receptionist retrieved successfully.",
                Data = result
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
                await _receptionistService.UpdateAsync(
                    id,
                    request);

            if (!result)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage =
                            "Receptionist not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Receptionist updated successfully.",
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
        var result =
            await _receptionistService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage =
                        "Receptionist not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<object>
            {
                ResponseCode = "00",
                ResponseMessage =
                    "Receptionist deactivated successfully.",
                Data = null
            });
    }
}