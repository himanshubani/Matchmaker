// ==============================
// FILE: src/app/guards/auth.guard.ts
// ==============================
 
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
 
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }
 
  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }
 
    this.router.navigate(['/login']);
    return false;
  }
}
 
 
// ==============================
// FILE: src/app/services/auth.service.ts
// ==============================
 
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtService } from './jwt.service';
 
@Injectable({
  providedIn: 'root'
})
export class AuthService {
 
  private apiUrl = 'api/login';
 
  constructor(
    private http: HttpClient,
    private jwtService: JwtService
  ) { }
 
  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl, {
      username,
      password
    });
  }
 
  storeToken(token: string): void {
    this.jwtService.saveToken(token);
  }
 
  logout(): void {
    this.jwtService.destroyToken();
  }
 
  isLoggedIn(): boolean {
    return this.jwtService.isLoggedIn();
  }
}
 
 
// ==============================
// FILE: src/app/services/jwt.service.ts
// ==============================
 
import { Injectable } from '@angular/core';
 
@Injectable({
  providedIn: 'root'
})
export class JwtService {
 
  saveToken(token: string): void {
    localStorage.setItem('jwt_token', token);
  }
 
  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }
 
  destroyToken(): void {
    localStorage.removeItem('jwt_token');
  }
 
  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }
}
 
 
// ==============================
// FILE: src/app/components/dashboard/dashboard.component.ts
// ==============================
 
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
 
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }
 
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
 
 
// ==============================
// FILE: src/app/components/dashboard/dashboard.component.html
// ==============================
 
<h2>Dashboard</h2>
<button (click)="logout()">Logout</button>
 
 
// ==============================
// FILE: src/app/components/login/login.component.ts
// ==============================
 
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
 
  username = '';
  password = '';
 
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }
 
  login(): void {
    this.authService.login(this.username, this.password)
      .subscribe((response: any) => {
        this.authService.storeToken(response.token);
        this.router.navigate(['/dashboard']);
      });
  }
}
 
 
// ==============================
// FILE: src/app/components/login/login.component.html
// ==============================
 
<h2>Login</h2>
 
<input [(ngModel)]="username" placeholder="Username">
 
<input
  type="password"
  [(ngModel)]="password"
  placeholder="Password">
 
<button (click)="login()">Login</button>
 
 
// ==============================
// FILE: src/app/app-routing.module.ts
// ==============================
 
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
 
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
 
 
// ==============================
// FILE: src/app/app.module.ts
// ==============================
 
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
 
imports: [
  BrowserModule,
  FormsModule,
  HttpClientModule
]
