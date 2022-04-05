// clase datos del usuario.
export class User {
  constructor(
    public id: string = '',
    public userName: string = '',
    public email: string = '',
    public role: string = '') {
  }
}

// usuario autentificado.
export class AuthUser {
  constructor(
    public userName: string = '',
    public role: 'Admin' | 'User' = 'User') {
  }
}

// clase para registrar usuarios.
export class UserRegister {
  constructor(
    public userName: string = '',
    public email: string = '',
    public password: string = '',
    public role: string = '') {
  }
}

// clase para registrar usuario desde una modal.
export class UserDataModal {
  constructor(
    public title: string = '',
    public type: 'ADD' | 'EDIT' = 'ADD',
    public userId: string = '',
    public userRegister: UserRegister = new UserRegister()) {
  }
}
