import {Component} from "@angular/core";
import {NotasFormComponent} from "../../components/notas-form/notas-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-notas-add",
  standalone: true,
  imports: [
    NotasFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./notas-add.component.html"
})
export class NotasAddComponent {

}
