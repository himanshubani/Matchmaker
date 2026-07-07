Models/Order.cs
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}

Models/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace OrderService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}

Controllers/OrderController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            var response = new
            {
                message = "Order created successfully",
                data = order
            };

            return StatusCode(201, response);
        }
    }
}

Program.cs

using Microsoft.EntityFrameworkCore;
using OrderService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run("http://localhost:8080");

appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False"
  }
}

ocelot.json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/order-api/order",
      "UpstreamHttpMethod": [ "GET", "POST" ],

      "DownstreamPathTemplate": "/api/order",
      "DownstreamScheme": "http",

      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 8080
        }
      ]
    }
  ],

  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:8081"
  }
}

Program.cs
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ocelot.json", false, true);

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run("http://localhost:8081");

