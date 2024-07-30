import {Component} from "@angular/core";
import {ReceivableNavbarComponent} from "../../navbars/receivable-navbar/receivable-navbar.component";

@Component({
  selector: "app-receivable-container",
  standalone: true,
  imports: [
    ReceivableNavbarComponent
  ],
  templateUrl: "./receivable-container.component.html"
})
export class ReceivableContainerComponent  {

}
