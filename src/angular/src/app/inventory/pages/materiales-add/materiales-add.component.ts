import {Component, OnInit} from '@angular/core';
import {MaterialesFormComponent} from "../../components/materiales-form/materiales-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-materiales-add",
  standalone: true,
  imports: [
    MaterialesFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./materiales-add.component.html"
})
export class MaterialesAddComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
