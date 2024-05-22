import {Component, OnInit} from "@angular/core";
import {AjustesFormComponent} from "../../components/ajustes-form/ajustes-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-ajustes-edit",
  standalone: true,
  imports: [
    AjustesFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./ajustes-edit.component.html"
})
export class AjustesEditComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
