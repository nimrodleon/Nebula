import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {AuthLogin, AuthUser} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private appURL: string = environment.applicationUrl + 'Auth';

  constructor(
    private router: Router,
    private http: HttpClient) {
  }

  // usuario autentificado.
  public getMe(): Observable<AuthUser> {
    return this.http.get<AuthUser>(`${this.appURL}/GetMe`);
  }

  public login(user: AuthLogin): any {
    return this.http.post(`${this.appURL}/Login`, user);
  }

  public loggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  public getToken(): string {
    return localStorage.getItem('token') || '';
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']).then(() => {
      console.info('logout');
    });
  }

}
