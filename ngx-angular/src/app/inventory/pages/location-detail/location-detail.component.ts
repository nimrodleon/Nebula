import {Component, OnInit} from "@angular/core";
import {faGear, faPrint} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute} from "@angular/router";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {LocationItemStockDto} from "../../interfaces";
import {LocationService} from "../../services";
import {range} from "app/common/interfaces";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";
import {NgClass, NgForOf} from "@angular/common";

@Component({
  selector: "app-location-detail",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    InventoryContainerComponent,
    NgForOf,
    NgClass
  ],
  templateUrl: "./location-detail.component.html"
})
export class LocationDetailComponent implements OnInit {
  faGear = faGear;
  faPrint = faPrint;
  locationDetailStocks = new Array<LocationItemStockDto>();
  numRow: Array<number> = [1];
  formControl: FormControl = this.fb.control(1);

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private locationService: LocationService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.locationService.detailStocks(id)
      .subscribe(result => this.locationDetailStocks = result);
  }

  public print(): void {
    window.print();
  }

  public generar(): void {
    this.numRow = range(1, this.formControl.value, 1);
  }

}
