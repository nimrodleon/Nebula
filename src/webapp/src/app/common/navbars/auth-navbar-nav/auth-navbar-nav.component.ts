import {Component, OnInit} from "@angular/core";
import {faCircleQuestion, faSignOutAlt, faUserCircle} from "@fortawesome/free-solid-svg-icons";
import {UserDataService} from "../../user-data.service";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterLink} from "@angular/router";
import {CambiarLocalComponent} from "../../components/cambiar-local/cambiar-local.component";

declare const bootstrap: any;

@Component({
  selector: "app-auth-navbar-nav",
  standalone: true,
  imports: [
    FaIconComponent,
    RouterLink,
    CambiarLocalComponent
  ],
  templateUrl: "./auth-navbar-nav.component.html"
})
export class AuthNavbarNavComponent implements OnInit {
  faCircleQuestion = faCircleQuestion;
  faUserCircle = faUserCircle;
  faSignOutAlt = faSignOutAlt;
  cambiarLocalModal: any;

  constructor(private userDataService: UserDataService) {
  }

  ngOnInit() {
    this.cambiarLocalModal = new bootstrap.Modal("#cambiarLocalModal");
  }

  public get userAuth() {
    return this.userDataService.userAuth;
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  public logout(): void {
    this.userDataService.logout();
  }

  public abrirWhatsapp(e: Event): void {
    e.preventDefault();
    window.open("https://wa.me/51916533482", "_blank");
  }

  public abrirCambiarLocal(): void {
    if (this.userDataService.isBusinessType()) {
      this.cambiarLocalModal.show();
    }
  }

}
