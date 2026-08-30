using ClinicManagementAPI.Data;
using ClinicManagementAPI.Enum;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{private readonly AppDbContext _context = context;

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPatientUserIdAsync(
        string userId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Where(a => a.Patient.User.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorUserIdAsync(
        string userId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Where(a => a.Doctor.User.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Where(a =>
                a.AppointmentDate >= startDate &&
                a.AppointmentDate <= endDate)
            .ToListAsync();
    }

    public async Task<bool> HasDoctorConflictAsync(
        int doctorId,
        DateTime appointmentDate,
        int? appointmentId = null)
    {
        return await _context.Appointments
            .AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate == appointmentDate &&
                a.Status != AppointmentStatus.Cancelled &&
                (!appointmentId.HasValue ||
                 a.Id != appointmentId.Value));
    }

    public async Task<bool> HasPatientConflictAsync(
        int patientId,
        int doctorId,
        DateTime appointmentDate,
        int? appointmentId = null)
    {
        return await _context.Appointments
            .AnyAsync(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.AppointmentDate == appointmentDate &&
                a.Status != AppointmentStatus.Cancelled &&
                (!appointmentId.HasValue ||
                 a.Id != appointmentId.Value));
    }

    public async Task<Appointment> CreateAsync(
        Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task UpdateAsync(
        Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment != null)
        {
            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
    
}