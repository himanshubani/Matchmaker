

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




