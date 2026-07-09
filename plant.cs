1. models/login.model.ts
export interface Login {
  username: string;
  password: string;
}

2. services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://8080--premiumproject.examly.io/api/login';

  constructor(private http: HttpClient) { }

  login(loginData: Login): Observable<any> {
    return this.http.post<any>(this.apiUrl, loginData);
  }

  isAuthenticated(): boolean {
    return localStorage.getItem('isLoggedIn') === 'true';
  }

  logout(): void {
    localStorage.removeItem('isLoggedIn');
  }
}

3. authguard/auth.guard.ts
import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(): boolean {

    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/error']);
    return false;
  }
}

4. components/userpage/userpage.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-userpage',
  templateUrl: './userpage.component.html',
  styleUrls: ['./userpage.component.css']
})
export class UserpageComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

5. components/userpage/userpage.component.html
<h1>User Page</h1>

<h2>Welcome User!</h2>

<button (click)="logout()">
  Logout
</button>

6. components/login/login.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginData: Login = {
    username: '',
    password: ''
  };

  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {

    this.authService.login(this.loginData).subscribe({

      next: (response) => {

        localStorage.setItem('isLoggedIn', 'true');
        this.router.navigate(['/user']);
      },

      error: () => {

        this.errorMessage = 'Invalid username or password';
      }
    });
  }
}
`
7. components/login/login.component.html
<h1>Login</h1>

<div>
  <input
    type="text"
    placeholder="Username"
    [(ngModel)]="loginData.username">
</div>

<br>

<div>
  <input
    type="password"
    placeholder="Password"
    [(ngModel)]="loginData.password">
</div>

<br>

<button (click)="login()">
  Login
</button>

<p
  *ngIf="errorMessage"
  style="color:red">
  {{ errorMessage }}
</p>

8. components/error/error.component.html
<h1>Unauthorized Access</h1>

<p>
  You are not authorized to view this page.
</p>

<a routerLink="/login">
  Go to Login
</a>

9. app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { UserpageComponent } from './components/userpage/userpage.component';
import { ErrorComponent } from './components/error/error.component';

import { AuthGuard } from './authguard/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'user',
    component: UserpageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'error',
    component: ErrorComponent
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

10. app.module.ts
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
imports: [
  BrowserModule,
  AppRoutingModule,
  FormsModule,
  HttpClientModule
]
declarations: [
  AppComponent,
  LoginComponent,
  UserpageComponent,
  ErrorComponent
]

11. app.component.html
<router-outlet></router-outlet>


































