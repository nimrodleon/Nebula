import {Component, OnInit} from '@angular/core';
import {TransferenciasFormComponent} from "../../components/transferencias-form/transferencias-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-transferencias-add",
  standalone: true,
  imports: [
    TransferenciasFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./transferencias-add.component.html"
})
export class TransferenciasAddComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
