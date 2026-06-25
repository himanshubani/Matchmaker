using dotnetapp.Controllers;
using dotnetapp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Required if using custom OpenApiInfo metadata

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger Generation Options
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Artworks & Collections API",
        Version = "v1",
        Description = "A RESTful API endpoint mapping matrix for managing galleries and artwork records."
    });
});

// Configure Database Engine Context
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer("user id=sa;password=examlyMssql@123;server=localhost;database=appdb;trusted_connection=false;persist security info=false;encrypt=false;TrustServerCertificate=true;")); // FIXED: Appended SSL handler bypass

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Access locally via: https://localhost:<port>/swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
