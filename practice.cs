character-count.pipe
import { Pipe, PipeTransform } from '@angular/core';
 
@Pipe({
  name: 'characterCounter'
})
export class CharacterCounterPipe implements PipeTransform {
 
  transform(value:string):number{
    return value ? value.length : 0;
  }
}
===============
character-count.component.ts
===
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-character-count',
  templateUrl: './character-count.component.html',
  styleUrls: ['./character-count.component.css']
})
export class CharacterCountComponent {
      inputText:string='';
}
======
character-count.component.html
<div>
<input type="text" [(ngModel)]="inputText" placeholder="Enter text">
</div>
<div>
<p>Number of characters: {{inputText |characterCounter}}</p>
</div>
