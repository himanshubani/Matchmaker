Models
==============
Bus.models.ts
export interface Bus
{
bookingId?:number;
busNumber:string;
routeSource:string;
routeDestination:string;
passengerName:string;
bookingDate:string;
}
//heart of system without this nothing is possible
Services
Bus.services.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bus } from '../models/bus.model';
import {HttpClient} from '@angular/common/http';
@Injectable({
providedIn: 'root'
})
export class BusService {
private apiUrl='https://8080-
fedcdeaabaac352196661ffdbabfeone.premiumproject.examly.io';
constructor(private objHttpClient:HttpClient) 
{ 
}
addBus(bus:Bus):Observable<Bus>
{
//first parameter is url and 2nd is object
return this.objHttpClient.post<Bus>(`${this.apiUrl}/api/Bus`,bus);
}
//it will return bus array
getBuses():Observable<Bus[]>
{
return this.objHttpClient.get<Bus[]>(`${this.apiUrl}/api/Bus`);
}
getBusById(bookingId:number):Observable<Bus>{
return this.objHttpClient.get<Bus>(`${this.apiUrl}/api/Bus/${bookingId}`);
}
updateBus(bookingId:number,bus:Bus):Observable<Bus>
{
return this.objHttpClient.put<Bus>(`${this.apiUrl}/api/Bus/${bookingId}`,bus);
}
deleteBus(bookingId:number):Observable<void>
{
return this.objHttpClient.delete<void>(`${this.apiUrl}/api/Bus/${bookingId}`);
}
}
Header.component.html
<nav class="navbar navbar-expand-lg navbar-dark bg-primary">
<h1 class="navbar brand">Bus Booking Platform</h1>
<div class="ml-auto">
<a routerLink="/addNewBus" class="btn btn-primarytext-white">Add New Bus</a>
<a routerLink="/viewBuses" class="btn btn-primarytext-white">View Bus</a>
</div>
</nav>
Bus-form.components
Bus-form.component.ts
import { Component, OnInit } from '@angular/core';
import { BusService } from '../services/bus.service';
import { Bus } from '../models/bus.model';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
@Component({
selector: 'app-bus-form',
templateUrl: './bus-form.component.html',
styleUrls: ['./bus-form.component.css']
})
export class BusFormComponent implements OnInit{
newBus: Bus={
routeSource:'',
routeDestination:'',
busNumber:'',
passengerName:'',
bookingDate:''
}
isEditMode:boolean;
constructor(private busService:BusService,private router:Router,private 
route:ActivatedRoute){
}
ngOnInit(): void {
const id=this.route.snapshot.paramMap.get('id');
if(id){
this.isEditMode=true;
this.busService.getBusById(+id).subscribe(bus=>this.newBus=bus)
}
}
addOrEditBus()
{
if(this.isEditMode)
{
this.busService.updateBus(this.newBus.bookingId,this.newBus).subscribe(()=>thi
s.router.navigate(['viewBuses']));
}
else{
this.busService.addBus(this.newBus).subscribe(()=>this.router.navigate(['viewBus
es']));
}
}
}
//(+id) + is used for type casting 
//
Bus-form.comonent.html
<form #busForm="ngForm" (ngSubmit)="addOrEditBus()">
<div class="form-group">
<label>Bus Number:</label>
<input type="text" class="form-control" id="busNumber" name="busNumber" 
[(ngModel)]="newBus.busNumber" required>
<div class="error-message" *ngIf="busForm.submitted && 
!newBus.busNumber">Bus Number is required</div>
</div>
<div class="form-group">
<label>Route Source:</label>
<input type="text" class="form-control" name="routeSource" id="routeSource" 
[(ngModel)]="newBus.routeSource" required>
<div class="error-message" *ngIf="busForm.submitted && 
!newBus.routeSource">Route Source is required</div>
</div>
<div class="form-group">
<label >Route Destination:</label>
<input type="text" class="form-control" name="routeDestination" 
id="routeDestination" [(ngModel)]="newBus.routeDestination" required>
<div class="error-message" *ngIf="busForm.submitted && 
!newBus.routeDestination">Route Destination is required</div>
</div>
<div class="form-group">
<label>Passenger Name:</label>
<input type="text" class="form-control" name="passengerName" 
id="passengerName" [(ngModel)]="newBus.passengerName" required>
<div class="error-message" *ngIf="busForm.submitted && 
!newBus.passengerName">Passenger Name is required</div>
</div>
<div class="form-group">
<label>Booking Date:</label>
<input type="date" class="form-control" name="bookingDate" id="bookingDate" 
[(ngModel)]="newBus.bookingDate" required>
<div class="error-message" *ngIf="busForm.submitted && 
!newBus.bookingDate">Booking Date is required</div>
</div>
<button type="submit">{{isEditMode?'Update Bus':'Add Bus'}}</button>
</form>
Bus-list.components
bus-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BusService } from '../services/bus.service';
import { Bus } from '../models/bus.model';
@Component({
selector: 'app-bus-list',
templateUrl: './bus-list.component.html',
styleUrls: ['./bus-list.component.css']
})
export class BusListComponent implements OnInit {
buses:Bus[]=[];
constructor(private busService:BusService,private route:Router){
}
ngOnInit(): void {
this.loadBuses();
}
loadBuses(){
this.busService.getBuses().subscribe(buses=>this.buses=buses);
}
editBus(bookingId:number):void{
this.route.navigate([`/editBus/${bookingId}`]);
}
deleteBus(bookingId:number):void{
this.route.navigate([`/confirmDelete/${bookingId}`]);
}
}
Bus-list.component.html
<table>
<thead>
<tr>
<th>Bus Number</th>
<th>Route Source</th>
<th>Route Destination</th>
<th>Passenger Name</th>
<th>Booking Date</th>
<th>Actions</th>
</tr>
</thead>
<tbody>
<tr *ngFor="let bus of buses">
<td>{{bus.busNumber}}</td>
<td>{{bus.routeSource}}</td>
<td>{{bus.routeDestination}}</td>
<td>{{bus.passengerName}}</td>
<td>{{bus.bookingDate | date: 'mediumDate'}}</td>
<td>
<button class="edit-button" id="edit" 
(click)="editBus(bus.bookingId!)">Edit</button>
<button class="delete-button" id="delete" 
(click)="deleteBus(bus.bookingId!)">Delete</button>
</td>
</tr>
</tbody>
</table>
Delete-confirm.components
Delete-confirm.component.ts
import { Component, OnInit } from '@angular/core';
import { Bus } from '../models/bus.model';
import { BusService } from '../services/bus.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
@Component({
selector: 'app-delete-confirm',
templateUrl: './delete-confirm.component.html',
styleUrls: ['./delete-confirm.component.css']
})
export class DeleteConfirmComponent implements OnInit{
bus:Bus;
constructor (private busService:BusService,private route:ActivatedRoute,private 
router:Router){
}
ngOnInit(): void {
this.route.params.subscribe(params=>{const bookingId =+params['id'];
this.busService.getBusById(bookingId).subscribe(bus=>this.bus=bus)});
}
confirmDelete(bookingId:number):void{
this.busService.deleteBus(bookingId).subscribe(()=>{this.router.navigate(['/viewBus
es'])});
}
cancelDelete():void{
this.router.navigate(['/viewBuses']);
}
}
Delete-confirm.component.html
<h2>Delete Confirmation</h2>
<p>Are you sure you want to delete this bus?</p>
<p>Bus Number: {{bus?.busNumber}}</p>
<p>Route Source: {{bus?.routeSource}}</p>
<p>Route Destination: {{bus?.routeDestination}}</p>
<p>Passenger Name: {{bus?.passengerName}}</p>
<p>Booking Date: {{bus?.bookingDate}}</p>
<button class="confirm-button" type="button" 
(click)="confirmDelete(bus?.bookingId!)">Confirm Delete</button>
<button class="cancel-button" type="button" 
(click)="cancelDelete()">Cancel</button>
App.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { BusFormComponent } from './bus-form/bus-form.component';
import { BusListComponent } from './bus-list/bus-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';
@NgModule({
declarations: [
AppComponent,
HeaderComponent,
BusFormComponent,
BusListComponent,
DeleteConfirmComponent
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
app.component.html
<app-header></app-header>
<router-outlet></router-outlet>
App-rounting.module.ts
===
import { NgModule } from '@angular/core';
//handles all the routing 
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { BusFormComponent } from './bus-form/bus-form.component';
import { BusListComponent } from './bus-list/bus-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';
const routes: Routes = [
{path :'addNewBus',component:BusFormComponent },
{path :'viewBuses', component:BusListComponent },
{path: 'editBus/:id',component:BusFormComponent},
{path: 'confirmDelete/:id',component:DeleteConfirmComponent}
];
@NgModule({
imports: [RouterModule.forRoot(routes)],
exports: [RouterModule]
})
export class AppRoutingModule { }
