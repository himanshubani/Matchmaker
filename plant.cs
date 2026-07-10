using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
 
builder.Services.AddControllers();
 
builder.Services.AddCors(options =>
 
{
 
    options.AddPolicy("CorsPolicy",
 
    policy =>
 
    {
 
    policy.AllowAnyOrigin()
 
    .AllowAnyMethod()
 
    .AllowAnyHeader();
 
    });
 
});
 
 
 
builder.Services.AddControllers()
 
    .AddJsonOptions(options =>
 
    {
 
    options.JsonSerializerOptions.PropertyNamingPolicy = null;      // PascalCase
 
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.DefaultIgnoreCondition =
 
    System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
 
    });
   
// Learn more about configuring Swagger/OpenAPI at Get started with Swashbuckle and ASP.NET Core | Microsoft Learn
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
app.UseRouting();
 
app.UseCors("CorsPolicy");
app.UseAuthorization();
 
 
app.MapControllers();
 
app.Run();
