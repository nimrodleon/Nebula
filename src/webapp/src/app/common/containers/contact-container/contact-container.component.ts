import {Component} from "@angular/core";
import {ContactNavbarComponent} from "../../navbars/contact-navbar/contact-navbar.component";

@Component({
  selector: "app-contact-container",
  standalone: true,
  imports: [
    ContactNavbarComponent
  ],
  templateUrl: "./contact-container.component.html"
})
export class ContactContainerComponent {

}
