using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicianBookingSystem.Exceptions;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Controllers
{
    // [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext db;
 
        public BookingController(ApplicationDbContext db1)
        {
            db = db1;
        }
 
        public IActionResult Index()
        {
            var slots = db.Slots.ToList();
            return View(slots);
        }
 
        public IActionResult Book(int id)
        {
            var slot = db.Slots.FirstOrDefault(s => s.SlotID == id);
            if(slot == null)
                return View(new Slot());
            return View(slot);
        }
 
        [HttpPost]
        public IActionResult Book(int id, int Userid)
        {
            try
            {
                var slot = db.Slots.FirstOrDefault(s => s.SlotID == id);
                if (slot == null)
                    return NotFound();
                if (slot.Bookings.Count >= 5)
                    throw new SlotBookingException("Slot is full.");
                if (slot.Bookings.Any(b => b.UserID == Userid))
                    throw new SlotBookingException("You have already booked this slot.");
                Booking booking = new Booking
                {
                    SlotID = id,
                    UserID = Userid,
                    Slot = slot
 
                };
                slot.Bookings.Add(booking);
                db.Bookings.Add(booking);
                db.SaveChanges();
                return View("Book",slot);
            }
            catch(SlotBookingException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Book", db.Slots.FirstOrDefault(s => s.SlotID == id));
            }
        }
        public IActionResult Summary(int Userid)
        {
            var b=db.Bookings.Where(bb=>bb.UserID == Userid).ToList();
            return View(b);
        }
 
 
    }
}
 
 
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
 
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
 
        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult Privacy()
        {
            return View();
        }
 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
 
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Controllers
{
    // [Route("[controller]")]
    public class SlotController : Controller
    {
        private readonly ApplicationDbContext db;
 
        public SlotController(ApplicationDbContext db1)
        {
            db=db1;
        }
 
        public IActionResult Index()
        {
            var slots = db.Slots.ToList();
            return View(slots);
        }
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace MusicianBookingSystem.Exceptions
{
    public class SlotBookingException : Exception
    {
        public SlotBookingException (string message) : base(message) {}
    }
}
 
 
 
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MusicianBookingSystem.Models;
using System.Collections.Generic;
 
namespace MusicianBookingSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options){}
 
        public DbSet<Slot>Slots{get;set;}
         public DbSet<Booking> Bookings {get; set;}
       
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
namespace MusicianBookingSystem.Models
{
    public class Booking{
 
        public int BookingID {get; set;}
 
        public int SlotID {get; set;}
 
        public int UserID {get; set;}
 
        public Slot Slot {get; set;}
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
namespace MusicianBookingSystem.Models
{
    public class Slot
    {
        [Key]
        public int SlotID{get;set;}
        public DateTime Time{get;set;}
        public int Duration{get;set;}
        public int Capacity{get;set;}
        public List<Booking> Bookings{get;set;}=new List<Booking>();
    }
}
 


 
var builder = WebApplication.CreateBuilder(args);
 
 
// Add services to the container.
builder.Services.AddControllersWithViews();
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
 
app.UseHttpsRedirection();
app.UseStaticFiles();
 
app.UseRouting();
 
app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Slot}/{action=Index}/{id?}");
 
app.Run();
 
Info
A bill like a multitool and plumage full of colour
 
