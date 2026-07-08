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




