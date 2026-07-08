8 july Session 1 cod 2

food-list.component.ts
 
import { Component, OnInit } from '@angular/core';
import { Food } from '../model/food.model';
import { FoodService } from '../services/food.service';
 
@Component({
  selector: 'app-food-list',
  templateUrl: './food-list.component.html',
  styleUrls: ['./food-list.component.css']
})
export class FoodListComponent implements OnInit {
 
  foods: Food[] = [];
 
  selectedFood!: Food;
 
  newFood: Food = {
    id: 0,
    name: '',
    description: ''
  };
 
  constructor(private foodService: FoodService) { }
 
  ngOnInit(): void {
    this.loadFoods();
  }
 
  loadFoods(): void {
    this.foodService.getFoods().subscribe(data => {
      this.foods = data;
    });
  }
 
  addFood(): void {
 
    if (this.newFood.id === 0) {
 
    this.foodService.addFood(this.newFood).subscribe(() => {
        this.loadFoods();
        this.newFood = {
          id: 0,
          name: '',
          description: ''
        };
      });
 
    } else {
 
    this.updateFood();
    }
  }
 
  editFood(food: Food): void {
    this.selectedFood = food;
 
    this.newFood = {
      id: food.id,
      name: food.name,
      description: food.description
    };
  }
 
  updateFood(): void {
    this.foodService.updateFood(this.newFood).subscribe(() => {
      this.loadFoods();
 
    this.newFood = {
        id: 0,
        name: '',
        description: ''
      };
    });
  }
 
  deleteFood(foodId: number): void {
    this.foodService.deleteFood(foodId).subscribe(() => {
      this.loadFoods();
    });
  }
}
 
 
food-list.component.html
 
<h2>Food List</h2>
 
<ul>
  <li *ngFor="let food of foods">
    {{ food.name }} - {{ food.description }}
 
    <button type="button" (click)="editFood(food)">
      Edit`</button>`
 
    <button type="button" (click)="deleteFood(food.id)">
      Delete`</button>`
 
</li>
</ul>
 
<h2>Add Food</h2>
 
<form (ngSubmit)="addFood()">
 
  `<label>`Name`</label>`
 
  <input
    type="text"
    name="name"
    [(ngModel)]="newFood.name"
    required>
 
  `<label>`Description`</label>`
 
<textarea
    name="description"
    [(ngModel)]="newFood.description"
    required>
  </textarea>
 
<button type="submit">
    Add Food
  </button>
 
</form>
 
 
model/food.model.ts
export interface Food {
    id: number;
    name: string;
    description: string;
  }
 
 
food.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Food } from '../model/food.model';
 
@Injectable({
  providedIn: 'root'
})
export class FoodService {
 
  public apiUrl = 'http://localhost:3001/foods';
 
  constructor(private http: HttpClient) { }
 
  getFoods(): Observable<Food[]> {
    return this.http.get<Food[]>(this.apiUrl);
  }
 
  addFood(food: Food): Observable<Food> {
    return this.http.post<Food>(this.apiUrl, food);
  }
 
  updateFood(food: Food): Observable<Food> {
    return this.http.put<Food>(`${this.apiUrl}/${food.id}`, food);
  }
 
  deleteFood(foodId: number): Observable <void> {
    return this.http.delete <void>(`${this.apiUrl}/${foodId}`);
  }
}
 
 
app.module.ts
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
 
import { AppComponent } from './app.component';
import { FoodListComponent } from './food-list/food-list.component';
 
@NgModule({
  declarations: [
    AppComponent,
    FoodListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.component.html
<app-food-list></app-food-list>


=======================

Session 2 Cod 1

model/event.model.ts
export interface Event {
    id?: number;
    name: string;
    date: string;
    location: string;
    description: string;
    isEditing?: boolean;
  }
  

event-list.component.ts


import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { Event } from '../model/event.model';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  events: Event[] = [];

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.getEvents();
  }

  getEvents(): void {
    this.eventService.getEvents().subscribe(data => {
      this.events = data;
    });
  }
}

event-list.component.ts
<table border="1" width="100%">
    <thead>
        <tr>
            <th>Name</th>
            <th>Date</th>
            <th>Location</th>
            <th>Description</th>
            <th>Actions</th>
        </tr>
    </thead>

    <tbody>
        <tr *ngFor="let event of events">
            <td>{{ event.name }}</td>
            <td>{{ event.date }}</td>
            <td>{{ event.location }}</td>
            <td>{{ event.description }}</td>
            <td>-</td>
        </tr>
    </tbody>
</table>

add-event.component.ts
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from '../services/event.service';

@Component({
  selector: 'app-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.css']
})
export class AddEventComponent implements OnInit {

  eventForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private eventService: EventService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.eventForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(6)]],
      date: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{4}-\d{2}-\d{2}$/),
          this.dateValidator
        ]
      ],
      location: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  dateValidator(control: AbstractControl): ValidationErrors | null {

    if (!control.value) {
      return null;
    }

    const value = control.value;
    const parts = value.split('-');

    if (parts.length !== 3) {
      return { invalidDate: true };
    }

    const month = +parts[1];
    const day = +parts[2];

    if (month < 1 || month > 12) {
      return { invalidDate: true };
    }

    if (day < 1 || day > 31) {
      return { invalidDate: true };
    }

    return null;
  }

  addNewEvent(): void {

    if (this.eventForm.invalid) {
      this.eventForm.markAllAsTouched();
      return;
    }

    this.eventService.addEvent(this.eventForm.value).subscribe(() => {
      this.router.navigate(['/events']);
    });
  }

  get f() {
    return this.eventForm.controls;
  }
}

add-event.component.html


<form [formGroup]="eventForm" (ngSubmit)="addNewEvent()">

    <!-- Name -->
    
    <div>
        <input
          type="text"
          placeholder="Event Name"
          formControlName="name">
    
        <div *ngIf="f['name'].touched && f['name'].errors">
          <small *ngIf="f['name'].errors['required']">
            Name is required`</small>`
    
        <small *ngIf="f['name'].errors['minlength']">
            Minimum length should be 6`</small>`
        `</div>`
    
    </div>
    
    <br>
    
    <!-- Date -->
    
    <div>
        <input
          type="text"
          placeholder="yyyy-mm-dd"
          formControlName="date">
    
        <div *ngIf="f['date'].touched && f['date'].errors">
    
        <small *ngIf="f['date'].errors['required']">
            Date is required`</small>`
    
        <small *ngIf="f['date'].errors['pattern']">
            Date format must be yyyy-mm-dd`</small>`
    
        <small *ngIf="f['date'].errors['invalidDate']">
            Invalid month or day`</small>`
    
        `</div>`
    
    </div>
    
    <br>
    
    <!-- Location -->
    
    <div>
        <input
          type="text"
          placeholder="Location"
          formControlName="location">
    
        <div *ngIf="f['location'].touched && f['location'].errors">`<small>`Location is required`</small>`
        `</div>`
    
    </div>
    
    <br>
    
    <!-- Description -->
    
    <div>
        <input
          type="text"
          placeholder="Description"
          formControlName="description">
    
        <div *ngIf="f['description'].touched && f['description'].errors">`<small>`Description is required`</small>`
        `</div>`
    
    </div>
    
    <br>
    
    <button type="submit">
        Add Event
      </button>
    
    </form>
    
event.service.ts


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event } from '../model/event.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  public backendUrl = 'http://localhost:3001/events';

  constructor(private http: HttpClient) {}

  getEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(this.backendUrl);
  }

  addEvent(obj: any): Observable <any> {
    return this.http.post(this.backendUrl, obj);
  }
}

app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { EventListComponent } from './event-list/event-list.component';
import { AddEventComponent } from './add-event/add-event.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    EventListComponent,
    AddEventComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}

app.component.html

<nav>
  <a routerLink="/events">Event List</a> |
  <a routerLink="/add-event">Add Event</a>
</nav>

<hr>

<router-outlet></router-outlet>

================================

Session2 cod 2

add-vehicle.component.ts


import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-add-vehicle',
  templateUrl: './add-vehicle.component.html',
  styleUrls: ['./add-vehicle.component.css']
})
export class AddVehicleComponent {

  name: string = '';
  type: string = '';
  brand: string = '';

  constructor(
    private vehicleService: VehicleService,
    private router: Router
  ) { }

  addVehicle() {

    const newVehicle = {
      name: this.name,
      type: this.type,
      brand: this.brand
    };

    this.vehicleService.addVehicle(newVehicle);

    this.router.navigate(['/vehicles']);
  }
}

add-vehicle.component.html



<h2>Add Vehicle</h2>

<div>
  <label>Name</label><br>
  <input
    type="text"
    [(ngModel)]="name"
    #vehicleName="ngModel"
    required>

<div *ngIf="vehicleName.invalid && vehicleName.touched">
    Name is required
  </div>
</div>

<br>

<div>
  <label>Type</label><br>
  <input
    type="text"
    [(ngModel)]="type"
    #vehicleType="ngModel"
    required>

<div *ngIf="vehicleType.invalid && vehicleType.touched">
    Type is required
  </div>
</div>

<br>

<div>
  <label>Brand</label><br>
  <input
    type="text"
    [(ngModel)]="brand"
    #vehicleBrand="ngModel"
    required>

<div *ngIf="vehicleBrand.invalid && vehicleBrand.touched">
    Brand is required
  </div>
</div>

<br>

<button (click)="addVehicle()">
  Add Vehicle
`</button>`

vehicle-list.component.ts



import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {

  vehicles: any[] = [];

  constructor(private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.vehicles = this.vehicleService.getVehicles();
  }

  deleteVehicle(index: number) {
    this.vehicleService.deleteVehicle(index);
  }
}

vehicle.component.html


<h2>Vehicle List</h2>

<div *ngIf="vehicles.length > 0; else noVehicles">

<ul>
    <li *ngFor="let vehicle of vehicles; let i = index">

    {{ vehicle.name }} -
      {{ vehicle.type }} -
      {{ vehicle.brand }}

    <button (click)="deleteVehicle(i)">
        Delete`</button>`

    `</li>`

</ul>

</div>

<ng-template #noVehicles>

<p>No vehicle is added.</p>
</ng-template>

vehicle.service.ts

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  vehicles: any[] = [];

  constructor() { }

  getVehicles() {
    return this.vehicles;
  }

  addVehicle(vehicle: any) {
    this.vehicles.push(vehicle);
  }

  deleteVehicle(index: number) {
    this.vehicles.splice(index, 1);
  }
}

app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddVehicleComponent } from './add-vehicle/add-vehicle.component';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    AddVehicleComponent,
    VehicleListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

app.component.html


<h1>Vehicle Inventory Management System</h1>

<nav>
  <a routerLink="/vehicles">Vehicle List</a>
    
  <a routerLink="/add-vehicle">Add Vehicle</a>
</nav>

<hr>

`<router-outlet></router-outlet>`

app-routing.module.ts

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { AddVehicleComponent } from './add-vehicle/add-vehicle.component';

const routes: Routes = [
  { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
  { path: 'vehicles', component: VehicleListComponent },
  { path: 'add-vehicle', component: AddVehicleComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


======================

==================

Session 3 cod 1 

product.model.ts

export interface Product {
  id?: number;
  name?: string;
  category?: string;
  price?: number;
  description?: string;
}

product.service.ts

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  getAllProducts() {
    return [];
  }

  addProduct(product: any) {
    return product;
  }

  getProductById(id: number) {
    return {};
  }
}

add-product.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html'
})
export class AddProductComponent {

  addProduct() {
  }
}


add-product.component.html

<h2>Add Product</h2>

product-list.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html'
})
export class ProductListComponent {
}

product-list.component.html

<h2>Product List</h2>

view-product.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.component.html'
})
export class ViewProductComponent {
}

view-product.component.html

<h2>View Product</h2>

app.component.html

<router-outlet></router-outlet>

==================


Session 3 Cod 2

navbar.component.html

<nav>
    <a>Home</a>
    <a>About</a>
    <a>Contact</a>
  </nav>
  

home.component.html



<h2>Welcome to the Home Page</h2>


contact.component.html

<h2>Contact Us</h2>

about.component.html



<h2>About Us</h2>




