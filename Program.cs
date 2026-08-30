using ClinicManagementAPI.Data;
using ClinicManagementAPI.Repository.Implementations;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Implementations;
using ClinicManagementAPI.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDataBaseConnection")));

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepositor>();
builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

//Services

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IReceptionistService, ReceptionistService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

builder.Services.AddScoped<IAuditLogService, AuditLogService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ISmsService, SmsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/openapi/v1.json", "v1");
        x.RoutePrefix = "swagger-ui";
    });
}


app.UseAuthorization();

app.MapControllers();

app.Run();