import {Component} from "@angular/core";
import {NotasFormComponent} from "../../components/notas-form/notas-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-notas-edit",
  standalone: true,
  imports: [
    NotasFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./notas-edit.component.html"
})
export class NotasEditComponent {

}
