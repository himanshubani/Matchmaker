using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string MobileNumber { get; set; }

        public string UserRole { get; set; }
    }
}

namespace dotnetapp.Models
{
    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Plant
    {
        [Key]
        public int PlantId { get; set; }

        public string Name { get; set; }

        public string ScientificName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}


using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Plant> Plants { get; set; }
    }
}


using System.Security.Claims;
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IAuthService
    {
        string Registration(User model, string role);

        string Login(LoginModel model);

        string GenerateToken(IEnumerable<Claim> claims);
    }
}




using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnetapp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Registration(User model, string role)
        {
            return "User Registered Successfully";
        }

        public string Login(LoginModel model)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Email == model.Email &&
                    x.Password == model.Password);

            if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole),
                new Claim(ClaimTypes.Name, user.Username)
            };

            return GenerateToken(claims);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}




using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthService authService,
            ApplicationDbContext context,
            ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var token = _authService.Login(model);

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Invalid Credentials");

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (user.UserRole != "Admin" &&
                    user.UserRole != "Customer")
                {
                    return BadRequest("Invalid Role");
                }

                _authService.Registration(
                    user,
                    user.UserRole);

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("User Registered Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}


using dotnetapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/plants")]
    public class PlantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlantController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var plants = await _context.Plants
                .ToListAsync();

            return Ok(plants);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(
            [FromBody] Plant plant)
        {
            if (plant == null)
                return BadRequest();

            await _context.Plants.AddAsync(plant);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = plant.PlantId },
                plant);
        }
    }
}



{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=appdb;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "Jwt": {
    "Key": "ThisIsMySuperSecretKey12345",
    "Issuer": "PlantAPI",
    "Audience": "PlantAPIUsers"
  }
}


using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]))
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();









