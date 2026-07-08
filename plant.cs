/////////////////DoctorController///////////////
using dotnetapp.Models;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
namespace dotnetapp.Controllers
 
{
 
    [Route("api/[controller]")]
 
    [ApiController]
 
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
 
    return await _context.Doctors
 
    .Include(d => d.Patients)
 
    .ToListAsync();
 
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
 
   
   
    return Created("",doctor);
 
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
 
    var doctor = await _context.Doctors.FindAsync(id);
 
    if (doctor == null)
 
    return NotFound();
 
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
////////////////patientController///////////////////
                   
 
 
 
using dotnetapp.Models;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
namespace dotnetapp.Controllers
 
{
 
    [Route("api/[controller]")]
 
    [ApiController]
 
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
 
    var patients = await _context.Patients
 
    .Include(p => p.Doctor)
 
    .ToListAsync();
 
    return Ok(patients);
 
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
 
    return StatusCode(StatusCodes.Status201Created, patient);
 
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
//////////////////////////userController//////////////////////
using dotnetapp.Models;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
namespace dotnetapp.Controllers
 
{
 
    [Route("api/users")]
 
    [ApiController]
 
    public class UserController : ControllerBase
 
    {
 
    private readonly ApplicationDbContext _context;
 
    public UserController(ApplicationDbContext context)
 
    {
 
    _context = context;
 
    }
 
    private bool IsValidRole(string role)
 
    {
 
    var validRoles = new[] { "Admin", "Organizer" };
 
    return validRoles.Contains(role);
 
    }
 
    [HttpPost("register")]
 
    public async Task<ActionResult<User>> Register(User user)
 
    {
 
    if (!IsValidRole(user.Role))
 
    return BadRequest("Invalid role");
 
    var existingUser = await _context.Users
 
    .FirstOrDefaultAsync(x => x.Username == user.Username);
 
    if (existingUser != null)
 
    return Conflict();
 
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
 
    Message = "Login successful",
 
    User = existingUser
 
    });
 
    }
 
    }
 
}
 
 
/////////////////Models Doctor.cs/////////////////////////////
using System.Collections.Generic;
 
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
 
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
/////////////Model LoginModel.cs///////////////
namespace dotnetapp.Models
 
{
 
 public class LoginModel
 
 {
 
 public string Username { get; set; }
 
 public string Password { get; set; }
 
 }
 
}
///////////////Model Patient.cs///////////////////
using System;
 
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
 
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
////////////////////Model User.cs////////////////
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
//////////////////////Model ApplicationDbContext.cs//////////////////
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
 
 base.OnModelCreating(modelBuilder);
 
 modelBuilder.Entity<Doctor>()
 
 .HasMany(d => d.Patients)
 
 .WithOne(p => p.Doctor)
 
 .HasForeignKey(p => p.DoctorId)
 
 .OnDelete(DeleteBehavior.Cascade);
 
 }
 
 }
 
}
 
/////////////////program.cs////////////
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
 
var builder = WebApplication.CreateBuilder(args);
 
 
// Add services to the container.
 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
 
    .AddJsonOptions(options =>
 
    {
 
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
 
    });
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer("User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False"));
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
Get started with Swashbuckle and ASP.NET Core | Microsoft Learn
Learn how to add Swashbuckle to your ASP.NET Core web API project to integrate the Swagger UI.
 
