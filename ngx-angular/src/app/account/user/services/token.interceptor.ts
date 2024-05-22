import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "./auth.service";
import { catchError } from "rxjs/operators";

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService);
  const tokenizeReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${authService.getToken()}`,
    }
  });
  return next(tokenizeReq).pipe(catchError(error => {
    if (error instanceof HttpErrorResponse && error.status === 401) {
      authService.logout();
    }
    throw error;
  }));
};
