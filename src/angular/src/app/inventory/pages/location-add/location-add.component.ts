import {Component, OnInit} from '@angular/core';
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";
import {LocationFormComponent} from "../../components/location-form/location-form.component";

@Component({
  selector: "app-location-add",
  standalone: true,
  imports: [
    InventoryContainerComponent,
    LocationFormComponent
  ],
  templateUrl: "./location-add.component.html"
})
export class LocationAddComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }

}
