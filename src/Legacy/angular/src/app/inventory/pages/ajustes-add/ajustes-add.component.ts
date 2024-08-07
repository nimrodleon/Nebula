import {Component, OnInit} from "@angular/core";
import {AjustesFormComponent} from "../../components/ajustes-form/ajustes-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-ajustes-add",
  standalone: true,
  imports: [
    AjustesFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./ajustes-add.component.html"
})
export class AjustesAddComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
