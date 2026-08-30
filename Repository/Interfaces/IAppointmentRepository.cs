using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id);

    Task<IEnumerable<Appointment>> GetAllAsync();

    Task<IEnumerable<Appointment>> GetByPatientUserIdAsync(string userId);

    Task<IEnumerable<Appointment>> GetByDoctorUserIdAsync(string userId);

    Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<bool> HasDoctorConflictAsync(int doctorId, DateTime appointmentDate, int? appointmentId = null);

    Task<bool> HasPatientConflictAsync(int patientId, int doctorId, DateTime appointmentDate, int? appointmentId = null);

    Task<Appointment> CreateAsync(Appointment appointment);

    Task UpdateAsync(Appointment appointment);

    Task DeleteAsync(int id);
}