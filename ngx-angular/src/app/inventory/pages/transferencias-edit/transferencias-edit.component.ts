import {Component, OnInit} from "@angular/core";
import {TransferenciasFormComponent} from "../../components/transferencias-form/transferencias-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-transferencias-edit",
  standalone: true,
  imports: [
    TransferenciasFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./transferencias-edit.component.html"
})
export class TransferenciasEditComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
