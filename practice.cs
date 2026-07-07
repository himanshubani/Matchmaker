app/models/employee.ts

export class Employee {
  FirstName: string = '';
  LastName: string = '';
  Gender: string = '';
  Email: string = '';
  TermsOfConditions: boolean = false;

  ContactDetails = {
    Address: '',
    Phone: ''
  };
}

app/employee-form/employee-form.component.ts
import { Component } from '@angular/core';
import { Employee } from '../models/employee';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent {

  employee: Employee = {
    FirstName: '',
    LastName: '',
    Gender: '',
    Email: '',
    TermsOfConditions: false,
    ContactDetails: {
      Address: '',
      Phone: ''
    }
  };

  onSubmit(): void {
    console.log(this.employee);
  }
}

app/employee-form/employee-form.component.html
<form #employeeForm="ngForm" (ngSubmit)="onSubmit()">

  <div>
    <label for="firstName">First Name</label>

    <input
      type="text"
      id="firstName"
      name="firstName"
      [(ngModel)]="employee.FirstName"
      #firstName="ngModel"
      required
      minlength="2"
      maxlength="50">

    <div *ngIf="firstName.invalid && firstName.touched">
      <small *ngIf="firstName.errors?.['required']">
        First Name is required
      </small>

      <small *ngIf="firstName.errors?.['minlength']">
        Minimum length is 2
      </small>

      <small *ngIf="firstName.errors?.['maxlength']">
        Maximum length is 50
      </small>
    </div>
  </div>

  <br>

  <div>
    <label for="lastName">Last Name</label>

    <input
      type="text"
      id="lastName"
      name="lastName"
      [(ngModel)]="employee.LastName"
      #lastName="ngModel"
      required
      minlength="2"
      maxlength="50">

    <div *ngIf="lastName.invalid && lastName.touched">
      <small *ngIf="lastName.errors?.['required']">
        Last Name is required
      </small>

      <small *ngIf="lastName.errors?.['minlength']">
        Minimum length is 2
      </small>

      <small *ngIf="lastName.errors?.['maxlength']">
        Maximum length is 50
      </small>
    </div>
  </div>

  <br>

  <div>
    <label>Gender</label>

    <input
      type="radio"
      name="gender"
      value="Male"
      [(ngModel)]="employee.Gender">
    Male

    <input
      type="radio"
      name="gender"
      value="Female"
      [(ngModel)]="employee.Gender">
    Female
  </div>

  <br>

  <div>
    <label for="email">Email</label>

    <input
      type="email"
      id="email"
      name="email"
      [(ngModel)]="employee.Email"
      #email="ngModel"
      required
      email>

    <div *ngIf="email.invalid && email.touched">
      <small *ngIf="email.errors?.['required']">
        Email is required
      </small>

      <small *ngIf="email.errors?.['email']">
        Enter a valid Email
      </small>
    </div>
  </div>

  <br>

  <fieldset>
    <legend>ContactDetails</legend>

    <div>
      <label for="address">Address</label>

      <input
        type="text"
        id="address"
        name="address"
        [(ngModel)]="employee.ContactDetails.Address">
    </div>

    <br>

    <div>
      <label for="phone">Phone</label>

      <input
        type="text"
        id="phone"
        name="phone"
        [(ngModel)]="employee.ContactDetails.Phone">
    </div>
  </fieldset>

  <br>

  <div>
    <input
      type="checkbox"
      id="termsConditions"
      name="termsConditions"
      [(ngModel)]="employee.TermsOfConditions"
      #termsConditions="ngModel"
      required>

    <label for="termsConditions">
      I Agree to Terms and Conditions
    </label>

    <div *ngIf="termsConditions.invalid && termsConditions.touched">
      <small>
        Terms and Conditions must be accepted
      </small>
    </div>
  </div>

  <br>

  <button type="submit" [disabled]="employeeForm.invalid">
    Submit
  </button>

</form>

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

app.component.html
<app-employee-form></app-employee-form>
