
 
7 July session 2 cod 1
 
form.component.ts
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
 
@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
 
  form!: FormGroup;
 
  constructor(private fb: FormBuilder) { }
 
  ngOnInit(): void {
 
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      phonenumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      age: ['', [Validators.required, Validators.min(18)]]
    });
  }
 
  get f() {
    return this.form.controls;
  }
 
  onSubmit(): void {
 
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
 
    if (this.form.value.password !== this.form.value.confirmPassword) {
      return;
    }
 
    alert('Form submitted successfully!');
  }
}
 
 
form.component.html
 
<h2>Reactive Form</h2>
 
<form [formGroup]="form" (ngSubmit)="onSubmit()">
 
  <label for="firstName">First Name</label>
  <input
    type="text"
    id="firstName"
    formControlName="firstName">
 
<div *ngIf="f['firstName'].touched && f['firstName'].errors?.['required']">
    First Name is required
  </div>
 
  <label for="lastName">Last Name</label>
  <input
    type="text"
    id="lastName"
    formControlName="lastName">
 
<div *ngIf="f['lastName'].touched && f['lastName'].errors?.['required']">
    Last Name is required
  </div>
 
  <label for="password">Password</label>
  <input
    type="password"
    id="password"
    formControlName="password">
 
<div *ngIf="f['password'].touched && f['password'].errors?.['required']">
    Password is required
  </div>
 
<div *ngIf="f['password'].touched && f['password'].errors?.['minlength']">
    Minimum length is 6
  </div>
 
  <label for="confirmPassword">Confirm Password</label>
  <input
    type="password"
    id="confirmPassword"
    formControlName="confirmPassword">
 
<div *ngIf="f['confirmPassword'].touched && f['confirmPassword'].errors?.['required']">
    Confirm Password is required
  </div>
 
<div *ngIf="form.value.password !== form.value.confirmPassword
              && form.value.confirmPassword">
    Passwords do not match
  </div>
 
  <label for="phonenumber">Phone number</label>
  <input
    type="text"
    id="phonenumber"
    formControlName="phonenumber">
 
<div *ngIf="f['phonenumber'].touched && f['phonenumber'].errors?.['required']">
    Phone number is required
  </div>
 
  <label for="email">Email</label>
  <input
    type="email"
    id="email"
    formControlName="email">
 
<div *ngIf="f['email'].touched && f['email'].errors?.['required']">
    Email is required
  </div>
 
<div *ngIf="f['email'].touched && f['email'].errors?.['email']">
    Invalid email format
  </div>
 
  <label for="age">Age</label>
  <input
    type="number"
    id="age"
    formControlName="age">
 
<div *ngIf="f['age'].touched && f['age'].errors?.['required']">
    Age is required
  </div>
 
<div *ngIf="f['age'].touched && f['age'].errors?.['min']">
    Age must be at least 18
  </div>
 
<button type="submit" [disabled]="form.invalid">
    Submit
  </button>
 
</form>
 
 
 
app.module.ts
 
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { FormComponent } from './form/form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    FormComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.component.html
<app-form></app-form>
 
 
7 July Session 2 cod 2
 
eventform.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
 
@Component({
  selector: 'app-eventform',
  templateUrl: './eventform.component.html',
  styleUrls: ['./eventform.component.css']
})
export class EventformComponent implements OnInit {
 
  registrationForm!: FormGroup;
 
  submitted: boolean = false;
 
  formData: any = {};
 
  sportsList: string[] = [
    'Football',
    'Basketball',
    'Athletics',
    'Tennis'
  ];
 
  constructor(private fb: FormBuilder) { }
 
  ngOnInit(): void {
 
    this.registrationForm = this.fb.group({
      name: [''],
      age: [''],
      grade: [''],
      gender: [''],
      email: [''],
      phone: [''],
      sports: this.fb.group({
        Football: [false],
        Basketball: [false],
        Athletics: [false],
        Tennis: [false]
      })
    });
  }
 
  getSelectedSports(): string {
 
    const selectedSports = Object.keys(
      this.registrationForm.get('sports')?.value
    ).filter(
      key => this.registrationForm.get('sports')?.value[key]
    );
 
    return selectedSports.join(', ');
  }
 
  onSubmit(): void {
 
    this.submitted = true;
 
    this.formData = {
      ...this.registrationForm.value,
      sports: this.getSelectedSports()
    };
 
    this.registrationForm.reset();
 
    this.registrationForm.patchValue({
      sports: {
        Football: false,
        Basketball: false,
        Athletics: false,
        Tennis: false
      }
    });
  }
 
  closeModal(): void {
    this.submitted = false;
  }
}
 
 
eventform.component.html
 
<h1>Registration Form</h1>
 
<form [formGroup]="registrationForm" (ngSubmit)="onSubmit()">
 
<div>
    <label class="form-label">Name:*</label>
    <input type="text" formControlName="name">
  </div>
 
<div>
    <label class="form-label">Age:*</label>
    <input type="number" formControlName="age">
  </div>
 
<div>
    <label class="form-label">Grade:*</label>
    <input type="text" formControlName="grade">
  </div>
 
<div>
    <label class="form-label">Gender:*</label>
    <input type="text" formControlName="gender">
  </div>
 
<div>
    <label class="form-label">Sports*</label>
 
    `<div formGroupName="sports">`
      `<label>`
        `<input type="checkbox" formControlName="Football">`
        Football
      `</label>`
 
    `<label>`
        `<input type="checkbox" formControlName="Basketball">`
        Basketball
      `</label>`
 
    `<label>`
        `<input type="checkbox" formControlName="Athletics">`
        Athletics
      `</label>`
 
    `<label>`
        `<input type="checkbox" formControlName="Tennis">`
        Tennis
      `</label>`
    `</div>`
 
</div>
 
<div>
    <label class="form-label">Email:*</label>
    <input type="email" formControlName="email">
  </div>
 
<div>
    <label class="form-label">Phone:*</label>
    <input type="text" formControlName="phone">
  </div>
 
  `<button type="submit">`Submit`</button>`
 
</form>
 
<div *ngIf="submitted">
 
<h3>Registration Successful</h3>
 
<p>Name: {{ formData.name }}</p>
  <p>Age: {{ formData.age }}</p>
  <p>Grade: {{ formData.grade }}</p>
  <p>Gender: {{ formData.gender }}</p>
  <p>Email: {{ formData.email }}</p>
  <p>Phone: {{ formData.phone }}</p>
  <p>Sports: {{ formData.sports }}</p>
 
<button type="button" (click)="closeModal()">
    Close
  </button>
 
</div>
 
 
app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { EventformComponent } from './eventform/eventform.component';
 
@NgModule({
  declarations: [
    AppComponent,
    EventformComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.component.html
<app-eventform></app-eventform>
 
 
7 July session 3 cod 1
 
