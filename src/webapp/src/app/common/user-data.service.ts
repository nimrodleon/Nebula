import {inject, Injectable} from "@angular/core";
import {Title} from "@angular/platform-browser";
import {AuthService} from "../account/user/services";
import {UserAuth, UserAuthConfig, UserRole} from "../account/user/interfaces";
import {LocalSummaryDto} from "../account/interfaces";

@Injectable({
  providedIn: "root"
})
export class UserDataService {
  private _userAuthConfig: UserAuthConfig = new UserAuthConfig();
  private title: Title = inject(Title);
  private authService: AuthService = inject(AuthService);

  public cargarData(): void {
    this.authService.getUserData()
      .subscribe(result => {
        this._userAuthConfig = result;
      });
  }

  public get userAuth(): UserAuth {
    return this._userAuthConfig.userAuth;
  }

  public get localDefault(): number {
    return this._userAuthConfig.userAuth.localDefault;
  }

  public get localName(): string {
    return this._userAuthConfig.localName;
  }

  public get locales(): LocalSummaryDto[] {
    return this._userAuthConfig.locales;
  }

  public canDelete(): boolean {
    return this._userAuthConfig.userAuth.userRole === UserRole.Admin;
  }

  public isUserAdmin(): boolean {
    return this._userAuthConfig.userAuth.userRole === UserRole.Admin;
  }

  public isBusinessType(): boolean {
    return this._userAuthConfig.userAuth.accountType === "business";
  }

  public logout(): void {
    this.authService.logout();
    this.title.setTitle("Login");
  }

}
