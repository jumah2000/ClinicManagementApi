using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using ClinicManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordController(IMedicalRecordService medicalRecordService) : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService =
        medicalRecordService;


    // POST: api/medicalrecord
    [HttpPost]
    public async Task<IActionResult> Create(
        MedicalRecordCreateRequestDto request)
    {
        try
        {
            var result =
                await _medicalRecordService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, new
            {
                responseCode = "00",
                responseMessage = "Medical record created successfully.",
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
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }


    // GET: api/medicalrecord
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _medicalRecordService.GetAllAsync();

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Medical records retrieved successfully.",
            data = result
        });
    }


    // GET: api/medicalrecord/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _medicalRecordService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                responseCode = "04",
                responseMessage = "Medical record not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            responseCode = "00",
            responseMessage = "Medical record retrieved successfully.",
            data = result
        });
    }


    // GET: api/medicalrecord/patient/PAT-2026-000001
    [HttpGet("patient/{userId}")]
    public async Task<IActionResult> GetByPatientUserId(
        string userId)
    {
        try
        {
            var result =
                await _medicalRecordService
                    .GetByPatientUserIdAsync(userId);

            return Ok(new
            {
                responseCode = "00",
                responseMessage =
                    "Patient medical records retrieved successfully.",
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
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }


    // GET: api/medicalrecord/doctor/DOC-2026-000001
    [HttpGet("doctor/{userId}")]
    public async Task<IActionResult> GetByDoctorUserId(
        string userId)
    {
        try
        {
            var result =
                await _medicalRecordService
                    .GetByDoctorUserIdAsync(userId);

            return Ok(new
            {
                responseCode = "00",
                responseMessage =
                    "Doctor medical records retrieved successfully.",
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
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }


    // PUT: api/medicalrecord/5
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
                return NotFound(new
                {
                    responseCode = "04",
                    responseMessage = "Medical record not found.",
                    data = (object?)null
                });
            }

            return Ok(new
            {
                responseCode = "00",
                responseMessage = "Medical record updated successfully.",
                data = (object?)null
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
            return BadRequest(new
            {
                responseCode = "01",
                responseMessage = ex.Message,
                data = (object?)null
            });
        }
    }
}