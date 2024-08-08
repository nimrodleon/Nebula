import { Company } from "../../interfaces";

export class UserTypeSystem {
  static Admin: string = "admin";
  static Distributor: string = "distributor";
  static Customer: string = "customer";
}

export class CompanyRoles {
  static Owner: string = "owner";
  static Admin: string = "admin";
  static User: string = "user";
}

export class User {
  id: any = undefined;
  userName: string = "";
  email: string = "";
  userType: string = UserTypeSystem.Customer;
  isEmailVerified: boolean = false;
}

export class AuthLogin {
  email: string = "";
  password: string = "";
}

export class UserCompanyRole {
  companyId: string = "";
  userRole: string = CompanyRoles.User;
}

export class ForgotPasswordRequest {
  email: string = "";
}

export class ResetPasswordRequest {
  token: string = "";
  newPassword: string = "";
}

export class UserRegister {
  userName: string = "";
  email: string = "";
  password: string = "";
}

export class UserAuth {
  user: User = new User();
  companies: Company[] = [];
  companyRoles: UserCompanyRole[] = [];
}
