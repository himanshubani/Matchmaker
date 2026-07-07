shopping-list.component.ts
import { Component } from '@angular/core';
 
interface Item {
 
  name: string;
 
  purchased: boolean;
 
}
 
@Component({
 
  selector: 'app-shopping-list',
 
  templateUrl: './shopping-list.component.html',
 
  styleUrls: ['./shopping-list.component.css']
 
})
 
export class ShoppingListComponent {
 
  items: Item[] = [];
 
  newItemName: string = '';
 
  addItem(): void {
 
    if (this.newItemName.trim() !== '') {
 
    this.items.push({
 
    name: this.newItemName,
 
    purchased: false
 
    });
 
    this.newItemName = '';
 
    }
 
  }
 
  purchaseItem(item: Item): void {
 
    item.purchased = !item.purchased;
 
  }
 
  deleteItem(index: number): void {
 
    this.items.splice(index, 1);
 
  }
 
}
===========
shopping-list.component.html
<h2>Shopping List</h2>
 
<input
 
  type="text"
 
  [(ngModel)]="newItemName"
 
  placeholder="Enter item name">
 
<button (click)="addItem()">Add Item`</button>`
 
<ul>
 
<li *ngFor="let item of items; let i = index">
 
    <span>{{ item.name }}</span>
 
    <button (click)="purchaseItem(item)">
 
    Purchased
 
    </button>
 
    <button (click)="deleteItem(i)">
 
    Delete
 
    </button>
 
</li>
 
</ul>
 
==========
app.component.html
<app-shopping-list></app-shopping-list>
