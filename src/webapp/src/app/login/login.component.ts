import {Component, OnInit} from "@angular/core";
import {Router, RouterLink} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {faLock, faSignInAlt, faUser} from "@fortawesome/free-solid-svg-icons";
import Swal from "sweetalert2";
import {AuthLogin} from "../account/user/interfaces";
import {AuthService} from "../account/user/services";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  standalone: true,
  imports: [
    FormsModule,
    FaIconComponent,
    RouterLink
  ],
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent implements OnInit {
  faUser = faUser;
  faLock = faLock;
  faSignInAlt = faSignInAlt;
  user: AuthLogin = new AuthLogin();

  constructor(
    private router: Router,
    private authService: AuthService) {
  }

  ngOnInit(): void {
    // verificar usuario autentificado.
    if (this.authService.loggedIn()) {
      this.router.navigate(["/account"]).then(() => {
        console.info("usuario autentificado!");
      });
    }
  }

  public async authLogin() {
    this.authService.login(this.user).subscribe((res: any) => {
      localStorage.setItem("token", res.token);
      window.location.href = "/account";
    }, (err: any) => {
      Swal.fire({
        icon: "error", title: "Oops...", text: err.error.Login[0]
      });
    });
  }

}
