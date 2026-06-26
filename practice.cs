program.cs
==========
using dotnetapp.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();


course.cs
=========
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetapp.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(9, int.MaxValue, ErrorMessage = "Duration must be greater than 8 hours.")]
        public int Duration { get; set; }

        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }
    }
}

Instructor.cs
=============
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}

ApplicationsDb
==============
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private string connectionString = "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False";

        public DbSet<Course> Courses { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().ToTable("Courses");

            modelBuilder.Entity<Instructor>().ToTable("Instructors");

            modelBuilder.Entity<Instructor>()
                .HasMany(instructor => instructor.Courses)
                .WithOne(course => course.Instructor)
                .HasForeignKey(course => course.InstructorId);

            modelBuilder.Entity<Instructor>().HasData(
                new Instructor
                {
                    InstructorId = 1,
                    Name = "Demo",
                    Email = "demo@gmail.com",
                    HireDate = new DateTime(2025, 2, 1)
                },
                new Instructor
                {
                    InstructorId = 2,
                    Name = "Instructor 1",
                    Email = "instructor1@gmail.com",
                    HireDate = new DateTime(2023, 2, 1)
                }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    Title = "Course 1 - HTML Basics",
                    Description = "Html Basics",
                    Duration = 10,
                    InstructorId = 2
                }
            );
        }
    }
}

registerviewmodel
=================
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}


loginviewmodel
==============
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

AccountController
=================
using dotnetapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Instructor");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await _signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        false,
                        false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Instructor");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}

Instructorcontroller
using dotnetapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    [Authorize]
    [Route("instructors")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            List<Instructor> instructors = _context.Instructors.ToList();

            return View(instructors);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(instructor);
        }
    }
}

courseControler
=============
using dotnetapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [Authorize]
    [Route("courses")]
    public class CourseController : Controller
    {
        private static List<Course> courses = new List<Course>
        {
            new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Demo Description 1",
                Duration = 12
            },
            new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Demo Description 2",
                Duration = 20
            },
            new Course
            {
                CourseId = 3,
                Title = "Course 3",
                Description = "Demo Description 3",
                Duration = 9
            }
        };

        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            course.CourseId = courses.Count == 0 ? 1 : courses.Max(courseItem => courseItem.CourseId) + 1;

            courses.Add(course);

            return RedirectToAction("Index");
        }

        [Route("")]
        public IActionResult IndexDbContext()
        {
            List<Course> courseList = _context.Courses
                .Include(course => course.Instructor)
                .ToList();

            return View(courseList);
        }

        [Route("create")]
        public IActionResult CreateDbContext()
        {
            LoadInstructors();

            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateDbContext(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);

                _context.SaveChanges();

                return RedirectToAction("IndexDbContext");
            }

            LoadInstructors();

            return View(course);
        }

        [Route("edit/{id}")]
        public IActionResult EditDbContext(int id)
        {
            Course course = _context.Courses.Find(id);

            if (course == null)
            {
                return NotFound();
            }

            LoadInstructors();

            return View(course);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult EditDbContext(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);

                _context.SaveChanges();

                return RedirectToAction("IndexDbContext");
            }

            LoadInstructors();

            return View(course);
        }

        [Route("delete/{id}")]
        public IActionResult DeleteDbContext(int id)
        {
            Course course = _context.Courses
                .Include(courseItem => courseItem.Instructor)
                .FirstOrDefault(courseItem => courseItem.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult DeleteConfirmedDbContext(int id)
        {
            Course course = _context.Courses.Find(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);

            _context.SaveChanges();

            return RedirectToAction("IndexDbContext");
        }

        private void LoadInstructors()
        {
            ViewBag.Instructors = new SelectList(
                _context.Instructors.ToList(),
                "InstructorId",
                "Name");
        }
    }
}

views/acc/reg
=============
@model RegisterViewModel

<h2>Register</h2>

Register

    <div asp-validation-summary="All" class="text-danger"></div>

    <div>
        <label>Email</label>
        <input asp-for="Email" name="Email" class="form-control" />
    </div>

    <div>
        <label>Password</label>
        <input asp-for="Password" name="Password" class="form-control" />
    </div>

    <div>
        <label>Confirm password</label>
        <input asp-for="ConfirmPassword" name="ConfirmPassword" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary mt-2">Register</button>

</form>

<p>
    I already have an account?<a href="/Account/Login">Login</a>
</p>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

/acc/login
==========
@model LoginViewModel

<h1>Login</h1>

<form asp-action="Login" method="posty="All" class="text-danger"></div>

    <div>
        <label>Email</label>
        <input asp-for="Email" name="Email" class="form-control" />
    </div>

    <div>
        <label>Password</label>
        <input asp-for="Password" name="Password" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary mt-2">Login</button>

</form>

<p>
    Dont have an account?<a href="/Account/Register">Register</a>
</p>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

views/instructor/index
@model IEnumerable<Instructor>

<h1>Instructor List</h1>

<table class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>HireDate</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Name</td>
                <td>@item.Email</td>
                <td>@item.HireDate</td>
            </tr>
        }
    </tbody>
</table>

/instructor/create
==================
@model Instructor

<h1>Create Instructor</h1>

<form asp-action="Create" method="post/label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div>
        <label>Email</label>
        <input asp-for="Email" class="form-control" />
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>

    <div>
        <label>HireDate</label>
        <input asp-for="HireDate" type="date" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary mt-2">Create</button>

</form>

<a href="/instructors">Back to List</a>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

/course/index
=============
@model List<dotnetapp.Models.Course>

@{
    ViewData["Title"] = "Course List";
}

<h1>Course List</h1>

<p>
    CreateCreate New Course</a>
</p>

<table class="table">
    <thead>
        <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Duration</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var course in Model)
        {
            <tr>
                <td>@course.Title</td>
                <td>@course.Description</td>
                <td>@course.Duration</td>
            </tr>
        }
    </tbody>
</table>

/course/create
==============
@model Course

<h1>Create Course</h1>

<form asp-action="Create" <label>Title</label>
        <input asp-for="Title" class="form-control" />
    </div>

    <div>
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
    </div>

    <div>
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary mt-2">Create</button>

</form>

<a asp-action="Index>

/course/indexdbcontext
     =====
@model IEnumerable<Course>

<h1>Course List</h1>

<table class="table">
    <thead>
        <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Duration</th>
            <th>Instructor Name</th>
            <th>Actions</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Title</td>
                <td>@item.Description</td>
                <td>@item.Duration</td>
                <td>@item.Instructor.Name</td>
                <td>
                    <a href="/courses/edit/@item.CourseId">Edit</a>
                    |
                    <a href="/courses/delete/@item.CourseId">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>

/course/createdbcontext
     ================
@model Course

<h1>Create Course</h1>

<form aspontext

    <div>
        <label>Title</label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div>
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div>
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
        <span asp-validation-for="Duration" class="text-danger"></span>
    </div>

    <div>
        <label>Instructor Name</label>
        <select asp-for="InstructorId" asp-items="ViewBag.Instructors" class="form-control">
            <option value="">Select a Instructor</option>
        </select>
    </div>

    <button type="submit" class="btn btn-primary mt-2">Create</button>

</form>

<a href="/courses">Back to List</a>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

course/EditDbcontext
====================
@model Course

<h1>Edit Course</h1>

<form asp-action="EditDbContext" asp-route-id="@Model.CourseId" methodv>
        <label>Title</label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div>
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div>
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
        <span asp-validation-for="Duration" class="text-danger"></span>
    </div>

    <div>
        <label>Instructor Name</label>
        <select asp-for="InstructorId" asp-items="ViewBag.Instructors" class="form-control">
            <option value="">Select a Instructor</option>
        </select>
    </div>

    <button type="submit" class="btn btn-primary mt-2">Save</button>

</form>

<a href="/courses">Back to List</a>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

/course/deleteDbContext
=======================
@model Course

<h1>Delete Course</h1>

<h3>Are you sure you want to delete this course?</h3>

<hr />

<dl class="row">

    <dt class="col-sm-2">CourseId</dt>
    <dd class="col-sm-10">@Model.CourseId</dd>

    <dt class="col-sm-2">Name</dt>
    <dd class="col-sm-10">@Model.Title</dd>

    <dt class="col-sm-2">Age</dt>
    <dd class="col-sm-10">@Model.Description</dd>

    <dt class="col-sm-2">Condition</dt>
    <dd class="col-sm-10">@Model.Duration</dd>

</dl>

<form asp-action="DeleteConfirmedDbContext" asp-route-id="@Model.CourseId" method="poston>

</form>

<a href="/courses">Back to List</a>

