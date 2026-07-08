food.model.ts
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

  public apiUrl = 'http://localhost:3000/foods';

  constructor(private http: HttpClient) { }

  getFoods(): Observable<Food[]> {
    return this.http.get<Food[]>(this.apiUrl);
  }

  addFood(food: Food): Observable<Food> {
    return this.http.post<Food>(this.apiUrl, food);
  }

  updateFood(food: Food): Observable<Food> {
    return this.http.put<Food>(
      `${this.apiUrl}/${food.id}`,
      food
    );
  }

  deleteFood(foodId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${foodId}`
    );
  }
}

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
    this.foodService.getFoods().subscribe(
      (data: Food[]) => {
        this.foods = data;
      }
    );
  }

  addFood(): void {

    if (this.newFood.id === 0) {

      const food: Food = {
        id: Date.now(),
        name: this.newFood.name,
        description: this.newFood.description
      };

      this.foodService.addFood(food).subscribe(() => {

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

    this.foodService.updateFood(this.newFood)
      .subscribe(() => {

        this.loadFoods();

        this.newFood = {
          id: 0,
          name: '',
          description: ''
        };
      });
  }

  deleteFood(foodId: number): void {

    this.foodService.deleteFood(foodId)
      .subscribe(() => {

        this.loadFoods();
      });
  }
}

food-list.component.html
<h2>Food List</h2>

<div *ngFor="let food of foods">

  {{ food.name }} - {{ food.description }}

  <button
    type="button"
    (click)="deleteFood(food.id)">
    Delete
  </button>

  <button
    type="button"
    (click)="editFood(food)">
    Edit
  </button>

</div>

<h2>Add Food</h2>

<form (ngSubmit)="addFood()">

  <label>Name:</label>

  <input
    type="text"
    name="name"
    [(ngModel)]="newFood.name">

  <br><br>

  <label>Description:</label>

  <textarea
    name="description"
    [(ngModel)]="newFood.description">
  </textarea>

  <br><br>

  <button type="submit">
    Add Food
  </button>

</form>

app.component.html
<app-food-list></app-food-list>

db.json
{
  "foods": [
    {
      "id": 1,
      "name": "Dosa",
      "description": "A popular South Indian dish made from fermented rice and lentil batter, served with chutney and sambar."
    },
    {
      "id": 2,
      "name": "Idli",
      "description": "Soft and fluffy steamed rice cakes, usually served with coconut chutney and sambar."
    },
    {
      "id": 3,
      "name": "Vada",
      "description": "Deep-fried savory donut made from fermented lentil batter, served with chutney and sambar."
    },
    {
      "id": 4,
      "name": "Masala Dosa",
      "description": "Dosa filled with a spicy potato filling, served with chutney and sambar."
    }
  ]
}














































