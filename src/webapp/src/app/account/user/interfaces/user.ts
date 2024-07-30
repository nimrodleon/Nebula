import {FormType} from "app/common/interfaces";
import {LocalSummaryDto} from "../../interfaces";

export class UserRole {
  static Admin: string = "admin:user";
  static User: string = "user";
}

export class User {
  id: number = 0;
  localDefault: number = 0;
  userName: string = "";
  passwordHash: string = "";
  email: string = "";
  accountType: string = "";
  userRole: string = UserRole.User;
  fullName: string = "";
  phoneNumber: string = "";
}

export class AuthLogin {
  userName: string = "";
  password: string = "";
}

export class ForgotPasswordRequest {
  email: string = "";
}

export class ResetPasswordRequest {
  token: string = "";
  newPassword: string = "";
}

export class UserRegisterPersonal {
  userName: string = "";
  email: string = "";
  userRole: string = UserRole.User;
  fullName: string = "";
  phoneNumber: string = "";
}

export class UserRegisterBusiness {
  userName: string = "";
  email: string = "";
  password: string = "";
  fullName: string = "";
  phoneNumber: string = "";
}

export class UserDataModal {
  title: string = "";
  type: FormType = FormType.ADD;
  user: User = new User();
}

export class UserAuth {
  userId: number = 0;
  userName: string = "";
  accountType: string = "";
  userRole: string = "";
  localDefault: number = 0;
}

export class UserAuthConfig {
  localName: string = "";
  userAuth: UserAuth = new UserAuth();
  locales: LocalSummaryDto[] = [];
}
