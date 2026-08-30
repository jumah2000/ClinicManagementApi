using ClinicManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Data;

public class AppDbContext: DbContext
{
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.UserId)
                .IsUnique();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // Patient
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.UserId)
                .IsUnique();

            entity.HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Doctor
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.HasIndex(d => d.UserId)
                .IsUnique();

            entity.HasIndex(d => d.LicenseNumber)
                .IsUnique();

            entity.HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Receptionist
        modelBuilder.Entity<Receptionist>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasIndex(r => r.UserId)
                .IsUnique();

            entity.HasOne(r => r.User)
                .WithOne(u => u.Receptionist)
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Admin
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasIndex(a => a.UserId)
                .IsUnique();

            entity.HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OTP
        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.HasOne(o => o.User)
                .WithMany(u => u.Otps)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Appointment
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Medical Record
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Audit Log
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}