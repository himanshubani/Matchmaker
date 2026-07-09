1. src/app/models/login.model.ts

export interface Login {

  username: string;

  password: string;

}
 
2. src/app/services/auth.service.ts

import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Login } from '../models/login.model';
 
@Injectable({

  providedIn: 'root'

})

export class AuthService {

  private apiUrl = 'https://8080-...premiumproject.examly.io/api/login';
 
  constructor(private http: HttpClient) {}
 
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

``

/auth.guard.ts
 
 
import { Injectable } from '@angular/core';

import { CanActivate, Router } from '@angular/router';

import { AuthService } from '../services/auth.service';
 
@Injectable({

  providedIn: 'root'

})

export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}
 
  canActivate(): boolean {

    if (this.authService.isAuthenticated()) {

      return true;

    }

    this.router.navigate(['/error']);

    return false;

  }

}
 
adminpage.component.ts
 
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
 
@Component({

  selector: 'app-adminpage',

  templateUrl: './adminpage.component.html',

  styleUrls: ['./adminpage.component.css']

})

export class AdminpageComponent {

  constructor(private authService: AuthService, private router: Router) {}
 
  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}
 
 
adminpage.component.html
 
 
<h1>Admin Page</h1>
<p>Welcome Admin!</p>
<button (click)="logout()">Logout</button>
 
 
login.component.ts
 
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

  loginData: Login = { username: '', password: '' };

  errorMessage: string = '';
 
  constructor(private authService: AuthService, private router: Router) {}
 
  login(): void {

    this.authService.login(this.loginData).subscribe({

      next: () => {

        localStorage.setItem('isLoggedIn', 'true');

        this.router.navigate(['/admin']);

      },

      error: () => {

        this.errorMessage = 'Invalid username or password';

      }

    });

  }

}
 
login.component.html
 
<h1>Login</h1>
<input type="text" placeholder="Username" [(ngModel)]="loginData.username" name="username" />
<br>
<input type="password" placeholder="Password" [(ngModel)]="loginData.password" name="password" />
<br>
<button (click)="login()">Login</button>
<p style="color: red;" *ngIf="errorMessage">{{ errorMessage }}</p>
 
 
/error.component.html
 
<h1>Unauthorized Access</h1>
<p>You are not authorized to view this page.</p>
<a routerLink="/login">Go to Login</a>
 
error.component.ts
 
import { Component } from '@angular/core';
 
@Component({

  selector: 'app-error',

  templateUrl: './error.component.html',

  styleUrls: ['./error.component.css']

})

export class ErrorComponent {}
 
 
app-routing.module.ts
 
import { NgModule } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';

import { AdminpageComponent } from './components/adminpage/adminpage.component';

import { ErrorComponent } from './components/error/error.component';

import { AuthGuard } from './authguard/auth.guard';
 
const routes: Routes = [

  { path: '', redirectTo: '/login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },

  { path: 'admin', component: AdminpageComponent, canActivate: [AuthGuard] },

  { path: 'error', component: ErrorComponent },

  { path: '**', redirectTo: '/login' }

];
 
@NgModule({

  imports: [RouterModule.forRoot(routes)],

  exports: [RouterModule]

})

export class AppRoutingModule {}
 
/app.module.ts
 
import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';

import { FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
 
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

import { LoginComponent } from './components/login/login.component';

import { AdminpageComponent } from './components/adminpage/adminpage.component';

import { ErrorComponent } from './components/error/error.component';

import { AuthGuard } from './authguard/auth.guard';

import { AuthService } from './services/auth.service';
 
@NgModule({

  declarations: [

    AppComponent,

    LoginComponent,

    AdminpageComponent,

    ErrorComponent

  ],

  imports: [

    BrowserModule,

    FormsModule,

    HttpClientModule,

    AppRoutingModule

  ],

  providers: [AuthGuard, AuthService],

  bootstrap: [AppComponent]

})

export class AppModule {}
 
app.component.html
 
<router-outlet></router-outlet>
 
