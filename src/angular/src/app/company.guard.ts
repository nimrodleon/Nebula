import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from "@angular/router";
import {inject} from "@angular/core";
import {UserDataService} from "./common/user-data.service";

export const companyGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
  const companyId: string = route.paramMap.get("companyId") || "";
  if (companyId !== "") {
    const _userDataService: UserDataService = inject(UserDataService);
    _userDataService.companyId = companyId;
    return true;
  } else {
    const _router: Router = inject(Router);
    _router.navigate(["/account"]).then(() => console.log("error"));
    return false;
  }
};
