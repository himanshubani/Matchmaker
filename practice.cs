Doc-patient proj->
-----------------------

	dotnetapp
===================================

     Models
----------------

ApplicationDbContext----------------->


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
 
//////////


Doctor---------->
 
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
 
////////
LoginModel------------>
 
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
 
/////////
Patient----------->
 
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
 
////////
User------------>
 
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
 
////////

	Controllers
-------------------------------

DoctorController---------->
 
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
 
 
 
//////////

patientcontroller---------->

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
 
/////////

UserController-------------->
 
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
 
//////////

program.cs------> 
 
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

builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlServer("User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=false;Encrypt=false;"));

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






//////////////////////////////////////////////////////////////////////////////////////////



	Angularapp
=================================



Admin-------->


admin.css--

:host {
    display: block;
    padding: 25px;
}

h2 {
    text-align: center;
    color: #0d6efd;
    margin-bottom: 30px;
}

app-doctor,
app-patient {
    display: block;
    margin-bottom: 40px;
}


admin.html-


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


///////////

authguard---------->

auth.guard.ts-

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

////////////////////

Doctor------------>

doctor.css-

h2 {
    color: #0d6efd;
    margin-bottom: 20px;
}

table {
    width: 100%;
    border-collapse: collapse;
    background: white;
    margin-bottom: 25px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

th {
    background-color: #0d6efd;
    color: white;
    padding: 12px;
    text-align: center;
}

td {
    padding: 12px;
    text-align: center;
    border: 1px solid #ddd;
}

tr:nth-child(even) {
    background-color: #f8f9fa;
}

tr:hover {
    background-color: #eef5ff;
}

button {
    padding: 8px 14px;
    margin: 3px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-weight: 600;
}

button:first-child {
    background-color: #ffc107;
    color: black;
}

button:last-child {
    background-color: #dc3545;
    color: white;
}

button:hover {
    opacity: 0.9;
}

div[ng-reflect-ng-if] {
    margin-top: 20px;
}

h3 {
    color: #0d6efd;
    margin-bottom: 15px;
}

label {
    font-weight: 600;
}

input {
    padding: 8px;
    width: 250px;
    border: 1px solid #ccc;
    border-radius: 5px;
}


doctor.html---

<h2>Doctors</h2>

<table border="1">
    <tr>
        <th>Doctor Name</th>
        <th>Specialization</th>
        <th>Consultation Fee</th>
        <th>Actions</th>
    </tr>
    <tr *ngFor="let doctor of doctors">
        <td>{{ doctor.Name }}</td>
        <td>{{ doctor.Specialization }}</td>
        <td>{{ doctor.ConsultationFee }}</td>
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

doctor.ts--

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


create-doctor------------>


.css--

form {
    max-width: 600px;
    margin: 30px auto;
    background: white;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0px 2px 10px rgba(0,0,0,0.15);
}

h2 {
    text-align: center;
    color: #0d6efd;
    margin-bottom: 25px;
}

label {
    display: block;
    font-weight: 600;
    margin-top: 15px;
}

input {
    width: 100%;
    padding: 10px;
    margin-top: 6px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

#status {
    margin-top: 15px;
    color: green;
    font-weight: bold;
}

button {
    padding: 10px 20px;
    margin-top: 20px;
    border: none;
    color: white;
    background-color: #0d6efd;
    border-radius: 5px;
    cursor: pointer;
}

button:disabled {
    background-color: gray;
}

.error-message {
    color: red;
    font-size: 13px;
}

.html--

<form #doctorForm="ngForm" (ngSubmit)="createDoctor()">
    <h2>Create Doctor</h2>
    <label>Doctor Name</label>
    <input type="text" id="doctorName" name="doctorName" required [(ngModel)]="newDoctor.Name" #doctorName="ngModel">
    <div class="error-message" *ngIf="doctorName.invalid && (doctorName.touched || doctorName.dirty)">
        <div *ngIf="doctorName.errors?.['required']">
            Doctor Name is required</div>
    </div>
    <label>Specialization</label>
    <input type="text" id="specialization" name="specialization" required [(ngModel)]="newDoctor.Specialization"
        #specialization="ngModel">
    <div class="error-message" *ngIf="specialization.invalid && (specialization.touched || specialization.dirty)">
        <div *ngIf="specialization.errors?.['required']">
            Specialization is required</div>
    </div>
    <label>Consultation Fee</label>
    <input type="number" id="consultationFee" name="consultationFee" required [(ngModel)]="newDoctor.ConsultationFee"
        #consultationFee="ngModel">
    <div class="error-message" *ngIf="consultationFee.invalid && (consultationFee.touched || consultationFee.dirty)">
        <div *ngIf="consultationFee.errors?.['required']">
            Consultation Fee is required</div>
    </div>
    <div id="status">
        {{ feeStatus }}
    </div>
    <button type="submit" id="submit" [disabled]="doctorForm.invalid">
        Create Doctor
    </button>
</form>

.ts--
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


//////////////////////////

Patient-------------->

patient.css-

h2 {
    color: #0d6efd;
    margin-bottom: 20px;
}

table {
    width: 100%;
    border-collapse: collapse;
    background: #ffffff;
    margin-bottom: 25px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

th {
    background-color: #0d6efd;
    color: white;
    padding: 12px;
    text-align: center;
    border: 1px solid #ddd;
}

td {
    padding: 12px;
    text-align: center;
    border: 1px solid #ddd;
}

tr:nth-child(even) {
    background-color: #f8f9fa;
}

tr:hover {
    background-color: #eef5ff;
}

button {
    padding: 8px 14px;
    margin: 3px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-weight: 600;
}

button:first-child {
    background-color: #ffc107;
    color: black;
}

button:last-child {
    background-color: #dc3545;
    color: white;
}

button:hover {
    opacity: 0.9;
}

h3 {
    color: #0d6efd;
    margin-top: 20px;
    margin-bottom: 15px;
}

label {
    font-weight: 600;
    display: inline-block;
    width: 140px;
}

input {
    padding: 8px;
    width: 250px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

div[ng-reflect-ng-if] {
    background: white;
    padding: 20px;
    margin-top: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}


patient.html--

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
        <td>{{patient.AppointmentDate | date: 'dd/MM/yyyy'}}</td>
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
    <input type="date" [ngModel]="editedPatient.AppointmentDate" (ngModelChange)="onAppointmentDateChange($event)">

    <br><br>

    <button (click)="onSaveEditedPatient()">Save</button>
    <button (click)="onCancelEditPatient()">Cancel</button>

</div>

patient.ts--

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
      this.editedPatient.AppointmentDate = new Date(date);
    }
  }

}


create-patient------------>

.css-

form {
    max-width: 600px;
    margin: 30px auto;
    background: white;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0px 2px 10px rgba(0,0,0,0.15);
}

h2 {
    text-align: center;
    color: #0d6efd;
    margin-bottom: 25px;
}

label {
    display: block;
    font-weight: 600;
    margin-top: 15px;
}

input {
    width: 100%;
    padding: 10px;
    margin-top: 6px;
    border-radius: 5px;
    border: 1px solid #ccc;
}

button {
    margin-top: 20px;
    padding: 10px 20px;
    background: #0d6efd;
    border: none;
    color: white;
    border-radius: 5px;
    cursor: pointer;
}

button:disabled {
    background: gray;
}

.error-message {
    color: red;
    font-size: 13px;
}

.html--

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

.ts--

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
    DoctorId: null
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
        DoctorId: null
      };
      form.resetForm();
      this.router.navigate(['/admin']);
    });
  }
}


///////////////////////////////////////

registration------------->

reg.css--


form {
    max-width: 500px;
    margin: 30px auto;
    background: #ffffff;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.15);
}

h2 {
    text-align: center;
    margin-bottom: 25px;
    color: #0d6efd;
}

label {
    display: block;
    margin-top: 15px;
    font-weight: 600;
}

input,
select {
    width: 100%;
    padding: 10px;
    margin-top: 5px;
    border: 1px solid #cccccc;
    border-radius: 5px;
}

button {
    width: 100%;
    margin-top: 25px;
    padding: 12px;
    border: none;
    border-radius: 5px;
    background-color: #0d6efd;
    color: white;
    font-weight: bold;
    cursor: pointer;
}

button:disabled {
    background-color: gray;
    cursor: not-allowed;
}

.error-message {
    color: red;
    font-size: 13px;
    margin-top: 5px;
} 


reg.html---------------------->
 
<div class="registration-container">
 
    <h2>REGISTRATION</h2>
 
    <form #regForm="ngForm" (ngSubmit)="register(regForm)">
 
        <label>Username *</label>
 
        <input type="text" id="username" name="username" [(ngModel)]="user.Username" required #userInp
            (input)="uDirty = true">
 
        <div class="error-message" *ngIf="uDirty && userInp.value === ''">
            Username is required</div>
 
        <label>Password *</label>
 
        <input type="password" id="password" name="password" [(ngModel)]="user.Password" required #passInp
            (input)="pDirty = true">
 
        <div class="error-message" *ngIf="pDirty && passInp.value === ''">
            Password is required</div>
 
        <label>Confirm Password *</label>
 
        <input type="password" id="confirmPassword" name="confirmPassword" [(ngModel)]="confirmPassword" required
            #confirmInp (input)="cDirty = true">
 
        <div class="error-message" *ngIf="cDirty && confirmInp.value === ''">
            Confirm Password is required
        </div>
 
        <div class="error-message"
            [style.display]="(cDirty && confirmInp.value !== '' && confirmPassword !== user.Password) ? 'block' : 'none'">
 
            Passwords do not match
 
        </div>
       
        <label>Role *</label>
 
        <select id="role" name="role" [(ngModel)]="user.Role" required #roleInp (change)="rDirty = true">
 
            <option value="">Select a role</option>
 
            <option *ngFor="let r of A" [value]="r">
                {{ r }}</option>
 
        </select>
 
        <div class="error-message" *ngIf="rDirty && roleInp.value === ''">
            Role is required</div>
 
        <button type="submit" id="submit" [disabled]="regForm.invalid || confirmPassword !== user.Password">
 
            REGISTER
 
        </button>
 
    </form>
 
</div>
 
reg.ts--------------->
 
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
 
  A: string[] = ['Admin', 'Organizer'];
 
  uDirty = false;
  pDirty = false;
  cDirty = false;
  rDirty = false;
 
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }
 
  register(form: NgForm): void {
 
    if (form.invalid || this.user.Password !== this.confirmPassword) {
      return;
    }
 
    this.authService.register(this.user).subscribe(() => {
      this.router.navigate(['/login']);
    });
 
  }
 
}



///////////////////////////////


Login--------------------->

login.css--
form {
    max-width: 450px;
    margin: 50px auto;
    background: white;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.15);
}

h2 {
    text-align: center;
    color: #0d6efd;
    margin-bottom: 25px;
}

label {
    font-weight: 600;
    display: block;
    margin-top: 15px;
}

input {
    width: 100%;
    padding: 10px;
    margin-top: 5px;
    border-radius: 5px;
    border: 1px solid #ccc;
}

button {
    width: 100%;
    margin-top: 25px;
    padding: 12px;
    background-color: #0d6efd;
    color: white;
    border: none;
    cursor: pointer;
    border-radius: 5px;
}

button:disabled {
    background-color: gray;
    cursor: not-allowed;
}

.error-message {
    color: red;
    font-size: 13px;
    margin-top: 5px;
}


reg.html---------------------->
 
<div class="registration-container">
 
    <h2>REGISTRATION</h2>
 
    <form #regForm="ngForm" (ngSubmit)="register(regForm)">
 
        <label>Username *</label>
 
        <input type="text" id="username" name="username" [(ngModel)]="user.Username" required #userInp
            (input)="uDirty = true">
 
        <div class="error-message" *ngIf="uDirty && userInp.value === ''">
            Username is required</div>
 
        <label>Password *</label>
 
        <input type="password" id="password" name="password" [(ngModel)]="user.Password" required #passInp
            (input)="pDirty = true">
 
        <div class="error-message" *ngIf="pDirty && passInp.value === ''">
            Password is required</div>
 
        <label>Confirm Password *</label>
 
        <input type="password" id="confirmPassword" name="confirmPassword" [(ngModel)]="confirmPassword" required
            #confirmInp (input)="cDirty = true">
 
        <div class="error-message" *ngIf="cDirty && confirmInp.value === ''">
            Confirm Password is required
        </div>
 
        <div class="error-message"
            [style.display]="(cDirty && confirmInp.value !== '' && confirmPassword !== user.Password) ? 'block' : 'none'">
 
            Passwords do not match
 
        </div>
       
        <label>Role *</label>
 
        <select id="role" name="role" [(ngModel)]="user.Role" required #roleInp (change)="rDirty = true">
 
            <option value="">Select a role</option>
 
            <option *ngFor="let r of A" [value]="r">
                {{ r }}</option>
 
        </select>
 
        <div class="error-message" *ngIf="rDirty && roleInp.value === ''">
            Role is required</div>
 
        <button type="submit" id="submit" [disabled]="regForm.invalid || confirmPassword !== user.Password">
 
            REGISTER
 
        </button>
 
    </form>
 
</div>
 
reg.ts--------------->
 
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
 
  A: string[] = ['Admin', 'Organizer'];
 
  uDirty = false;
  pDirty = false;
  cDirty = false;
  rDirty = false;
 
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }
 
  register(form: NgForm): void {
 
    if (form.invalid || this.user.Password !== this.confirmPassword) {
      return;
    }
 
    this.authService.register(this.user).subscribe(() => {
      this.router.navigate(['/login']);
    });
 
  }
 
}
 
login.html-
 
<form #loginForm="ngForm" (ngSubmit)="login(loginForm)">
 
    <h2>Login</h2>
 
    <label>Username *</label>
 
    <input type="text" id="username" name="username" required [(ngModel)]="loginUser.Username" #userInp
        (input)="uDirty = true">
 
    <div class="error-message" *ngIf="uDirty && userInp.value === ''">
        Username is required</div>
 
    <label>Password *</label>
 
    <input type="password" id="password" name="password" required [(ngModel)]="loginUser.Password" #passInp
        (input)="pDirty = true">
 
    <div class="error-message" *ngIf="pDirty && passInp.value === ''">
        Password is required</div>
 
    <button type="submit" id="submit" [disabled]="loginForm.invalid">
 
        LOGIN
 
    </button>
 
</form>
 
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
 
  uDirty = false;
  pDirty = false;
 
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
 
 
 
 /////////////////////////////////


Navbar------------>

navbar.css-

nav {
    background-color: #0d6efd;
    padding: 15px 30px;
    display: flex;
    gap: 25px;
    align-items: center;
    justify-content: center;
}

nav a {
    color: white;
    text-decoration: none;
    font-weight: 600;
    cursor: pointer;
    transition: 0.3s;
}

nav a:hover {
    color: #dbe8ff;
}

navbar.html--

<nav>

    <a routerLink="/">Home</a>

    <a *ngIf="authService.isOrganizer()" routerLink="/organizer">
        Organizer</a>

    <a *ngIf="authService.isAdmin()" routerLink="/admin">
        Admin</a>

    <a *ngIf="authService.isAdmin()" routerLink="/admin/createDoctor">
        Create Doctor</a>

    <a *ngIf="authService.isAdmin()" routerLink="/admin/createPatient">
        Create Patient</a>

    <a *ngIf="!authService.isLoggedIn()" routerLink="/signup">
        Register</a>

    <a *ngIf="!authService.isLoggedIn()" routerLink="/login">
        Login</a>

    <a *ngIf="authService.isLoggedIn()" (click)="logout()">
        Logout</a>

</nav>

navbar.ts--

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
////////////////////////////////////////


Organizer------------------>

org.css--

:host {
    display: block;
    padding: 25px;
}

h2 {
    text-align: center;
    color: #0d6efd;
    margin-bottom: 25px;
}

h3 {
    color: #0d6efd;
    margin-top: 25px;
    margin-bottom: 15px;
}

table {
    width: 100%;
    border-collapse: collapse;
    background-color: #ffffff;
    margin-bottom: 30px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

th {
    background-color: #0d6efd;
    color: white;
    padding: 12px;
    border: 1px solid #ddd;
    text-align: center;
}

td {
    padding: 10px;
    border: 1px solid #ddd;
    text-align: center;
}

tr:nth-child(even) {
    background-color: #f8f9fa;
}

tr:hover {
    background-color: #eef5ff;
}

button {
    padding: 8px 14px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-weight: 600;
    margin: 2px;
}

button:hover {
    opacity: 0.9;
}

.assign-btn {
    background-color: #198754;
    color: white;
}

.release-btn {
    background-color: #dc3545;
    color: white;
}

select {
    padding: 8px;
    width: 180px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

label {
    font-weight: 600;
}


org.html--

<h2>Doctor Patient Assignment Panel</h2>

<br><br>

<h3>Unassigned Patients</h3>

<br><br>

<p id="no_unassigned" *ngIf="unassignedPatients.length == 0">
    No Unassigned Patients
</p>

<br><br>

<table border="1">
    <thead>
        <tr>
            <th>Name</th>
            <th>Age</th>
            <th>Condition</th>
            <th>Assign Doctor</th>
            <th>Action</th>
        </tr>
    </thead>
    <tbody>
        <tr *ngFor="let patient of unassignedPatients">
            <td>{{ patient.Name }}</td>
            <td>{{ patient.Age }}</td>
            <td>{{ patient.Condition }}</td>

            <td>
                <select #doctorSelect>
                    <option *ngFor="let doctor of doctors" [value]="doctor.DoctorId">
                        {{ doctor.Name }}
                        </option>
                    
                </select>
                
            </td>

            <td>
                <button (click)="assignPatientToDoctor(patient, +doctorSelect.value)">
                    Assign To Doctor
                    </button>
                
            </td>
            
        </tr>

    </tbody>
</table>

<br><br>

<hr>
<h3>Doctors</h3>

<br><br>

<p id="no_doctors" *ngIf="doctors.length == 0">
    No Doctors Available
</p>

<br><br>

<table border="1" *ngIf="doctors.length > 0">
    <tr>
        <th>Doctor Name</th>
        <th>Specialization</th>
        <th>Consultation Fee</th>
        <th>Patients</th>
    </tr>
    <ng-container *ngFor="let doctor of doctors">
        <tr>
            <td>{{ doctor.Name }}</td>
            <td>{{ doctor.Specialization }}</td>
            <td>{{ doctor.ConsultationFee }}</td>

            <td>
                <div *ngFor="let patient of patients">
                    <span *ngIf="patient.DoctorId == doctor.DoctorId">
                        {{ patient.Name }}
                        <button (click)="releasePatientFromDoctor(patient)">
                            Release Patient
                            </button>
                        <br> 
                    </span>                      
                </div>
                
            </td>
        </tr>
    </ng-container>

</table>


org.ts---

import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../models/doctor.model';
import { Patient } from '../../models/patient.model';
import { DoctorService } from '../services/doctor.service';
import { PatientService } from '../services/patient.service';

@Component({
  selector: 'app-organizer',
  templateUrl: './organizer.component.html',
  styleUrls: ['./organizer.component.css']
})
export class OrganizerComponent implements OnInit {

  doctors: Doctor[] = [];
  patients: Patient[] = [];
  unassignedPatients: Patient[] = [];

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

  getPatients(): void {
    this.patientService.getPatients().subscribe(data => {
      this.patients = data;

    this.unassignedPatients = this.patients.filter(
        patient => patient.DoctorId == null || patient.DoctorId === 0
      );
    });
  }

  assignPatientToDoctor(
    patient: Patient,
    selectedDoctorId: number
  ): void {

    patient.DoctorId = selectedDoctorId;

    this.patientService
      .updatePatient(patient.PatientId, patient)
      .subscribe(() => {
        this.getPatients();
        this.getDoctors();
      });
  }

  releasePatientFromDoctor(patient: Patient): void {

    patient.DoctorId = null;

    this.patientService
      .updatePatient(patient.PatientId, patient)
      .subscribe(() => {
        this.getPatients();
        this.getDoctors();
      });
  }
}



////////////////////////////////

home------------------->

home.css-

.home-container {
    text-align: center;
    margin-top: 80px;
    padding: 30px;
}

.home-container h1 {
    color: #0d6efd;
    font-size: 40px;
    margin-bottom: 20px;
}

.home-container p {
    font-size: 18px;
    color: #555;
    margin-bottom: 10px;
}


home.html---

<div class="home-container">
    <h1>Hospital Management System</h1>

    <p>
        Welcome to the Hospital Management System.
    </p>

    <p>
        Manage doctors, patients, appointments, and assignments efficiently.
    </p>

</div>


home.ts-

import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

}


////////////////////////////////////

services-------------->

auth.service.ts--

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

  public baseUrl = 'https://8080-bbbbecefabaadaea351850042fffcaecbefaone.premiumproject.examly.io/api/users';

 


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



patient.service.ts--


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Patient } from '../../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private baseUrl = 'https://8080-bbbbecefabaadaea351850042fffcaecbefaone.premiumproject.examly.io/api/Patient';

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


doctor.service.ts--

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Doctor } from '../../models/doctor.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private baseUrl = 'https://8080-bbbbecefabaadaea351850042fffcaecbefaone.premiumproject.examly.io/api/Doctor';

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


////////////////////////////////

app.routing.module.ts--

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


app.module.ts--

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AdminComponent } from './admin/admin.component';
import { OrganizerComponent } from './organizer/organizer.component';
import { PatientComponent } from './patient/patient.component';
import { DoctorComponent } from './doctor/doctor.component';
import { HomeComponent } from './home/home.component';
import { ErrorComponent } from './error/error.component';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CreatePatientComponent } from './patient/create-patient/create-patient.component';
import { CreateDoctorComponent } from './doctor/create-doctor/create-doctor.component';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    NavbarComponent,
    AdminComponent,
    OrganizerComponent,
    PatientComponent,
    DoctorComponent,
    HomeComponent,
    ErrorComponent,
    CreatePatientComponent,
    CreateDoctorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }


app.comp.html---------->

<app-navbar></app-navbar>
<router-outlet></router-outlet>


/////////////////////////////////

models (of angularapp)------------>

doctor.model.ts--

import { Patient } from "./patient.model";

export interface Doctor{
    DoctorId?: number;
    Name: string;
    Specialization: string;
    ConsultationFee: number;
    Patients?: Patient[]
}

patient.model.ts--

import { Doctor } from "./doctor.model";
export interface Patient{
    PatientId?: number;
    Name: string;
    Age: number;
    Condition: string;
    AppointmentDate: Date;
    DoctorId: number | null;
    Doctor?: Doctor;
}

user.model.ts--

export interface User{
    Id?: number;
    Username: string;
    Password: string;
    Role: string;
}

login-model.model.ts--

export interface LoginModel{
    Username: string;
    Password: string;
}

================== :)  =====================

