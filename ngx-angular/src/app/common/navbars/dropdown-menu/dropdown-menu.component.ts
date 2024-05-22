import { Component, Input, inject } from "@angular/core";
import { faShareNodes } from "@fortawesome/free-solid-svg-icons";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { RouterModule } from "@angular/router";
import { UserDataService } from "app/common/user-data.service";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-dropdown-menu",
  standalone: true,
  imports: [
    RouterModule,
    FaIconComponent,
    NgIf,
  ],
  templateUrl: "./dropdown-menu.component.html"
})
export class DropdownMenuComponent {
  private userDataService: UserDataService = inject(UserDataService);
  faShareNodes = faShareNodes;
  @Input()
  companyId: string = "";

  public get isEnableModInventarios(): boolean {
    return this.userDataService.isEnableModInventarios();
  }

  public get isEnableModComprobantes(): boolean {
    return this.userDataService.isEnableModComprobantes();
  }

  public get isEnableModCuentaPorCobrar(): boolean {
    return this.userDataService.isEnableModCuentaPorCobrar();
  }

  public get isEnableModReparaciones(): boolean {
    return this.userDataService.isEnableModReparaciones();
  }

  public get isEnableModCajasDiaria(): boolean {
    return this.userDataService.isEnableModCajasDiaria();
  }

}
