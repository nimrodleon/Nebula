import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {Collaborator, CollaboratorResponse, InviteCollaborator, InvoiceSerie, Warehouse} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class CollaboratorService {
  private controller: string = "Collaborator";
  private baseUrl: string = environment.applicationUrl + "auth";

  constructor(private http: HttpClient,) {
  }

  public index(companyId: string = ""): Observable<CollaboratorResponse[]> {
    const url: string = `${this.baseUrl}/${this.controller}/${companyId}`;
    return this.http.get<CollaboratorResponse[]>(url);
  }

  public invite(data: InviteCollaborator): Observable<any> {
    const url: string = `${this.baseUrl}/${this.controller}/Invite`;
    return this.http.post<any>(url, data);
  }

  public validar(uuid: string): Observable<any> {
    const url: string = `${this.baseUrl}/${this.controller}/Validate/${uuid}`;
    return this.http.post<any>(url, {});
  }

  public update(id: string, data: Collaborator): Observable<Collaborator> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<Collaborator>(url, data);
  }

  public delete(id: string): Observable<Collaborator> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<Collaborator>(url);
  }

}
