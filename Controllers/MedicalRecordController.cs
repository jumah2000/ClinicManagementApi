using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using ClinicManagementAPI.DTOs.Responses.Common;
using ClinicManagementAPI.DTOs.Responses.MedicalRecord;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordController(IMedicalRecordService medicalRecordService) : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService = medicalRecordService;
    
    [HttpPost]
    public async Task<IActionResult> Create(MedicalRecordCreateRequestDto request)
    {
        try
        {
            var result = await _medicalRecordService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<MedicalRecordResponseDto>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Medical record created successfully.",
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
            return BadRequest(
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
            await _medicalRecordService.GetAllAsync();

        return Ok(
            new ApiResponse<IEnumerable<MedicalRecordResponseDto>>
            {
                ResponseCode = "00",
                ResponseMessage =
                    "Medical records retrieved successfully.",
                Data = result
            });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _medicalRecordService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    ResponseCode = "04",
                    ResponseMessage = "Medical record not found.",
                    Data = null
                });
        }

        return Ok(
            new ApiResponse<MedicalRecordResponseDto>
            {
                ResponseCode = "00",
                ResponseMessage =
                    "Medical record retrieved successfully.",
                Data = result
            });
    }
    
    [HttpGet("patient/{userId}")]
    public async Task<IActionResult> GetByPatientUserId(
        string userId)
    {
        try
        {
            var result =
                await _medicalRecordService
                    .GetByPatientUserIdAsync(userId);

            return Ok(
                new ApiResponse<IEnumerable<MedicalRecordResponseDto>>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Patient medical records retrieved successfully.",
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
            return BadRequest(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpGet("doctor/{userId}")]
    public async Task<IActionResult> GetByDoctorUserId(
        string userId)
    {
        try
        {
            var result =
                await _medicalRecordService
                    .GetByDoctorUserIdAsync(userId);

            return Ok(
                new ApiResponse<IEnumerable<MedicalRecordResponseDto>>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Doctor medical records retrieved successfully.",
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
            return BadRequest(
                new ApiResponse<object>
                {
                    ResponseCode = "01",
                    ResponseMessage = ex.Message,
                    Data = null
                });
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        MedicalRecordUpdateRequestDto request)
    {
        try
        {
            var result =
                await _medicalRecordService
                    .UpdateAsync(id, request);

            if (!result)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        ResponseCode = "04",
                        ResponseMessage =
                            "Medical record not found.",
                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<object>
                {
                    ResponseCode = "00",
                    ResponseMessage =
                        "Medical record updated successfully.",
                    Data = null
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