import { Component } from '@angular/core';
import {PurchaseFormComponent} from "../../components/purchase-form/purchase-form.component";

@Component({
  selector: "app-edit-purchase",
  standalone: true,
  imports: [
    PurchaseFormComponent
  ],
  templateUrl: "./edit-purchase.component.html"
})
export class EditPurchaseComponent {

}
