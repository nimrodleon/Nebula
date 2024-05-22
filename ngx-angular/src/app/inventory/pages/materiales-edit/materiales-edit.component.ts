import {Component, OnInit} from '@angular/core';
import {MaterialesFormComponent} from "../../components/materiales-form/materiales-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-materiales-edit",
  standalone: true,
  imports: [
    MaterialesFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./materiales-edit.component.html"
})
export class MaterialesEditComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
