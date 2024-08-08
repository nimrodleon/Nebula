import {inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {environment} from "environments/environment";
import {AuthLogin, UserAuth} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  private appURL: string = environment.applicationUrl + "Auth";
  private router: Router = inject(Router);
  private http: HttpClient = inject(HttpClient);

  public login(user: AuthLogin): any {
    return this.http.post(`${this.appURL}/Login`, user);
  }

  public getUserData(): Observable<UserAuth> {
    return this.http.get<UserAuth>(`${this.appURL}/UserData`);
  }

  public loggedIn(): boolean {
    return !!localStorage.getItem("token");
  }

  public getToken(): string {
    return localStorage.getItem("token") || "";
  }

  public logout(): void {
    localStorage.removeItem("token");
    this.router.navigate(["/login"]).then(() => {
      console.info("logout");
    });
  }

}
