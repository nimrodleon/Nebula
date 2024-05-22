import { inject, Injectable } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { AuthService } from "../account/user/services";
import { CompanyRoles, User, UserAuth, UserCompanyRole, UserTypeSystem } from "../account/user/interfaces";
import { Company } from "../account/interfaces";
import _ from "lodash";

@Injectable({
  providedIn: "root"
})
export class UserDataService {
  private _userAuth: UserAuth = new UserAuth();
  private _companyId: string = "";
  private title: Title = inject(Title);
  private authService: AuthService = inject(AuthService);

  get companyId(): string {
    return this._companyId;
  }

  set companyId(value: string) {
    this._companyId = value;
  }

  public cargarData(): void {
    this.authService.getUserData()
      .subscribe(result => {
        this._userAuth = result;
      });
  }

  public get userAuth(): User {
    return this._userAuth.user;
  }

  public get companies(): Company[] {
    return this._userAuth.companies;
  }

  public get currentCompany(): Company {
    return _.find(this.companies, (item: Company) =>
      item.id === this._companyId) || new Company();
  }

  public get companyName(): string {
    return `${this.currentCompany.ruc} - ${this.currentCompany.rznSocial}`;
  }

  private getUserCompanyRole(): UserCompanyRole | undefined {
    return _.find(this._userAuth.companyRoles, (item: UserCompanyRole) => item.companyId === this._companyId);
  }

  public canDelete(): boolean {
    const userCompanyRole: UserCompanyRole | undefined = this.getUserCompanyRole();
    if (userCompanyRole === undefined) return false;
    const arrRole: string[] = userCompanyRole.userRole.split(":");
    const owner: boolean = _.includes(arrRole, CompanyRoles.Owner);
    const admin: boolean = _.includes(arrRole, CompanyRoles.Admin);
    return owner || admin;
  }

  public hasPermission(): boolean {
    const userCompanyRole = this.getUserCompanyRole();
    if (!userCompanyRole) return false;
    const userRoles = userCompanyRole.userRole.split(":");
    return userRoles.includes(CompanyRoles.Owner) || userRoles.includes(CompanyRoles.Admin);
  }

  public logout(): void {
    this.authService.logout();
    this.title.setTitle("Login");
  }

  public isUserTypeAdmin(): boolean {
    return this._userAuth.user.userType === UserTypeSystem.Admin;
  }

  public isUserTypeDistributor(): boolean {
    return this._userAuth.user.userType === UserTypeSystem.Distributor;
  }

  public isUserTypeCustomer(): boolean {
    return this._userAuth.user.userType === UserTypeSystem.Customer;
  }

  public isEnableModInventarios(): boolean {
    return this.currentCompany.modInventarios === true;
  }

  public isEnableModComprobantes(): boolean {
    return this.currentCompany.modComprobantes === true;
  }

  public isEnableModCuentaPorCobrar(): boolean {
    return this.currentCompany.modCuentaPorCobrar === true;
  }

  public isEnableModReparaciones(): boolean {
    return this.currentCompany.modReparaciones === true;
  }

  public isEnableModCajasDiaria(): boolean {
    return this.currentCompany.modCajasDiaria === true;
  }

}
