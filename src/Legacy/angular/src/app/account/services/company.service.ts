import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Company, CompanyInfo} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class CompanyService {
  private baseUrl: string = environment.applicationUrl + "account/Company";

  constructor(private http: HttpClient) {
  }

  // Obtener todas las empresas
  getCompanies(query?: string): Observable<Company[]> {
    const url: string = `${this.baseUrl}?query=${query || ""}`;
    return this.http.get<Company[]>(url);
  }

  // Obtener una empresa por ID
  getCompanyById(id: string): Observable<Company> {
    const url: string = `${this.baseUrl}/${id}`;
    return this.http.get<Company>(url);
  }

  // Obtener información básica de una empresa
  getCompanyInfo(id: string): Observable<CompanyInfo> {
    const url: string = `${this.baseUrl}/Info/${id}`;
    return this.http.get<CompanyInfo>(url);
  }

  // Crear una nueva empresa
  createCompany(company: Company): Observable<Company> {
    return this.http.post<Company>(this.baseUrl, company);
  }

  // Actualizar una empresa existente por ID
  updateCompany(id: string, company: Company): Observable<Company> {
    const url: string = `${this.baseUrl}/${id}`;
    return this.http.put<Company>(url, company);
  }

  // Eliminar una empresa por ID
  deleteCompany(id: string): Observable<Company> {
    const url: string = `${this.baseUrl}/${id}`;
    return this.http.delete<Company>(url);
  }

  subirCertificado(id: string, data: FormData): Observable<Company> {
    const url: string = `${this.baseUrl}/SubirCertificado/${id}`;
    return this.http.post<Company>(url, data);
  }

  sincronizarDatos(id: string): Observable<Company> {
    const url: string = `${this.baseUrl}/SincronizarDatos/${id}`;
    return this.http.patch<Company>(url, {});
  }

  quitarCertificado(id: string): Observable<Company> {
    const url: string = `${this.baseUrl}/QuitarCertificado/${id}`;
    return this.http.patch<Company>(url, {});
  }

  cambiarSunatEndpoint(id: string): Observable<Company> {
    const url: string = `${this.baseUrl}/CambiarSunatEndpoint/${id}`;
    return this.http.patch<Company>(url, {});
  }

}
