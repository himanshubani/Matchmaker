
 
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
 
