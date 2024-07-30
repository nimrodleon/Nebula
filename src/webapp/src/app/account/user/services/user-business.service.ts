import {inject, Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient,} from "@angular/common/http";
import {Observable} from "rxjs";
import {toastError} from "../../../common/interfaces";
import {UserRegisterBusiness} from "../interfaces";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class UserBusinessService {
  private baseUrl: string = environment.applicationUrl + "auth/UserBusiness";
  private http: HttpClient = inject(HttpClient);

  public create(user: UserRegisterBusiness): Observable<any> {
    return this.http.post(`${this.baseUrl}`, user)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

}
