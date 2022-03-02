import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {faLock, faSignInAlt, faUser} from '@fortawesome/free-solid-svg-icons';
import {AuthService} from '../user/services';
import {AuthLogin, AuthLoginDefaultValues} from '../user/interfaces';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  faUser = faUser;
  faLock = faLock;
  faSignInAlt = faSignInAlt;
  user: AuthLogin = AuthLoginDefaultValues();

  constructor(
    private router: Router,
    private authService: AuthService) {
  }

  ngOnInit(): void {
    // verificar usuario autentificado.
    if (this.authService.loggedIn()) {
      this.router.navigate(['/']).then(() => {
        console.info('usuario autentificado!');
      });
    }
  }

  public async authLogin() {
    this.authService.login(this.user).subscribe((res: any) => {
      localStorage.setItem('token', res.token);
      this.router.navigate(['/']);
    }, (err: any) => {
      Swal.fire({
        icon: 'error', title: 'Oops...', text: err.error.Login[0]
      });
    });
  }

}
