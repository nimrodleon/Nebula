import {Component} from "@angular/core";
import {TallerNavbarComponent} from "../../navbars/taller-navbar/taller-navbar.component";

@Component({
  selector: "app-taller-container",
  standalone: true,
  imports: [
    TallerNavbarComponent
  ],
  templateUrl: "./taller-container.component.html"
})
export class TallerContainerComponent  {

}
