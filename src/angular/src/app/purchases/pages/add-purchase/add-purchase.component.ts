import { Component } from '@angular/core';
import {PurchaseFormComponent} from "../../components/purchase-form/purchase-form.component";

@Component({
  selector: "app-add-purchase",
  standalone: true,
  imports: [
    PurchaseFormComponent
  ],
  templateUrl: "./add-purchase.component.html"
})
export class AddPurchaseComponent {

}
