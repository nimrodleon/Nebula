export interface AuthLogin {
  userName: string,
  password: string,
}

export function AuthLoginDefaultValues(): AuthLogin {
  return {
    userName: '',
    password: ''
  };
}
