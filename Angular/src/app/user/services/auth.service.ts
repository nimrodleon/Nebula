import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient} from '@angular/common/http';
import {AuthLogin} from '../interfaces';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private appURL: string = environment.applicationUrl + 'Auth';

  constructor(
    private router: Router,
    private http: HttpClient) {
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
