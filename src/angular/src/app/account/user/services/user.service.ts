import { inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "environments/environment";
import { ForgotPasswordRequest, ResetPasswordRequest, User, UserRegister } from "../interfaces";
import { catchError } from "rxjs/operators";
import { PaginationResult, toastError } from "app/common/interfaces";
import { Company } from "app/account/interfaces";

@Injectable({
  providedIn: "root"
})
export class UserService {
  private baseUrl: string = environment.applicationUrl + "auth/User";
  private http: HttpClient = inject(HttpClient);

  public index(query: string, page: number = 1): Observable<PaginationResult<User>> {
    let params = new HttpParams();
    params = params.append("query", query || "");
    params = params.append("page", page || 1);
    return this.http.get<PaginationResult<User>>(this.baseUrl, { params });
  }

  public show(userId: string): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${userId}/Show`);
  }

  public getCompanies(userId: string): Observable<Company[]> {
    return this.http.get<Company[]>(`${this.baseUrl}/${userId}/Companies`);
  }

  public register(user: UserRegister): Observable<any> {
    return this.http.post(`${this.baseUrl}`, user)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public verifyEmail(token: string): Observable<any> {
    let params = new HttpParams();
    params = params.append("token", token);
    return this.http.get<any>(`${this.baseUrl}/VerifyEmail`, { params });
  }

  public forgotPassword(request: ForgotPasswordRequest): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/ForgotPassword`, request);
  }

  public resetPassword(request: ResetPasswordRequest): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/ResetPassword`, request);
  }

}
