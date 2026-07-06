<h1>Task Tracker</h1>
<app-task-input (taskAdded)="addTask($event)">
</app-task-input>
<app-task-list [tasks]="tasks" (taskDeleted)="deleteTask($event)">
</app-task-list>
<router-outlet></router-outlet>
 
app.html
 
<h1>Task Input</h1>
 
<input type="text" placeholder="Enter Task Description" [(ngModel)]="taskInput">
<button (click)="addTask()">Add Task</button>
 
task-input.html
 
import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Output } from '@angular/core';
 
@Component({
  selector: 'app-task-input',
  templateUrl: './task-input.component.html',
  styleUrls: ['./task-input.component.css']
})
export class TaskInputComponent {
  taskInput: string = '';
  @Output()
  taskAdded = new EventEmitter<string>();
  addTask(): void{
    if(this.taskInput.trim() != ''){
      this.taskAdded.emit(this.taskInput);
      this.taskInput = '';
    }
  }
 
}
 
uska css
 
js
 
<h1>Task List</h1>
<ul>
    <li *ngFor="let task of tasks; let i = index">
    {{task}}
 
    <button (click)="deleteTask(i)">
        Delete
    </button>
</li>
</ul>
 
task-list.html
 
import { Component, EventEmitter, Input, Output } from '@angular/core';
 
@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  @Input()
  tasks: string[] = [];
  @Output()
  taskDeleted = new EventEmitter<number>();
 
  deleteTask(index: number): void{
    this.taskDeleted.emit(index);
  }
}
 
 
uska .ts
 
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
  tasks: string[] = [];
  addTask(task: string): void{
    this.tasks.push(task);
  }
  deleteTask(index: number): void{
    this.tasks.splice(index, 1);
  }
}
 
appcomponent.ts
