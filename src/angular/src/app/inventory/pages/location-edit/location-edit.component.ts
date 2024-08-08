import {Component, OnInit} from "@angular/core";
import {LocationFormComponent} from "../../components/location-form/location-form.component";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-location-edit",
  standalone: true,
  imports: [
    LocationFormComponent,
    InventoryContainerComponent
  ],
  templateUrl: "./location-edit.component.html"
})
export class LocationEditComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
