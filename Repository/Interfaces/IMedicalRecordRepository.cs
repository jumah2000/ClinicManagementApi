using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IMedicalRecordRepository
{
    Task<MedicalRecord?> GetByIdAsync(int id);

    Task<IEnumerable<MedicalRecord>> GetAllAsync();

    Task<IEnumerable<MedicalRecord>> GetByPatientUserIdAsync(string userId);

    Task<IEnumerable<MedicalRecord>> GetByDoctorUserIdAsync(string userId);

    Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord);

    Task UpdateAsync(MedicalRecord medicalRecord);
}