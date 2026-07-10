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
 
org.html-
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
.ts->
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
 
    patient.DoctorId = 0;
 
    this.patientService
      .updatePatient(patient.PatientId, patient)
      .subscribe(() => {
        this.getPatients();
        this.getDoctors();
      });
  }
}
 
 
