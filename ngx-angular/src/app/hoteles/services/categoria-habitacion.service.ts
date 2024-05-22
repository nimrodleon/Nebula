import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { UserDataService } from 'app/common/user-data.service';
import { environment } from 'environments/environment';
import { CategoriaHabitacion } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoriaHabitacionService {
  private controller: string = "CategoriaHabitacion";
  private baseUrl: string = environment.applicationUrl + "hoteles";
  private http: HttpClient = inject(HttpClient);
  private userDataService = inject(UserDataService);

  public index(query: string = ""): Observable<CategoriaHabitacion[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<CategoriaHabitacion[]>(url, { params });
  }

  public show(id: string): Observable<CategoriaHabitacion> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<CategoriaHabitacion>(url);
  }

  public create(data: CategoriaHabitacion): Observable<CategoriaHabitacion> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<CategoriaHabitacion>(url, data);
  }

  public update(id: string, data: CategoriaHabitacion): Observable<CategoriaHabitacion> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<CategoriaHabitacion>(url, data);
  }

  public delete(id: string): Observable<CategoriaHabitacion> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<CategoriaHabitacion>(url);
  }

}
