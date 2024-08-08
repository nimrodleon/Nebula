import {Component, OnInit} from "@angular/core";
import {
  faChartBar,
  faEdit,
  faFilter,
  faPlus,
  faSearch,
  faTrashAlt,
} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {LocationDetailService, LocationService} from "../../services";
import {accessDenied, deleteConfirm, deleteError} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import {Location} from "../../interfaces";
import _ from "lodash";
import Swal from "sweetalert2";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {
  InventoryDetailContainerComponent
} from "../../components/inventory-detail-container/inventory-detail-container.component";
import {NgForOf} from "@angular/common";

@Component({
  selector: "app-location-list",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    InventoryDetailContainerComponent,
    RouterLink,
    NgForOf
  ],
  templateUrl: "./location-list.component.html"
})
export class LocationListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faChartBar = faChartBar;
  companyId: string = "";
  query: FormControl = this.fb.control("");
  locations: Array<Location> = new Array<Location>();
  locationIds: Array<string> = new Array<string>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userDataService: UserDataService,
    private locationService: LocationService,
    private locationDetailService: LocationDetailService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.getLocations();
  }

  public get companyName(): string {
    return this.userDataService.companyName;
  }

  public getLocations(): void {
    this.locationService.index(this.query.value)
      .subscribe(result => this.locations = result);
  }

  public submit(e: Event): void {
    e.preventDefault();
    this.getLocations();
  }

  public deleteLocation(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.locationDetailService.countDocuments(id)
            .subscribe(totalDocuments => {
              if (totalDocuments > 0) {
                deleteError().then(() => console.log(totalDocuments));
              } else {
                this.locationService.delete(id).subscribe(result => {
                  this.locations = _.filter(this.locations, (o: Location) => o.id !== result.id);
                });
              }
            });
        }
      });
    }
  }

  public changeCheckbox({value, checked}: any): void {
    if (checked) {
      this.locationIds = _.concat(this.locationIds, value);
    } else {
      this.locationIds = _.filter(this.locationIds, index => index !== value);
    }
  }

  public reponerProductos(): void {
    if (this.locationIds.length > 0) {
      this.router.navigate([
        `/${this.companyId}/inventories/location/reponer`, String(this.locationIds)
      ]).then(() => console.log(":)"));
    } else {
      Swal.fire(
        "Error",
        "Seleccione una Ubicación!",
        "error",
      );
    }
  }

}
