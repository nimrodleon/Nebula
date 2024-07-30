import {Component} from "@angular/core";
import {ProductNavbarComponent} from "../../navbars/product-navbar/product-navbar.component";

@Component({
  selector: "app-product-container",
  standalone: true,
  imports: [
    ProductNavbarComponent
  ],
  templateUrl: "./product-container.component.html"
})
export class ProductContainerComponent  {

}
