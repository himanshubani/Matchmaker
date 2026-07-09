c/app/services/jwt.service.ts =========
 
import { Injectable } from '@angular/core';
 
@Injectable({
  providedIn: 'root'
})
export class JwtService {
  private readonly TOKEN_KEY = 'jwt_token';
 
  saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }
 
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }
 
  destroyToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }
 
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
 
 
src/app/services/auth.service.ts
 
 
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtService } from './jwt.service';
 
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://8080-...premiumproject.examly.io/api/login';
 
  constructor(private http: HttpClient, private jwtService: JwtService) {}
 
  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl, { username, password });
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
 
app/components/authguard/auth.guard.ts
 
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}
 
  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
}
 
 
dashboard/dashboard.component.ts
 
 
 
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(private authService: AuthService, private router: Router) {}
 
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
dashboard.component.html
 
<h1>Dashboard</h1>
<p>Welcome to your dashboard!</p>
<button (click)="logout()">Logout</button>
 
 
login/login.component.ts
 
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
 
  constructor(private authService: AuthService, private router: Router) {}
 
  login(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response: any) => {
        if (response && response.token) {
          this.authService.storeToken(response.token);
        }
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage = 'Invalid username or password';
      }
    });
  }
}
 
in/login.component.html
 
 
<h1>Login</h1>
<input type="text" placeholder="Username" [(ngModel)]="username" name="username" />
<br>
<input type="password" placeholder="Password" [(ngModel)]="password" name="password" />
<br>
<button (click)="login()">Login</button>
<p style="color: red;" *ngIf="errorMessage">{{ errorMessage }}</p>
 
 
p/app-routing.module.ts =======
 
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './components/authguard/auth.guard';
 
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '/login' }
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
 
 
c/app/app.module.ts==================
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './components/authguard/auth.guard';
import { AuthService } from './services/auth.service';
import { JwtService } from './services/jwt.service';
 
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [AuthGuard, AuthService, JwtService],
  bootstrap: [AppComponent]
})
export class AppModule {}
 
 
/app.component.html
 
<router-outlet></router-outlet>
