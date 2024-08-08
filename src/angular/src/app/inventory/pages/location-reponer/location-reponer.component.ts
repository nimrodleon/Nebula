import {Component, OnInit} from "@angular/core";
import {LocationItemStockDto, RespLocationDetailStock} from "../../interfaces";
import {ActivatedRoute} from "@angular/router";
import {LocationService} from "../../services";
import _ from "lodash";
import {NgForOf} from "@angular/common";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";

@Component({
  selector: "app-location-reponer",
  standalone: true,
  imports: [
    NgForOf,
    InventoryContainerComponent
  ],
  templateUrl: "./location-reponer.component.html"
})
export class LocationReponerComponent implements OnInit {
  locationDetailStocks = new Array<LocationItemStockDto>();

  constructor(
    private activatedRoute: ActivatedRoute,
    private locationService: LocationService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const arrId: string = params.get("arrId") || "";
      this.locationService.reponerStocks(arrId)
        .subscribe(result => {
          _.forEach(result, (respLocationDetailStock: RespLocationDetailStock) => {
            const {location} = respLocationDetailStock;
            _.forEach(respLocationDetailStock.locationDetailStocks, (locationItemStock: LocationItemStockDto) => {
              const itemStock = locationItemStock;
              itemStock.description = `${location?.warehouseName} - ${location?.description} | ${locationItemStock.description}`;
              this.locationDetailStocks = _.concat(this.locationDetailStocks, itemStock);
            });
          });
        });
    });
  }
}
