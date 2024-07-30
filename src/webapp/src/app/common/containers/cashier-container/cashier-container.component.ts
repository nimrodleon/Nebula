import {Component} from "@angular/core";
import {CashierNavbarComponent} from "../../navbars/cashier-navbar/cashier-navbar.component";

@Component({
  selector: "app-cashier-container",
  standalone: true,
  imports: [
    CashierNavbarComponent
  ],
  templateUrl: "./cashier-container.component.html"
})
export class CashierContainerComponent  {
}
