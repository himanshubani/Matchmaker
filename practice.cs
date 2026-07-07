recipe.model.ts
export class Recipe{
    id:number;
    name:string;
    type:string;
    ingredients:string[];
    instructions:string;
    constructor(
        id:number,
        name:string,
        type:string,
        ingredients:string[],
        instructions:string
    ){
        this.id=id;
        this.name=name;
        this.type=type;
        this.ingredients=ingredients;
        this.instructions=instructions;
    }
}
============
recipe-list.component.ts
import { Component } from '@angular/core';
//import { Recipe } from '../model/recipe.model';
export interface Recipe{
       id:number,
       name:string,
      type:string,
       ingredients:string[],
     instructions:string
}
@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent {
  recipes: Recipe[]=[
    {
    id:1,
    name:'Pancakes',
    type:'Breakfast',
    ingredients:['Flour','Milk','Eggs','Butter'],
    instructions:'Mix flour,milk and eggs.Cook in a pan with butter.'
    },
    {
      id:2,
      name:'Spaghetti Carbonara',
      type:'Dinner',
      ingredients:['Spaghetti','Eggs','Bacon','Parmesan cheese'],
      instructions:'Mix flour,milk and eggs.Cook in a pan with butter.'
    }

 
  ];
  selectedRecipe: Recipe|null =null;
  showDetails(recipe:Recipe): void{
      this.selectedRecipe=recipe;
  }
  hideDetails(){
    this.selectedRecipe=null;
  }
  deleteRecipe(recipe:Recipe){
     this.recipes=this.recipes.filter(r=>r.id!==recipe.id);
     if(this.selectedRecipe?.id===recipe.id){
      this.selectedRecipe=null;
     }
  }
 
}
=============
recipe-list.component.html
<h1 class="heading">Recipe Manager</h1>
<div class="recipe-list">
<div *ngFor="let recipe of recipes" class="recipe">
<h3>{{recipe.name}}({{recipe.type}})</h3>
<p>{{recipe.ingredients.join(', ')}}</p>
<button (click)="showDetails(recipe)">View Details</button>
</div>
</div>
<div *ngIf="selectedRecipe" class="recipe-details-container">
<h2>Recipe Details</h2>
<p><strong>Name:</strong>{{selectedRecipe.name}}</p>
<p><strong>Type:</strong>{{selectedRecipe.type}}</p>
<p><strong>Ingredients:</strong>{{selectedRecipe.ingredients.join(', ')}}</p>
<p><strong>Instructions:</strong>{{selectedRecipe.instructions}}</p>
<button (click)="hideDetails()">Hide Details</button>
<button (click)="deleteRecipe(selectedRecipe)">Delete</button>
</div>
