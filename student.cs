Inc. Proj 3


---------------------
 
Models->


========
 
Doctor.cs-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;
 
namespace dotnetapp.Models


{


    public class Doctor


    {


        [Key]


        public int DoctorId{get;set;}


        public string Name {get;set;}


        public string Specialization {get;set;}


        public decimal ConsultationFee {get;set;}


        public ICollection<Patient>? Patients{get;set;}
 
    }


}
 
Patient.cs-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;
 
namespace dotnetapp.Models


{


    public class Patient 


    {


        [Key]


        public int PatientId {get;set;}


        public string Name {get;set;}


        public int Age {get;set;}


        public string Condition {get;set;}


        public DateTime AppointmentDate {get;set;}


        public int? DoctorId {get;set;}


        public Doctor? Doctor{get;set;}


    }


}
 
User.cs-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;
 
namespace dotnetapp.Models


{


    public class User


    {


        [Key]


        public long Id {get;set;}


        public string Username {get;set;}


        public string Password {get;set;}


        public string Role {get;set;}


    }


}
 
Login-model.cs-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;
 
namespace dotnetapp.Models


{


    public class LoginModel


    {


        public string Username {get;set;}


        public string Password {get;set;}


    }


}
 
ApplicationDbContext-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;


using Microsoft.EntityFrameworkCore;
 
namespace dotnetapp.Models


{


    public class ApplicationDbContext : DbContext


    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){
 
        } 


        public DbSet<Doctor> Doctors {get;set;}


        public DbSet<Patient> Patients {get;set;}


        public DbSet<User> Users {get;set;}
 
    }


}
 
Controller->


=================


DoctorController-
 
using System;


using System.Collections.Generic;


using System.Diagnostics;


using System.Linq;


using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;


using dotnetapp.Models;
 
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


        //[HttpGet]


        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()


        {


            try


            {


                return await _context.Doctors.Include(d => d.Patients).ToListAsync();


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }
 
        [HttpPost("PostDoctor")]


        //[HttpPost]


        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)


        {


            try


            {


                _context.Doctors.Add(doctor);


                await _context.SaveChangesAsync();
 
                //return CreatedAtAction(nameof(GetDoctors), new { id = doctor.DoctorId }, doctor);


                //return Created("",doctor);


                return StatusCode(StatusCodes.Status201Created, doctor);


            }


            catch(Exception ex)


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
 
                var existingDoctor = await _context.Doctors


                .Include(d => d.Patients)


                .FirstOrDefaultAsync(d => d.DoctorId == id);
 
                if (existingDoctor == null)


                    return NotFound();
 
                if (existingDoctor.Patients != null && existingDoctor.Patients.Any())


                    return Conflict("Doctor has associated patients");
 
                existingDoctor.Name = doctor.Name;


                existingDoctor.Specialization = doctor.Specialization;


                existingDoctor.ConsultationFee = doctor.ConsultationFee;
 
                await _context.SaveChangesAsync();


                return NoContent();


            }


            catch(Exception ex)


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


                    return Conflict("Doctor has associated patients");
 
                _context.Doctors.Remove(doctor);


                await _context.SaveChangesAsync();
 
                return NoContent();


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }


    }


}
 
 
PatientController-
 
using System;


using System.Collections.Generic;


using System.Diagnostics;


using System.Linq;


using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;


using dotnetapp.Models;
 
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


        //[HttpGet]


        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()


        {


            try


            {


                return Ok(await _context.Patients.Include(p => p.Doctor).ToListAsync());


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }
 
        [HttpPost("PostPatient")]


        //[HttpPost]


        public async Task<ActionResult<Patient>> PostPatient(Patient patient)


        {


            try


            {


                _context.Patients.Add(patient);


                await _context.SaveChangesAsync();
 
                // return CreatedAtAction(nameof(PostPatient), new { id = patient.PatientId }, patient);


                //return Created("",patient);


                return StatusCode(StatusCodes.Status201Created, patient);


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }
 
        [HttpPut("PutPatient/{id}")]


        public async Task<ActionResult> PutPatient(int id, Patient patient)


        {


            try


            {


                if (id != patient.PatientId)


                    return BadRequest();
 
                var existingPatient = await _context.Patients.FindAsync(id);
 
                if (existingPatient == null)


                    return NotFound();
 
                existingPatient.Name = patient.Name;


                existingPatient.Age = patient.Age;


                existingPatient.Condition = patient.Condition;


                existingPatient.AppointmentDate = patient.AppointmentDate;


                existingPatient.DoctorId = patient.DoctorId;
 
                await _context.SaveChangesAsync();
 
                return NoContent();


            }


            catch(Exception ex)


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


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }


    }


}
 
UserController-
 
using System;


using System.Collections.Generic;


using System.Linq;


using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc;


using dotnetapp.Models;


using Microsoft.EntityFrameworkCore;
 
namespace dotnetapp.Controllers


{


    [ApiController]


    [Route("api/users")]


    public class UserController : ControllerBase


    {


        private readonly ApplicationDbContext _context;
 
        public UserController(ApplicationDbContext context)


        {


            _context = context;


        }
 
        [HttpPost("register")]


        public async Task<ActionResult<User>> Register(User user)


        {


            try


            {


                if (await _context.Users.AnyAsync(x => x.Username == user.Username))


                    return Conflict("Username already exists");
 
                if (!IsValidRole(user.Role))


                    return BadRequest("Invalid role");
 
                _context.Users.Add(user);


                await _context.SaveChangesAsync();
 
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }
 
        [HttpPost("login")]


        public async Task<ActionResult<object>> Login(LoginModel user)


        {


            try


            {


                var existingUser = await _context.Users.FirstOrDefaultAsync(x =>


                    x.Username == user.Username && 


                    x.Password == user.Password);
 
                if (existingUser == null)


                    return BadRequest(new { message = "Invalid credentials" });
 
                return Ok(new


                {


                    message = "Login successful",


                    user = existingUser


                });


            }


            catch(Exception ex)


            {


                return BadRequest(ex.Message);


            }


        }
 
        private bool IsValidRole(string role)


        {


            string[] validRoles = { "Admin", "Organizer" };


            return validRoles.Contains(role);


        }


    }


}
 
 
Program.cs->


============
 
using dotnetapp.Models;


using Microsoft.EntityFrameworkCore;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
 
builder.Services.AddControllers();


builder.Services.AddControllers().AddJsonOptions(options=>{


    options.JsonSerializerOptions.PropertyNamingPolicy=null;


});


builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlServer("User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=false;Encrypt=false;"));
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())


{


    app.UseSwagger();


    app.UseSwaggerUI();


}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.UseRouting();
 
app.MapControllers();
 
app.Run();
 
 
 
 
=============================================
 
 
make a folder inside src of angularapp the folder name is models->
 
inside model folder make 4 files->
 
src/models/


user.model.ts-
 
export interface User{


   Id?: number;


   Username: string;


   Password: string;


   Role: string;


}
 
        


src/models/


doctor.model.ts-
 
import { Patient } from "./patient.model";
 
export interface Doctor{


    DoctorId?: number;


    Name: string;


    Specialization: string;


    ConsultationFee: number;


    Patients?: Patient[]


}
 
 
src/models/


patient.model.ts-
 
import { Doctor } from "./doctor.model";
 
export interface Patient{


    PatientId?: number;


    Name: string;


    Age: number;


    Condition: string;


    AppointmentDate: Date;


    DoctorId: number;


    Doctor?: Doctor;


}
 
 
src/models/


login-model.model.ts-
 
export interface LoginModel{


    Username: string;


    Password: string;


   }
 
 
=====================================
 
now run these commands in integrated terminal of angularapp-
 
 
npx ng g c registration


npx ng g c login


npx ng g c navbar


npx ng g c admin


npx ng g c organizer


npx ng g c patient


npx ng g c doctor


npx ng g c home


npx ng g c error
 
 
done till week 6 day 2 :)

 
create patient.html


========================================================================
 
<form #patientForm="ngForm" (ngSubmit)="createPatient(patientForm)">
<h2>Create Patient</h2>
<label>Name</label>
<input type="text" id="patientName" name="patientName" required minlength="2" maxlength="100"


        [(ngModel)]="newPatient.Name" #patientName="ngModel">
<div class="error-message">


        Name is required


        Age is required
</div>
<label>Age</label>
<input type="number" id="patientAge" name="patientAge" required [(ngModel)]="newPatient.Age" #patientAge="ngModel">
<div class="error-message">


        Age is required
</div>
<label>Condition</label>
<input type="text" id="patientCondition" name="patientCondition" required [(ngModel)]="newPatient.Condition"


        #patientCondition="ngModel">
<div class="error-message">


        Condition is required
</div>
<label>Appointment Date</label>
<input type="date" id="appointmentDate" name="appointmentDate" required [(ngModel)]="newPatient.AppointmentDate"


        #appointmentDate="ngModel">
<div class="error-message">


        Appointment Date is required
</div>
<button type="submit" id="submit" [disabled]="patientForm.invalid">


        Create Patient
</button>
</form>
 
=======================================================================


create-pt.comp.ts-
 
import { Component } from '@angular/core';


import { Router } from '@angular/router';


import { NgForm } from '@angular/forms';


import { Patient } from '../../../models/patient.model';


import { PatientService } from '../../services/patient.service';


@Component({


  selector: 'app-create-patient',


  templateUrl: './create-patient.component.html',


  styleUrls: ['./create-patient.component.css']


})


export class CreatePatientComponent {


  newPatient: Patient = {


    Name: '',


    Age: null,


    Condition: '',


    AppointmentDate: null,


    DoctorId: 0


  };


  constructor(


    private patientService: PatientService,


    private router: Router


  ) { }


  createPatient(form: NgForm) {


    if (form.invalid) {


      return;


    }


    this.patientService.createPatient(this.newPatient).subscribe(() => {


      this.newPatient = {


        Name: '',


        Age: null,


        Condition: '',


        AppointmentDate: null,


        DoctorId: 0


      };


      form.resetForm();


      this.router.navigate(['/admin']);


    });


  }


}
 
 
===========================================================
 
create doctor.html


==================
 
<form #doctorForm="ngForm" (ngSubmit)="createDoctor()">
<h2>Create Doctor</h2>
<label>Doctor Name</label>
<input


        type="text"


        id="doctorName"


        name="doctorName"


        required


        [(ngModel)]="newDoctor.Name"


        #doctorName="ngModel">
<div class="error-message"


        *ngIf="doctorName.invalid && (doctorName.touched || doctorName.dirty)">
<div *ngIf="doctorName.errors?.['required']">


            Doctor Name is required</div>
</div>
<label>Specialization</label>
<input


        type="text"


        id="specialization"


        name="specialization"


        required


        [(ngModel)]="newDoctor.Specialization"


        #specialization="ngModel">
<div class="error-message"


        *ngIf="specialization.invalid && (specialization.touched || specialization.dirty)">
<div *ngIf="specialization.errors?.['required']">


            Specialization is required</div>
</div>
<label>Consultation Fee</label>
<input


        type="number"


        id="consultationFee"


        name="consultationFee"


        required


        [(ngModel)]="newDoctor.ConsultationFee"


        #consultationFee="ngModel">
<div class="error-message"


        *ngIf="consultationFee.invalid && (consultationFee.touched || consultationFee.dirty)">
<div *ngIf="consultationFee.errors?.['required']">


            Consultation Fee is required</div>
</div>
<div id="status">


        {{ feeStatus }}
</div>
<button


        type="submit"


        id="submit"


        [disabled]="doctorForm.invalid">


    Create Doctor
</button>
</form>
 
======================================================
 
create-doc.comp.ts=
 
import { Component } from '@angular/core';


import { Router } from '@angular/router';


import { Doctor } from '../../../models/doctor.model';


import { DoctorService } from '../../services/doctor.service';
 
@Component({


  selector: 'app-create-doctor',


  templateUrl: './create-doctor.component.html',


  styleUrls: ['./create-doctor.component.css']


})


export class CreateDoctorComponent {


  newDoctor: Doctor = {


    Name: '',


    Specialization: '',


    ConsultationFee: 0


  };


  constructor(


    private doctorService: DoctorService,


    private router: Router


  ) { }


  get feeStatus(): string {


    if (this.newDoctor.ConsultationFee == null || this.newDoctor.ConsultationFee == 0) {


      return 'Low Fee';


    }


    if (this.newDoctor.ConsultationFee < 100) {


      return 'Insufficient';


    }


    if (this.newDoctor.ConsultationFee < 300) {


      return 'Adequate';


    }


    return 'Good Fee';


  }


  createDoctor() {


    this.doctorService.createDoctor(this.newDoctor).subscribe(() => {


      this.router.navigate(['/admin']);


    });


  }


}
 
=======================================================
 
login.ts-
 
import { Component } from '@angular/core';


import { NgForm } from '@angular/forms';


import { Router } from '@angular/router';


import { LoginModel } from '../../models/login-model.model';


import { AuthService } from '../services/auth.service';
 
@Component({


  selector: 'app-login',


  templateUrl: './login.component.html',


  styleUrls: ['./login.component.css']


})


export class LoginComponent {
 
  loginUser: LoginModel = {


    Username: '',


    Password: ''


  };
 
  constructor(


    private authService: AuthService,


    private router: Router


  ) { }
 
  login(form: NgForm) {
 
    if (form.invalid) {


      return;


    }
 
    this.authService.login(this.loginUser).subscribe((response: any) => {
 
      if (response.user.Role === 'Admin') {


        this.router.navigate(['/admin']);


      }


      else {


        this.router.navigate(['/organizer']);


      }
 
    });
 
  }
 
}
 
=======================================================
 
login.html-
<form #loginForm="ngForm" (ngSubmit)="login(loginForm)">
 
    <h2>Login</h2>
 
    <label>Username *</label>
 
    <input type="text" id="username" name="username" required [(ngModel)]="loginUser.Username" #username="ngModel">
 
    <div class="error-message">


        Username is required


        Password is required
</div>
 
    <label>Password *</label>
 
    <input type="password" id="password" name="password" required [(ngModel)]="loginUser.Password" #password="ngModel">
 
    <div class="error-message">


        Password is required
</div>
 
    <button type="submit" id="submit" [disabled]="loginForm.invalid">
 
        LOGIN
 
    </button>
 
</form>


====================================================


registration.html-
 
<form #regForm="ngForm" (ngSubmit)="register(regForm)">
 
    <h2>REGISTRATION</h2>
 
    <label>Username *</label>
 
    <input type="text" id="username" name="username" required [(ngModel)]="user.Username" #username="ngModel">
 
    <div class="error-message">


        Username is required


        Password is required


        Confirm Password is required


        Passwords do not match
</div>
 
    <label>Password *</label>
 
    <input type="password" id="password" name="password" required


        pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$" [(ngModel)]="user.Password" #password="ngModel">
 
    <div class="error-message">


        Password is required
</div>
 
    <label>Confirm Password *</label>
 
    <input type="password" id="confirmPassword" name="confirmPassword" required [(ngModel)]="confirmPassword"


        #confirm="ngModel">
 
    <div class="error-message">


        Confirm Password is required
</div>
 
    <div class="error-message">
 
        Passwords do not match
 
    </div>
 
    <label>Role *</label>
 
    <select id="role" name="role" required [(ngModel)]="user.Role" #role="ngModel">
 
        <option value="">Select a role</option>
<option value="Admin">Admin</option>
<option value="Organizer">Organizer</option>
 
    </select>
 
    <div class="error-message">


        Role is required
</div>
 
    <button id="submit" type="submit" [disabled]="regForm.invalid || user.Password != confirmPassword">
 
        REGISTER
 
    </button>
 
</form>


====================================================================
 
reg.ts-
 
import { Component } from '@angular/core';


import { NgForm } from '@angular/forms';


import { Router } from '@angular/router';


import { User } from '../../models/user.model';


import { AuthService } from '../services/auth.service';
 
@Component({


  selector: 'app-registration',


  templateUrl: './registration.component.html',


  styleUrls: ['./registration.component.css']


})


export class RegistrationComponent {
 
  user: User = {


    Username: '',


    Password: '',


    Role: ''


  };
 
  confirmPassword: string = '';
 
  passwordPattern =


    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$/;
 
  constructor(


    private authService: AuthService,


    private router: Router


  ) { }
 
  register(form: NgForm) {
 
    if (form.invalid || this.user.Password !== this.confirmPassword) {


      return;


    }
 
    this.authService.register(this.user).subscribe(() => {


      this.router.navigate(['/login']);


    });


  }


}


======================================================
 
 
========================


doctor.html-
 
<h2>Doctors</h2>
 
<table border="1">
<tr>
<th>Doctor Id</th>
<th>Name</th>
<th>Specialization</th>
<th>Consultation Fee</th>
 
    </tr>
<tr *ngFor="let doctor of doctors">
<td>{{doctor.DoctorId}}</td>
<td>{{doctor.Name}}</td>
<td>{{doctor.Specialization}}</td>
<td>{{doctor.ConsultationFee}}</td>
</tr>
</table>
 
 
==========================


doctor.ts-
 
import { Component, OnInit } from '@angular/core';


import { Doctor } from '../../models/doctor.model';


import { DoctorService } from '../services/doctor.service';
 
@Component({


  selector: 'app-doctor',


  templateUrl: './doctor.component.html',


  styleUrls: ['./doctor.component.css']


})


export class DoctorComponent implements OnInit {
 
  doctors: Doctor[] = [];
 
  constructor(private doctorService: DoctorService) { }
 
  ngOnInit(): void {


    this.getDoctors();


  }
 
  getDoctors(): void {


    this.doctorService.getDoctors().subscribe(data => {


      this.doctors = data;


    });


  }
 
}
 
 
 
======================
 
patient.html-
 
<h2>Patients</h2>
 
<table border="1">
<tr>
<th>Patient Id</th>
<th>Name</th>
<th>Age</th>
<th>Condition</th>
<th>Appointment Date</th>
<th>Doctor Id</th>
 
    </tr>
<tr *ngFor="let patient of patients">
<td>{{patient.PatientId}}</td>
<td>{{patient.Name}}</td>
<td>{{patient.Age}}</td>
<td>{{patient.AppointmentDate}}</td>
<td>{{patient.DoctorId}}</td>
</tr>
</table>
 
 
=====================


patient.ts-
 
import { Component, OnInit } from '@angular/core';


import { Patient } from '../../models/patient.model';


import { PatientService } from '../services/patient.service';
 
@Component({


  selector: 'app-patient',


  templateUrl: './patient.component.html',


  styleUrls: ['./patient.component.css']


})


export class PatientComponent implements OnInit {
 
  patients: Patient[] = [];
 
  constructor(private patientService: PatientService) { }
 
  ngOnInit(): void {


    this.getPatients();


  }
 
  getPatients(): void {


    this.patientService.getPatients().subscribe(data => {


      this.patients = data;


    });


  }
 
}
 
 
==================

 
create patient.html


========================================================================
 
<form #patientForm="ngForm" (ngSubmit)="createPatient(patientForm)">
<h2>Create Patient</h2>
<label>Name</label>
<input type="text" id="patientName" name="patientName" required minlength="2" maxlength="100"


        [(ngModel)]="newPatient.Name" #patientName="ngModel">
<div class="error-message">


        Name is required


        Age is required
</div>
<label>Age</label>
<input type="number" id="patientAge" name="patientAge" required [(ngModel)]="newPatient.Age" #patientAge="ngModel">
<div class="error-message">


        Age is required
</div>
<label>Condition</label>
<input type="text" id="patientCondition" name="patientCondition" required [(ngModel)]="newPatient.Condition"


        #patientCondition="ngModel">
<div class="error-message">


        Condition is required
</div>
<label>Appointment Date</label>
<input type="date" id="appointmentDate" name="appointmentDate" required [(ngModel)]="newPatient.AppointmentDate"


        #appointmentDate="ngModel">
<div class="error-message">


        Appointment Date is required
</div>
<button type="submit" id="submit" [disabled]="patientForm.invalid">


        Create Patient
</button>
</form>
 
=======================================================================


create-pt.comp.ts-
 
import { Component } from '@angular/core';


import { Router } from '@angular/router';


import { NgForm } from '@angular/forms';


import { Patient } from '../../../models/patient.model';


import { PatientService } from '../../services/patient.service';


@Component({


  selector: 'app-create-patient',


  templateUrl: './create-patient.component.html',


  styleUrls: ['./create-patient.component.css']


})


export class CreatePatientComponent {


  newPatient: Patient = {


    Name: '',


    Age: null,


    Condition: '',


    AppointmentDate: null,


    DoctorId: 0


  };


  constructor(


    private patientService: PatientService,


    private router: Router


  ) { }


  createPatient(form: NgForm) {


    if (form.invalid) {


      return;


    }


    this.patientService.createPatient(this.newPatient).subscribe(() => {


      this.newPatient = {


        Name: '',


        Age: null,


        Condition: '',


        AppointmentDate: null,


        DoctorId: 0


      };


      form.resetForm();


      this.router.navigate(['/admin']);


    });


  }


}
 
 
===========================================================
 
create doctor.html


==================
 
<form #doctorForm="ngForm" (ngSubmit)="createDoctor()">
<h2>Create Doctor</h2>
<label>Doctor Name</label>
<input


        type="text"


        id="doctorName"


        name="doctorName"


        required


        [(ngModel)]="newDoctor.Name"


        #doctorName="ngModel">
<div class="error-message"


        *ngIf="doctorName.invalid && (doctorName.touched || doctorName.dirty)">
<div *ngIf="doctorName.errors?.['required']">


            Doctor Name is required</div>
</div>
<label>Specialization</label>
<input


        type="text"


        id="specialization"


        name="specialization"


        required


        [(ngModel)]="newDoctor.Specialization"


        #specialization="ngModel">
<div class="error-message"


        *ngIf="specialization.invalid && (specialization.touched || specialization.dirty)">
<div *ngIf="specialization.errors?.['required']">


            Specialization is required</div>
</div>
<label>Consultation Fee</label>
<input


        type="number"


        id="consultationFee"


        name="consultationFee"


        required


        [(ngModel)]="newDoctor.ConsultationFee"


        #consultationFee="ngModel">
<div class="error-message"


        *ngIf="consultationFee.invalid && (consultationFee.touched || consultationFee.dirty)">
<div *ngIf="consultationFee.errors?.['required']">


            Consultation Fee is required</div>
</div>
<div id="status">


        {{ feeStatus }}
</div>
<button


        type="submit"


        id="submit"


        [disabled]="doctorForm.invalid">


    Create Doctor
</button>
</form>
 
======================================================
 
create-doc.comp.ts=
 
import { Component } from '@angular/core';


import { Router } from '@angular/router';


import { Doctor } from '../../../models/doctor.model';


import { DoctorService } from '../../services/doctor.service';
 
@Component({


  selector: 'app-create-doctor',


  templateUrl: './create-doctor.component.html',


  styleUrls: ['./create-doctor.component.css']


})


export class CreateDoctorComponent {


  newDoctor: Doctor = {


    Name: '',


    Specialization: '',


    ConsultationFee: 0


  };


  constructor(


    private doctorService: DoctorService,


    private router: Router


  ) { }


  get feeStatus(): string {


    if (this.newDoctor.ConsultationFee == null || this.newDoctor.ConsultationFee == 0) {


      return 'Low Fee';


    }


    if (this.newDoctor.ConsultationFee < 100) {


      return 'Insufficient';


    }


    if (this.newDoctor.ConsultationFee < 300) {


      return 'Adequate';


    }


    return 'Good Fee';


  }


  createDoctor() {


    this.doctorService.createDoctor(this.newDoctor).subscribe(() => {


      this.router.navigate(['/admin']);


    });


  }


}
 
=======================================================
 
login.ts-
 
import { Component } from '@angular/core';


import { NgForm } from '@angular/forms';


import { Router } from '@angular/router';


import { LoginModel } from '../../models/login-model.model';


import { AuthService } from '../services/auth.service';
 
@Component({


  selector: 'app-login',


  templateUrl: './login.component.html',


  styleUrls: ['./login.component.css']


})


export class LoginComponent {
 
  loginUser: LoginModel = {


    Username: '',


    Password: ''


  };
 
  constructor(


    private authService: AuthService,


    private router: Router


  ) { }
 
  login(form: NgForm) {
 
    if (form.invalid) {


      return;


    }
 
    this.authService.login(this.loginUser).subscribe((response: any) => {
 
      if (response.user.Role === 'Admin') {


        this.router.navigate(['/admin']);


      }


      else {


        this.router.navigate(['/organizer']);


      }
 
    });
 
  }
 
}
 
=======================================================
 
login.html-
<form #loginForm="ngForm" (ngSubmit)="login(loginForm)">
 
    <h2>Login</h2>
 
    <label>Username *</label>
 
    <input type="text" id="username" name="username" required [(ngModel)]="loginUser.Username" #username="ngModel">
 
    <div class="error-message">


        Username is required


        Password is required
</div>
 
    <label>Password *</label>
 
    <input type="password" id="password" name="password" required [(ngModel)]="loginUser.Password" #password="ngModel">
 
    <div class="error-message">


        Password is required
</div>
 
    <button type="submit" id="submit" [disabled]="loginForm.invalid">
 
        LOGIN
 
    </button>
 
</form>


====================================================


registration.html-
 
<form #regForm="ngForm" (ngSubmit)="register(regForm)">
 
    <h2>REGISTRATION</h2>
 
    <label>Username *</label>
 
    <input type="text" id="username" name="username" required [(ngModel)]="user.Username" #username="ngModel">
 
    <div class="error-message">


        Username is required


        Password is required


        Confirm Password is required


        Passwords do not match
</div>
 
    <label>Password *</label>
 
    <input type="password" id="password" name="password" required


        pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$" [(ngModel)]="user.Password" #password="ngModel">
 
    <div class="error-message">


        Password is required
</div>
 
    <label>Confirm Password *</label>
 
    <input type="password" id="confirmPassword" name="confirmPassword" required [(ngModel)]="confirmPassword"


        #confirm="ngModel">
 
    <div class="error-message">


        Confirm Password is required
</div>
 
    <div class="error-message">
 
        Passwords do not match
 
    </div>
 
    <label>Role *</label>
 
    <select id="role" name="role" required [(ngModel)]="user.Role" #role="ngModel">
 
        <option value="">Select a role</option>
<option value="Admin">Admin</option>
<option value="Organizer">Organizer</option>
 
    </select>
 
    <div class="error-message">


        Role is required
</div>
 
    <button id="submit" type="submit" [disabled]="regForm.invalid || user.Password != confirmPassword">
 
        REGISTER
 
    </button>
 
</form>


====================================================================
 
reg.ts-
 
import { Component } from '@angular/core';


import { NgForm } from '@angular/forms';


import { Router } from '@angular/router';


import { User } from '../../models/user.model';


import { AuthService } from '../services/auth.service';
 
@Component({


  selector: 'app-registration',


  templateUrl: './registration.component.html',


  styleUrls: ['./registration.component.css']


})


export class RegistrationComponent {
 
  user: User = {


    Username: '',


    Password: '',


    Role: ''


  };
 
  confirmPassword: string = '';
 
  passwordPattern =


    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$/;
 
  constructor(


    private authService: AuthService,


    private router: Router


  ) { }
 
  register(form: NgForm) {
 
    if (form.invalid || this.user.Password !== this.confirmPassword) {


      return;


    }
 
    this.authService.register(this.user).subscribe(() => {


      this.router.navigate(['/login']);


    });


  }


}


======================================================
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
import { Injectable } from '@angular/core';


import { HttpClient } from '@angular/common/http';


import { Observable } from 'rxjs';


import { Patient } from '../../models/patient.model';
 
@Injectable({


  providedIn: 'root'


})


export class PatientService {
 
  private baseUrl = 'https://8080-***************.premiumproject.examly.io/api/Patient';
 
  constructor(private http: HttpClient) { }
 
  getPatients(): Observable<Patient[]> {


    return this.http.get<Patient[]>(`${this.baseUrl}/GetPatients`);


  }
 
  createPatient(patient: Patient): Observable <Patient> {


    return this.http.post <Patient>(`${this.baseUrl}/PostPatient`, patient);


  }
 
  updatePatient(patientId: number, patient: Patient): Observable <Patient> {


    return this.http.put <Patient> (`${this.baseUrl}/PutPatient/${patientId}`, patient);


  }
 
  deletePatient(patientId: number): Observable <void> {


    return this.http.delete <void>(`${this.baseUrl}/DeletePatient/${patientId}`);


  }
 
}
 
 
 
 
 
 
 
 
import { Injectable } from '@angular/core';


import { HttpClient } from '@angular/common/http';


import { Observable } from 'rxjs';


import { Doctor } from '../../models/doctor.model';
 
@Injectable({


  providedIn: 'root'


})


export class DoctorService {
 
  private baseUrl = 'https://8080-***************.premiumproject.examly.io/api/Doctor';
 
  constructor(private http: HttpClient) { }
 
  getDoctors(): Observable<Doctor[]> {


    return this.http.get<Doctor[]>(`${this.baseUrl}/GetDoctors`);


  }
 
  createDoctor(doctor: Doctor): Observable <Doctor> {


    return this.http.post<Doctor>(`${this.baseUrl}/PostDoctor`, doctor);


  }
 
  updateDoctor(doctorId: number, doctor: Doctor): Observable<Doctor> {


    return this.http.put<Doctor>(`${this.baseUrl}/PutDoctor/${doctorId}`, doctor);


  }
 
  deleteDoctor(doctorId: number): Observable<void> {


    return this.http.delete <void>(`${this.baseUrl}/DeleteDoctor/${doctorId}`);


  }
 
}
 
 
 
 
 
 
 
 
 
import { Injectable } from '@angular/core';


import { HttpClient } from '@angular/common/http';


import { Observable, BehaviorSubject, of } from 'rxjs';


import { catchError, tap } from 'rxjs/operators';


import { User } from '../../models/user.model';


import { LoginModel } from '../../models/login-model.model';
 
@Injectable({


  providedIn: 'root'


})


export class AuthService {
 
  public baseUrl = 'https://8080-***************.premiumproject.examly.io/api/users';
 
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
 
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
 
  constructor(private http: HttpClient) { }
 
  register(newUser: User): Observable<User> {


    return this.http.post<User>(`${this.baseUrl}/register`, newUser).pipe(


      tap(user => this.storeUserData(user)),


      catchError(this.handleError <User>('register'))


    );


  }
 
  login(loginUser: LoginModel): Observable<any> {


    return this.http.post<any>(`${this.baseUrl}/login`, loginUser).pipe(


      tap(response => {


        localStorage.setItem('token', 'loggedin');


        this.storeUserData(response.user);


        this.updateAuthenticationStatus(true);


      }),


      catchError(this.handleError<any>('login'))


    );


  }
 
  logout(): void {


    localStorage.clear();


    this.updateAuthenticationStatus(false);


  }
 
  isLoggedIn(): boolean {


    return localStorage.getItem('token') !== null;


  }
 
  isAdmin(): boolean {


    return localStorage.getItem('role') === 'Admin';


  }
 
  isOrganizer(): boolean {


    return localStorage.getItem('role') === 'Organizer';


  }
 
  updateAuthenticationStatus(isAuthenticated: boolean): void {


    this.isAuthenticatedSubject.next(isAuthenticated);


  }
 
  private storeUserData(user: any): void {


    if (user) {


      localStorage.setItem('role', user.Role);


    }


  }
 
  private handleError <T>(operation = 'operation', result?: T) {


    return () => {


      console.error(`${operation} failed`);


      return of(result as T);


    };


  }
 
}
 
Inc-3, w7d1- 
 
admin.ts-
 
import { Component, OnInit } from '@angular/core';


import { Doctor } from '../../models/doctor.model';


import { Patient } from '../../models/patient.model';


import { DoctorService } from '../services/doctor.service';


import { PatientService } from '../services/patient.service';
 
@Component({


  selector: 'app-admin',


  templateUrl: './admin.component.html',


  styleUrls: ['./admin.component.css']


})


export class AdminComponent implements OnInit {
 
  doctors: Doctor[] = [];


  patients: Patient[] = [];
 
  editedDoctor: Doctor | null = null;


  editedPatient: Patient | null = null;
 
  constructor(


    private doctorService: DoctorService,


    private patientService: PatientService


  ) { }
 
  ngOnInit(): void {


    this.getDoctors();


    this.getPatients();


  }
 
  getDoctors(): void {


    this.doctorService.getDoctors().subscribe(data => {


      this.doctors = data;


    });


  }
 
  editDoctor(doctor: Doctor): void {


    this.editedDoctor = { ...doctor };


  }
 
  saveEditedDoctor(): void {


    if (this.editedDoctor) {


      this.doctorService.updateDoctor(this.editedDoctor.DoctorId, this.editedDoctor).subscribe(() => {


        this.getDoctors();


        this.editedDoctor = null;


      });


    }


  }
 
  cancelEditDoctor(): void {


    this.editedDoctor = null;


  }
 
  deleteDoctor(doctorId: number): void {


    this.doctorService.deleteDoctor(doctorId).subscribe(() => {


      this.getDoctors();


    });


  }
 
  getPatients(): void {


    this.patientService.getPatients().subscribe(data => {


      this.patients = data;


    });


  }
 
  editPatient(patient: Patient): void {


    this.editedPatient = { ...patient };


  }
 
  saveEditedPatient(): void {


    if (this.editedPatient) {


      this.patientService.updatePatient(this.editedPatient.PatientId, this.editedPatient).subscribe(() => {


        this.getPatients();


        this.editedPatient = null;


      });


    }


  }
 
  cancelEditPatient(): void {


    this.editedPatient = null;


  }
 
  deletePatient(patientId: number): void {


    this.patientService.deletePatient(patientId).subscribe(() => {


      this.getPatients();


    });


  }
 
}
 
 
admin.html-
 
import { Component, OnInit } from '@angular/core';


import { Doctor } from '../../models/doctor.model';


import { Patient } from '../../models/patient.model';


import { DoctorService } from '../services/doctor.service';


import { PatientService } from '../services/patient.service';
 
@Component({


  selector: 'app-admin',


  templateUrl: './admin.component.html',


  styleUrls: ['./admin.component.css']


})


export class AdminComponent implements OnInit {
 
  doctors: Doctor[] = [];


  patients: Patient[] = [];
 
  editedDoctor: Doctor | null = null;


  editedPatient: Patient | null = null;
 
  constructor(


    private doctorService: DoctorService,


    private patientService: PatientService


  ) { }
 
  ngOnInit(): void {


    this.getDoctors();


    this.getPatients();


  }
 
  getDoctors(): void {


    this.doctorService.getDoctors().subscribe(data => {


      this.doctors = data;


    });


  }
 
  editDoctor(doctor: Doctor): void {


    this.editedDoctor = { ...doctor };


  }
 
  saveEditedDoctor(): void {


    if (this.editedDoctor) {


      this.doctorService.updateDoctor(this.editedDoctor.DoctorId, this.editedDoctor).subscribe(() => {


        this.getDoctors();


        this.editedDoctor = null;


      });


    }


  }
 
  cancelEditDoctor(): void {


    this.editedDoctor = null;


  }
 
  deleteDoctor(doctorId: number): void {


    this.doctorService.deleteDoctor(doctorId).subscribe(() => {


      this.getDoctors();


    });


  }
 
  getPatients(): void {


    this.patientService.getPatients().subscribe(data => {


      this.patients = data;


    });


  }
 
  editPatient(patient: Patient): void {


    this.editedPatient = { ...patient };


  }
 
  saveEditedPatient(): void {


    if (this.editedPatient) {


      this.patientService.updatePatient(this.editedPatient.PatientId, this.editedPatient).subscribe(() => {


        this.getPatients();


        this.editedPatient = null;


      });


    }


  }
 
  cancelEditPatient(): void {


    this.editedPatient = null;


  }
 
  deletePatient(patientId: number): void {


    this.patientService.deletePatient(patientId).subscribe(() => {


      this.getPatients();


    });


  }
 
}
 
 
 
authgaurd/auth.gaurd.ts-
 
import { Injectable } from '@angular/core';


import { CanActivate, Router } from '@angular/router';


import { AuthService } from '../services/auth.service';
 
@Injectable({


  providedIn: 'root'


})


export class AuthGuard implements CanActivate{


  constructor(


    private authService: AuthService,


    private router: Router


  ){}


  canActivate(): boolean{


    if (this.authService.isLoggedIn()){


      return true;


    }


    this.router.navigate(['/Login']);


    return false;


  }
 
}
 
 
 
navbar.html-
 
<nav>
 
    <a routerLink="/">Home</a>
 
    <a


        *ngIf="authService.isOrganizer()"


        routerLink="/organizer">


        Organizer</a>
 
    <a


        *ngIf="authService.isAdmin()"


        routerLink="/admin">


        Admin</a>
 
    <a


        *ngIf="authService.isAdmin()"


        routerLink="/admin/createDoctor">


        Create Doctor</a>
 
    <a


        *ngIf="authService.isAdmin()"


        routerLink="/admin/createPatient">


        Create Patient</a>
 
    <a


        *ngIf="!authService.isLoggedIn()"


        routerLink="/signup">


        Register</a>
 
    <a


        *ngIf="!authService.isLoggedIn()"


        routerLink="/login">


        Login</a>
 
    <a


        *ngIf="authService.isLoggedIn()"


        (click)="logout()">


        Logout</a>
 
</nav>
 
 
 
navbar.ts-
 
 
import { Component } from '@angular/core';


import { Router } from '@angular/router';


import { AuthService } from '../services/auth.service';
 
@Component({


  selector: 'app-navbar',


  templateUrl: './navbar.component.html',


  styleUrls: ['./navbar.component.css']


})


export class NavbarComponent {
 
  constructor(


    public authService: AuthService,


    private router: Router


  ) { }
 
  logout() {


    this.authService.logout();


    this.router.navigate(['/login']);


  }
 
}
 
 
 
app.routing.module.ts-
 
 
import { NgModule } from '@angular/core';


import { RouterModule, Routes } from '@angular/router';
 
import { HomeComponent } from './home/home.component';


import { AdminComponent } from './admin/admin.component';


import { OrganizerComponent } from './organizer/organizer.component';


import { LoginComponent } from './login/login.component';


import { RegistrationComponent } from './registration/registration.component';


import { CreateDoctorComponent } from './doctor/create-doctor/create-doctor.component';


import { CreatePatientComponent } from './patient/create-patient/create-patient.component';


import { ErrorComponent } from './error/error.component';
 
import { AuthGuard } from './authguard/auth.guard';
 
const routes: Routes = [


  {


    path: '',


    component: HomeComponent


  },


  {


    path: 'admin',


    component: AdminComponent,


    canActivate: [AuthGuard]


  },


  {


    path: 'admin/createPatient',


    component: CreatePatientComponent,


    canActivate: [AuthGuard]


  },


  {


    path: 'admin/createDoctor',


    component: CreateDoctorComponent,


    canActivate: [AuthGuard]


  },


  {


    path: 'organizer',


    component: OrganizerComponent,


    canActivate: [AuthGuard]


  },


  {


    path: 'login',


    component: LoginComponent


  },


  {


    path: 'signup',


    component: RegistrationComponent


  },


  {


    path: 'error',


    component: ErrorComponent


  },


  {


    path: '**',


    redirectTo: 'error'


  }


];
 
@NgModule({


  imports: [RouterModule.forRoot(routes)],


  exports: [RouterModule]


})


export class AppRoutingModule { }
 
 
 
 
Patient.ts-


import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';


import { Patient } from '../../models/patient.model';


@Component({


selector: 'app-patient',


templateUrl: './patient.component.html',


styleUrls: ['./patient.component.css']


})


export class PatientComponent implements OnInit {


@Input() patients: Patient[] = [];


@Input() editedPatient!: Patient | null;


@Output() editPatientEvent = new EventEmitter<Patient>();


@Output() saveEditedPatientEvent = new EventEmitter<void>();


@Output() cancelEditPatientEvent = new EventEmitter<void>();


@Output() deletePatientEvent = new EventEmitter<number>();


constructor() { }


ngOnInit(): void {


}


onEditPatient(patient: Patient): void {


this.editPatientEvent.emit(patient);


}


onSaveEditedPatient(): void {


this.saveEditedPatientEvent.emit();


}


onCancelEditPatient(): void {


this.cancelEditPatientEvent.emit();


}


onDeletePatient(patientId: number): void {


this.deletePatientEvent.emit(patientId);


}


onAppointmentDateChange(date: string): void {


if (this.editedPatient) {


this.editedPatient.AppointmentDate = date;


}


}


}



Patient.html-
<h2>Patients</h2>
<table border="1">
<tr>
<th>Name</th>
<th>Age</th>
<th>Condition</th>
<th>Appointment Date</th>
<th>Actions</th>
</tr>
<tr *ngFor="let patient of patients">
<td>{{patient.Name}}</td>
<td>{{patient.Age}}</td>
<td>{{patient.Condition}}</td>
<td>{{patient.AppointmentDate}}</td>
<td>
<button (click)="onEditPatient(patient)">Edit</button>
<button (click)="onDeletePatient(patient.PatientId!)">Delete</button>
</td>
</tr>
</table>
<div *ngIf="editedPatient">
<h3>Edit Patient</h3>
<label>Name</label>
<input type="text" [(ngModel)]="editedPatient.Name">
<br><br>
<label>Age</label>
<input type="number" [(ngModel)]="editedPatient.Age">
<br><br>
<label>Condition</label>
<input type="text" [(ngModel)]="editedPatient.Condition">
<br><br>
<label>Appointment Date</label>
<input


        type="date"


        [ngModel]="editedPatient.AppointmentDate"


        (ngModelChange)="onAppointmentDateChange($event)">
<br><br>
<button (click)="onSaveEditedPatient()">Save</button>
<button (click)="onCancelEditPatient()">Cancel</button>
</div>
 
 
Doc.comp.ts-


import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';


import { Doctor } from '../../models/doctor.model';


@Component({


selector: 'app-doctor',


templateUrl: './doctor.component.html',


styleUrls: ['./doctor.component.css']


})


export class DoctorComponent implements OnInit {


@Input() doctors: Doctor[] = [];


@Input() editedDoctor!: Doctor | null;


@Output() editDoctorEvent = new EventEmitter<Doctor>();


@Output() saveEditedDoctorEvent = new EventEmitter<void>();


@Output() cancelEditDoctorEvent = new EventEmitter<void>();


@Output() deleteDoctorEvent = new EventEmitter<number>();


constructor() { }


ngOnInit(): void {


}


onEditDoctor(doctor: Doctor): void {


this.editDoctorEvent.emit(doctor);


}


onSaveEditedDoctor(): void {


this.saveEditedDoctorEvent.emit();


}


onCancelEditDoctor(): void {


this.cancelEditDoctorEvent.emit();


}


onDeleteDoctor(doctorId: number): void {


this.deleteDoctorEvent.emit(doctorId);


}


}



Doc.html-

<h2>Doctors</h2>
<table border="1">
<tr>
<th>Doctor Name</th>
<th>Specialization</th>
<th>Consultation Fee</th>
<th>Actions</th>
</tr>
<tr *ngFor="let doctor of doctors">
<td>{{doctor.Name}}</td>
<td>{{doctor.Specialization}}</td>
<td>{{doctor.ConsultationFee}}</td>
<td>
<button (click)="onEditDoctor(doctor)">Edit</button>
<button (click)="onDeleteDoctor(doctor.DoctorId!)">Delete</button>
</td>
</tr>
</table>
<div *ngIf="editedDoctor">
<h3>Edit Doctor</h3>
<label>Doctor Name</label>
<input type="text" [(ngModel)]="editedDoctor.Name">
<br><br>
<label>Specialization</label>
<input type="text" [(ngModel)]="editedDoctor.Specialization">
<br><br>
<label>Consultation Fee</label>
<input type="number" [(ngModel)]="editedDoctor.ConsultationFee">
<br><br>
<button (click)="onSaveEditedDoctor()">Save</button>
<button (click)="onCancelEditDoctor()">Cancel</button>
</div>

 
<h2>Admin Panel</h2>
 
<app-doctor [doctors]="doctors" [editedDoctor]="editedDoctor" (editDoctorEvent)="editDoctor($event)"
    (saveEditedDoctorEvent)="saveEditedDoctor()" (cancelEditDoctorEvent)="cancelEditDoctor()"
    (deleteDoctorEvent)="deleteDoctor($event)">
</app-doctor>
 
<br><br>
 
<app-patient [patients]="patients" [editedPatient]="editedPatient" (editPatientEvent)="editPatient($event)"
    (saveEditedPatientEvent)="saveEditedPatient()" (cancelEditPatientEvent)="cancelEditPatient()"
    (deletePatientEvent)="deletePatient($event)">
</app-patient>
 
