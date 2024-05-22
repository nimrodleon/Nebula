import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PaginationResult } from 'app/common/interfaces';
import { UserDataService } from 'app/common/user-data.service';
import { environment } from 'environments/environment';
import { HabitacionDisponible } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecepcionService {
  private controller: string = "Recepcion";
  private baseUrl: string = environment.applicationUrl + "hoteles";
  private http: HttpClient = inject(HttpClient);
  private userDataService: UserDataService = inject(UserDataService);

  public getHabitacionesDisponibles(query: string, page: number = 1): Observable<PaginationResult<HabitacionDisponible>> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/HabitacionesDisponibles`;
    let params = new HttpParams();
    params = params.append("query", query || "");
    params = params.append("page", page || 1);
    return this.http.get<PaginationResult<HabitacionDisponible>>(url, { params });
  }

}
