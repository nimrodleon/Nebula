import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { UserDataService } from 'app/common/user-data.service';
import { environment } from 'environments/environment';
import { PisoHotel } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PisoHotelService {
  private controller: string = "PisoHotel";
  private baseUrl: string = environment.applicationUrl + "hoteles";
  private http: HttpClient = inject(HttpClient);
  private userDataService = inject(UserDataService);

  public index(query: string = ""): Observable<PisoHotel[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PisoHotel[]>(url, { params });
  }

  public show(id: string): Observable<PisoHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<PisoHotel>(url);
  }

  public create(data: PisoHotel): Observable<PisoHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<PisoHotel>(url, data);
  }

  public update(id: string, data: PisoHotel): Observable<PisoHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<PisoHotel>(url, data);
  }

  public delete(id: string): Observable<PisoHotel> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<PisoHotel>(url);
  }

}
