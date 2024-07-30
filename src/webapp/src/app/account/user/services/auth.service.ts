import {inject, Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Router} from "@angular/router";
import {environment} from "environments/environment";
import {AuthLogin, UserAuthConfig} from "../interfaces";
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

  public localChange(idLocal: number): any {
    const params = new HttpParams().set("idLocal", idLocal);
    return this.http.post(`${this.appURL}/LocalChange`, {}, {params});
  }

  public getUserData(): Observable<UserAuthConfig> {
    return this.http.get<UserAuthConfig>(`${this.appURL}/UserData`);
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
