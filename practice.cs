import { Pipe, PipeTransform } from '@angular/core';
 
@Pipe({
 
  name: 'searchFilter'
 
})
 
export class SearchFilterPipe implements PipeTransform {
 
  transform(items: any[], searchTerm: string): any[] {
 
    if (!items || !searchTerm) {
 
    return items;
 
    }
 
    searchTerm = searchTerm.toLowerCase();
 
    return items.filter(item =>
 
    JSON.stringify(item).toLowerCase().includes(searchTerm)
 
    );
 
  }
 
}
===============
.ts
import { Component } from '@angular/core';
 
@Component({
 
  selector: 'app-search',
 
  templateUrl: './search.component.html',
 
  styleUrls: ['./search.component.css']
 
})
 
export class SearchComponent {
 
  searchText: string = '';
 
  items = [
 
    { id: 1, name: 'Apple', category: 'Fruit' },
 
    { id: 2, name: 'Banana', category: 'Fruit' },
 
    { id: 3, name: 'Carrot', category: 'Vegetable' }
 
  ];
 
}
=========
.html
<input type="text" [(ngModel)]="searchText" placeholder="Search..." />
 
<div>
 
<ul>
 
    <li *ngFor="let item of items | searchFilter: searchText">
 
    {{ item.name }} - {{ item.category }}
 
    </li>
 
</ul>
 
</div>
