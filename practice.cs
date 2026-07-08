models/employee.model.ts
export class Employee {
    firstName: string = '';
    lastName: string = '';
    gender: string = '';
    email: string = '';
    termsOfConditions: boolean = false;
 
    contactDetails = {
      address: '',
      phone: ''
    };
  }
 
 
employee-form.component.html


 
<form #employeeForm="ngForm" (ngSubmit)="onSubmit()">
 
    <!-- First Name -->
   
    <div>
        <label><b>First Name:</b></label><br>
   
        <input
          type="text"
          id="firstName"
          name="firstName"
          [(ngModel)]="employee.firstName"
          required
          minlength="2"
          maxlength="50"
          #firstName="ngModel">
   
        <div *ngIf="firstName.invalid && firstName.touched" style="color:red">
          <div *ngIf="firstName.errors?.['required']">
            First Name is required.`</div>`
          <div *ngIf="firstName.errors?.['minlength']">
            Minimum 2 characters required.
          `</div>`
        `</div>`
   
    </div>
   
    <!-- Last Name -->
   
    <div>
        <label><b>Last Name:</b></label><br>
   
        <input
          type="text"
          id="lastName"
          name="lastName"
          [(ngModel)]="employee.lastName"
          required
          minlength="2"
          maxlength="50"
          #lastName="ngModel">
   
        <div *ngIf="lastName.invalid && lastName.touched" style="color:red">
          Last Name is required.`</div>`
   
    </div>
   
    <!-- Gender -->
   
    <div>
        <label><b>Gender:</b></label><br>
   
        <input
          type="radio"
          name="gender"
          value="Male"
          [(ngModel)]="employee.gender"> Male
   
        <input
          type="radio"
          name="gender"
          value="Female"
          [(ngModel)]="employee.gender"> Female
   
    </div>
   
    <!-- Email -->
   
    <div>
        <label><b>Email:</b></label><br>
   
        <input
          type="email"
          id="email"
          name="email"
          [(ngModel)]="employee.email"
          required
          email
          #email="ngModel">
   
        <div *ngIf="email.invalid && email.touched" style="color:red">
          <div *ngIf="email.errors?.['required']">
            Email is required.`</div>`
          <div *ngIf="email.errors?.['email']">
            Enter valid email.
          `</div>`
        `</div>`
   
    </div>
   
    <!-- Terms & Conditions -->
   
    <div>
        <label><b>Terms and Conditions:</b></label><br>
   
        <input
          type="checkbox"
          id="termsConditions"
          name="termsConditions"
          [(ngModel)]="employee.termsOfConditions"
          required
          #termsConditions="ngModel">
   
        <div
          *ngIf="!employee.termsOfConditions && termsConditions.touched"
          style="color:red">
          Terms and Conditions must be accepted.`</div>`
   
    </div>
   
    <!-- Contact Details -->
   
    <div>
   
        `<label><b>`Address:`</b></label>``<br>`
   
        <input
          type="text"
          id="address"
          name="address"
          [(ngModel)]="employee.contactDetails.address"
          required
          #address="ngModel">
   
        <div *ngIf="address.invalid && address.touched" style="color:red">
          Address is required.`</div>`
   
        `<br>`
   
        `<label><b>`Phone:`</b></label>``<br>`
   
        <input
          type="text"
          id="phone"
          name="phone"
          [(ngModel)]="employee.contactDetails.phone"
          required
          #phone="ngModel">
   
        <div *ngIf="phone.invalid && phone.touched" style="color:red">
          Phone is required.`</div>`
   
    </div>
   
    <br>
   
    <button type="submit" [disabled]="employeeForm.invalid">
        Submit
      </button>
   
    </form>
   
 
employee-form.component.ts


 
import { Component } from '@angular/core';
import { Employee } from '../models/employee.model';
 
@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent {
 
  employee: Employee = new Employee();
 
  onSubmit() {
    console.log(this.employee);
  }
}
 
 
app.component.html
<app-employee-form></app-employee-form>
 
 
app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    EmployeeFormComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
]
 
registration-form.component.ts
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
 
@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent {
 
  isSubmitted: boolean = false;
  passwordMismatch: boolean = false;
 
  onSubmit(form: NgForm): void {
 
    if (form.value.password !== form.value.confirmPassword) {
      this.passwordMismatch = true;
      this.isSubmitted = false;
      return;
    }
 
    this.passwordMismatch = false;
 
    if (form.valid) {
      this.isSubmitted = true;
      form.resetForm();
    }
  }
}
 
 
registration-form.component.html
 
<h1>Registration Form</h1>
 
<form #registrationForm="ngForm"
      (ngSubmit)="onSubmit(registrationForm)">
 
  <label for="name">Name</label>
  <input
    type="text"
    id="name"
    name="name"
    ngModel
    required>
 
  <label for="email">Email</label>
  <input
    type="email"
    id="email"
    name="email"
    ngModel
    required
    email>
 
  <label for="password">Password</label>
  <input
    type="password"
    id="password"
    name="password"
    ngModel
    required>
 
  <label for="confirmPassword">Confirm Password</label>
  <input
    type="password"
    id="confirmPassword"
    name="confirmPassword"
    ngModel
    required>
 
<div *ngIf="passwordMismatch" class="error-message">
    Passwords do not match
  </div>
 
  <button
    type="submit"
    [disabled]="registrationForm.invalid">
    Register
  </button>
 
</form>
 
<div *ngIf="isSubmitted" class="success-message">
  Registration successful!
</div>
 
 
app.module.ts
 
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    RegistrationFormComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.component.html
<app-registration-form></app-registration-form>
 
 
 
7 July session 1 cod 2
 
contact-form.component.ts
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
 
@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent {
 
  contactForm: FormGroup;
 
  constructor(private fb: FormBuilder) {
 
    this.contactForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
 
    address: this.fb.group({
        street: [''],
        city: [''],
        state: [''],
        postalCode: ['']
      }),
 
    subjectDetails: this.fb.group(
        {
          subject: [''],
          message: ['']
        },
        {
          validators: this.subjectMessageValidator
        }
      )
    });
  }
 
  subjectMessageValidator(
    group: AbstractControl
  ): ValidationErrors | null {
 
    const subject = group.get('subject')?.value;
    const message = group.get('message')?.value;
 
    if ((subject && !message) || (!subject && message)) {
      return { subjectMessageRequired: true };
    }
 
    return null;
  }
 
  onSubmit(): void {
 
    if (this.contactForm.valid) {
 
    console.log(this.contactForm.value);
 
    this.contactForm.reset();
 
    } else {
 
    console.log('Form Invalid');
 
    this.contactForm.markAllAsTouched();
    }
  }
 
  get firstName() {
    return this.contactForm.get('firstName');
  }
 
  get lastName() {
    return this.contactForm.get('lastName');
  }
 
  get email() {
    return this.contactForm.get('email');
  }
}
 
 
contact-form.component.html
 
 
<h1>Contact Form</h1>
 
<form
  [formGroup]="contactForm"
  (ngSubmit)="onSubmit()">
 
<div>
    <label>First Name</label>
 
    `<input
      type="text"
      formControlName="firstName">`
 
    <div
      *ngIf="firstName?.touched && firstName?.errors?.['required']">
      First Name is required`</div>`
 
    <div
      *ngIf="firstName?.touched && firstName?.errors?.['minlength']">
      Minimum 2 characters required`</div>`
 
</div>
 
<br>
 
<div>
    <label>Last Name</label>
 
    `<input
      type="text"
      formControlName="lastName">`
 
    <div
      *ngIf="lastName?.touched && lastName?.errors?.['required']">
      Last Name is required`</div>`
 
    <div
      *ngIf="lastName?.touched && lastName?.errors?.['minlength']">
      Minimum 2 characters required`</div>`
 
</div>
 
<br>
 
<div>
    <label>Email</label>
 
    `<input
      type="email"
      formControlName="email">`
 
    <div
      *ngIf="email?.touched && email?.errors?.['required']">
      Email is required`</div>`
 
    <div
      *ngIf="email?.touched && email?.errors?.['email']">
      Invalid email format`</div>`
 
</div>
 
<br>
 
<div formGroupName="address">
 
    `<h3>`Address`</h3>`
 
    `<input
      type="text"
      formControlName="street"
      placeholder="Street">`
 
    `<input
      type="text"
      formControlName="city"
      placeholder="City">`
 
    `<input
      type="text"
      formControlName="state"
      placeholder="State">`
 
    `<input
      type="text"
      formControlName="postalCode"
      placeholder="Postal Code">`
 
</div>
 
<br>
 
<div formGroupName="subjectDetails">
 
    `<h3>`Subject Details`</h3>`
 
    `<input
      type="text"
      formControlName="subject"
      placeholder="Subject">`
 
    `<textarea
      formControlName="message"
      placeholder="Message"></textarea>`
 
    <div
      *ngIf="contactForm.get('subjectDetails')?.errors?.['subjectMessageRequired']">
      Subject and Message must both be filled.`</div>`
 
</div>
 
<br>
 
  <button
    type="submit"
    [disabled]="contactForm.invalid">
    Submit
  `</button>`
 
</form>
 
 
app.module.ts
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { ContactFormComponent } from './contact-form/contact-form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    ContactFormComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.comonent.html
<app-contact-form></app-contact-form>
 
 
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
 
