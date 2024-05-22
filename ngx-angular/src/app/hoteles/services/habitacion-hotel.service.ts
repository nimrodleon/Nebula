import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { UserDataService } from 'app/common/user-data.service';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { HabitacionHotel } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class HabitacionHotelService {
  private controller: string = "HabitacionHotel";
  private baseUrl: string = environment.applicationUrl + "hoteles";
  private http: HttpClient = inject(HttpClient);
  private userDataService = inject(UserDataService);

  public index(query: string = ""): Observable<HabitacionHotel[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<HabitacionHotel[]>(url, { params });
  }

  public show(id: string): Observable<HabitacionHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<HabitacionHotel>(url);
  }

  public create(data: HabitacionHotel): Observable<HabitacionHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<HabitacionHotel>(url, data);
  }

  public update(id: string, data: HabitacionHotel): Observable<HabitacionHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<HabitacionHotel>(url, data);
  }

  public delete(id: string): Observable<HabitacionHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<HabitacionHotel>(url);
  }

}
