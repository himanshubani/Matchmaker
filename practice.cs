namespace dotnetapp
 
{
 
    using System.Text;
 
    using dotnetapp.Models;
 
    using dotnetapp.Services;
 
    using Microsoft.AspNetCore.Authentication.JwtBearer;
 
    using Microsoft.EntityFrameworkCore;
 
    using Microsoft.IdentityModel.Tokens;
 
    public class Program
 
    {
 
    public static void Main(string[] args)
 
    {
 
    var builder = WebApplication.CreateBuilder(args);
 
    // Run on port 8080
 
    builder.WebHost.UseUrls("http://0.0.0.0:8080");
 
    // Add configuration values in-memory so appsettings.json is not required
 
    var configValues = new Dictionary<string, string?>
 
    {
 
    { "ConnectionStrings:DefaultConnection", "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False" },
 
    { "Jwt:Key", "ThisIsMyVerySecureJwtKeyForBooksManagement12345" },
 
    { "Jwt:Issuer", "dotnetapp" },
 
    { "Jwt:Audience", "dotnetapp_users" },
 
   
    };
            IConfigurationBuilder configurationBuilder = builder.Configuration.AddInMemoryCollection(configValues);
 
            builder.Services.AddControllers();
 
    builder.Services.AddEndpointsApiExplorer();
 
    builder.Services.AddSwaggerGen();
 
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
    builder.Services.AddScoped<IAuthService, AuthService>();
 
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsMyVerySecureJwtKeyForBooksManagement12345";
 
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "dotnetapp";
 
    var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "dotnetapp_users";
 
    builder.Services.AddAuthentication(options =>
 
    {
 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 
    })
 
    .AddJwtBearer(options =>
 
    {
 
    options.RequireHttpsMetadata = false;
 
    options.SaveToken = true;
 
    options.TokenValidationParameters = new TokenValidationParameters
 
    {
 
    ValidateIssuer = true,
 
    ValidateAudience = true,
 
    ValidateLifetime = true,
 
    ValidateIssuerSigningKey = true,
 
    ValidIssuer = jwtIssuer,
 
    ValidAudience = jwtAudience,
 
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
 
    };
 
    });
 
    builder.Services.AddAuthorization();
 
    var app = builder.Build();
 
    // Create database automatically
 
    using (var scope = app.Services.CreateScope())
 
    {
 
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
 
    db.Database.EnsureCreated();
 
    }
 
    app.UseSwagger();
 
    app.UseSwaggerUI();
 
    app.UseAuthentication();
 
    app.UseAuthorization();
 
    app.MapControllers();
 
    app.Run();
 
    }
 
    }
 
}
 
