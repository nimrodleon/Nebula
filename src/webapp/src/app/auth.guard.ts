import { CanActivateFn, Router } from "@angular/router";
import { inject } from "@angular/core";
import { AuthService } from "./account/user/services";

export const authGuard: CanActivateFn = () => {
  const router: Router = inject(Router);
  const authService: AuthService = inject(AuthService);
  if (authService.loggedIn()) {
    return true;
  }
  router.navigate(["/login"]).then(() => {
    console.error("Unauthorized");
  });
  return false;
};
