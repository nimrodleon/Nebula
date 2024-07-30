import {Component} from "@angular/core";
import {SalesNavbarComponent} from "../../navbars/sales-navbar/sales-navbar.component";

@Component({
  selector: "app-sales-container",
  standalone: true,
  imports: [
    SalesNavbarComponent
  ],
  templateUrl: "./sales-container.component.html"
})
export class SalesContainerComponent {
}
