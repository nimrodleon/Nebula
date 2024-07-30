import {inject, Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PaginationResult, toastError} from "../../../common/interfaces";
import {User, UserRegisterPersonal} from "../interfaces";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class UserPersonalService {
  private baseUrl: string = environment.applicationUrl + "auth/UserPersonal";
  private http: HttpClient = inject(HttpClient);

  public index(query: string, page: number = 1): Observable<PaginationResult<User>> {
    let params = new HttpParams();
    params = params.append("query", query || "");
    params = params.append("page", page || 1);
    return this.http.get<PaginationResult<User>>(this.baseUrl, {params});
  }

  public create(user: UserRegisterPersonal): Observable<any> {
    return this.http.post(`${this.baseUrl}`, user)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public update(id: number, user: UserRegisterPersonal): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, user)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public changePassword(id: number, password: string): Observable<any> {
    return this.http.patch(`${this.baseUrl}/${id}/change-password`, {password})
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public delete(id: number): Observable<any> {
    const url: string = `${this.baseUrl}/${id}`;
    return this.http.delete(url);
  }

}
