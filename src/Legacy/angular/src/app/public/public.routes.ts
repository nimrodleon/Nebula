import {Routes} from "@angular/router";
import {UserRegisterComponent} from "./pages/user-register/user-register.component";
import {VerifyEmailComponent} from "./pages/verify-email/verify-email.component";
import {ValidateEmployeeComponent} from "./pages/validate-employee/validate-employee.component";

export const routes: Routes = [
  {path: "user/register", component: UserRegisterComponent},
  {path: "user/verify-email", component: VerifyEmailComponent},
  {path: "collaborator/validate", component: ValidateEmployeeComponent},
];
