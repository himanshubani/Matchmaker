 
<h1>Product Details</h1>
 
<div class="product-details-box" *ngIf="selectedProduct">
 
    <img [src]="selectedProduct.imageUrl" [alt]="selectedProduct.name" width="150">
 
    <h3>{{ selectedProduct.name }}</h3>
 
    <p>{{ selectedProduct.description }}</p>
 
    <p>Price: {{ selectedProduct.price }}</p>
 
</div>
 
import { Component, Input } from '@angular/core';
 
@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {
 
  @Input()
  selectedProduct: any;
}
 
 
<h1>Product List</h1>
 
<div class="product-item" *ngFor="let product of products">
    <img [src]="product.imageUrl" [alt]="product.name">
 
    <h3>{{ product.name }}</h3>
 
    <button (click)="viewDetails(product)">
        View Details
    </button>
 
</div>
 
import { Component, EventEmitter, Output } from '@angular/core';
 
@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
 
  @Output()
  productSelected = new EventEmitter<any>();
 
  products = [
    {
      id: 1,
      name: 'Pencil',
      description: 'A pencil is an object that you write or draw with. It consists of a thin piece of wood with a rod of a black or coloured substance through the middle',
      price: 10.99,
      imageUrl: 'assets/pencil.jpg'
    },
    {
      id: 2,
      name: 'Scale',
      description: 'A scale is a set of levels or numbers which are used in a particular system of measuring things or are used when comparing things',
      price: 19.99,
      imageUrl: 'assets/scale.jpg'
    },
    {
      id: 3,
      name: 'Eraser',
      description: 'An eraser, piece of rubber or other material used to rub out marks made by ink, pencil, or chalk. The modern eraser is usually a mixture of an abrasive such as fine pumice, a rubbery matrix such as synthetic rubber or vinyl, and other ingredients.',
      price: 29.99,
      imageUrl: 'assets/eraser.jpg'
    }
  ];
 
  viewDetails(product: any): void {
    this.productSelected.emit(product);
  }
}
 
 
<!-- <app-product-details></app-product-details>
<app-product-list></app-product-list> -->
 
<h1>Stationery Management System</h1>
 
<div class="container">
 
  <div class="left-panel">
    <app-product-list (productSelected)="onProductSelected($event)">
    </app-product-list>
  </div>
 
  <div class="right-panel">
    <app-product-details [selectedProduct]="selectedProduct">
    </app-product-details>
  </div>
 
</div>
 
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
 
  title = 'angularapp';
 
  selectedProduct: any;
 
  onProductSelected(product: any): void {
    this.selectedProduct = product;
  }
}
 
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
 
@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ProductDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
