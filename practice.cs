Models/Doctor.cs
using System.Collections.Generic;

namespace dotnetapp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string Specialization { get; set; }

        public decimal ConsultationFee { get; set; }

        public ICollection<Patient>? Patients { get; set; }
    }
}

Models/Patient.cs
using System;

namespace dotnetapp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Condition { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int? DoctorId { get; set; }

        public Doctor? Doctor { get; set; }
    }
}

Models/User.cs
namespace dotnetapp.Models
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}

Models/LoginModel.cs
namespace dotnetapp.Models
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}

Models/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().ToTable("Doctors");

            modelBuilder.Entity<Patient>().ToTable("Patients");

            base.OnModelCreating(modelBuilder);
        }
    }
}

appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False"
  }
}

Program.cs
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Controllers/DoctorController.cs
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetDoctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            try
            {
                return Ok(await _context.Doctors
                    .Include(d => d.Patients)
                    .ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostDoctor")]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            try
            {
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDoctors),
                    new { id = doctor.DoctorId }, doctor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDoctor/{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            try
            {
                if (id != doctor.DoctorId)
                    return BadRequest();

                _context.Entry(doctor).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.Patients)
                    .FirstOrDefaultAsync(d => d.DoctorId == id);

                if (doctor == null)
                    return NotFound();

                if (doctor.Patients != null && doctor.Patients.Any())
                    return Conflict("Doctor has associated patients.");

                _context.Doctors.Remove(doctor);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

Controllers/PatientController.cs
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetPatients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            try
            {
                return Ok(await _context.Patients
                    .Include(p => p.Doctor)
                    .ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostPatient")]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            try
            {
                _context.Patients.Add(patient);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPatients),
                    new { id = patient.PatientId }, patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutPatient/{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            try
            {
                if (id != patient.PatientId)
                    return BadRequest();

                _context.Entry(patient).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePatient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                    return NotFound();

                _context.Patients.Remove(patient);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

Controllers/UserController.cs
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly string[] validRoles =
        {
            "Admin",
            "Organizer"
        };

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (!IsValidRole(user.Role))
                return BadRequest("Invalid role");

            if (await _context.Users
                .AnyAsync(x => x.Username == user.Username))
            {
                return Conflict("Username already exists");
            }

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register),
                new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginModel user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Username == user.Username &&
                    x.Password == user.Password);

            if (existingUser == null)
                return BadRequest("Login failed");

            return Ok(new
            {
                Message = "Login Successful",
                User = existingUser
            });
        }

        private bool IsValidRole(string role)
        {
            return validRoles.Contains(role);
        }
    }
}

