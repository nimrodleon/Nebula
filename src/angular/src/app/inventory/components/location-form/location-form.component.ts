import {Component, Injector, Input, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {faCircleLeft, faEdit, faPlus, faSave, faTrashAlt, faTruckRampBox} from "@fortawesome/free-solid-svg-icons";
import {Location, LocationDetail, ProductLocationDataModal} from "../../interfaces";
import {LocationDetailService, LocationService} from "../../services";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";
import {deleteConfirm, initializeSelect2Injector, toastError, toastSuccess} from "app/common/interfaces";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf} from "@angular/common";
import {ProductLocationComponent} from "../product-location/product-location.component";

declare const bootstrap: any;

@Component({
  selector: "app-location-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
    NgForOf,
    ProductLocationComponent
  ],
  templateUrl: "./location-form.component.html"
})
export class LocationFormComponent implements OnInit {
  @Input()
  type: "ADD" | "EDIT" = "ADD";
  faTruckRampBox = faTruckRampBox;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
  faPlus = faPlus;
  companyId: string = "";
  productLocationModal: any;
  location: Location = new Location();
  dataModal = new ProductLocationDataModal();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  locationForm: FormGroup = this.fb.group({
    warehouseId: [null, [Validators.required]],
    description: [null, [Validators.required]],
  });
  locationDetails: Array<LocationDetail> = new Array<LocationDetail>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private locationService: LocationService,
    private warehouseService: WarehouseService,
    private locationDetailService: LocationDetailService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.productLocationModal = new bootstrap.Modal("#product-location");
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    if (this.type === "EDIT") {
      const id: string = this.activatedRoute.snapshot.params["id"];
      this.locationService.show(id).subscribe(result => {
        this.location = result;
        this.locationForm.reset({...result});
      });
      this.locationDetailService.index(id)
        .subscribe(result => this.locationDetails = result);
    }
  }

  public inputIsInvalid(field: string) {
    return this.locationForm.controls[field].errors
      && this.locationForm.controls[field].touched;
  }

  public saveChange(): void {
    const warehouse = _.find(this.warehouses, (o: Warehouse) =>
      o.id === this.locationForm.get("warehouseId")?.value);
    if (warehouse !== undefined) this.location.warehouseName = warehouse.name;
    this.location = {...this.location, ...this.locationForm.value};
    if (this.locationForm.invalid) {
      this.locationForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    if (this.type === "ADD") {
      this.locationService.create(this.location)
        .subscribe(result => {
          this.router.navigate([
            "/", this.companyId, "inventories", "location", "edit", result.id
          ]).then(() => {
            toastSuccess("La ubicación ha sido registrado!");
          });
        });
    }
    if (this.type === "EDIT") {
      this.locationService.update(this.location.id, this.location)
        .subscribe(result => {
          this.location = result;
          this.locationForm.reset({...result});
          toastSuccess("La ubicación ha sido actualizado!");
        });
    }
  }

  public saveChangeDetail(data: ProductLocationDataModal): void {
    if (this.type === "EDIT") {
      data.locationDetail.locationId = this.location.id;
      if (data.type === "ADD") {
        this.locationDetailService.create(data.locationDetail)
          .subscribe(result => {
            this.locationDetails = _.concat(this.locationDetails, result);
            this.productLocationModal.hide();
            toastSuccess("Nuevo producto agregado a la tabla!");
          });
      }
      if (data.type === "EDIT") {
        this.locationDetailService.update(data.locationDetail.id, data.locationDetail)
          .subscribe(result => {
            this.locationDetails = _.map(this.locationDetails, (o: LocationDetail) => {
              if (o.id === result.id) o = result;
              return o;
            });
            this.productLocationModal.hide();
            toastSuccess("El producto de la tabla ha sido actualizado!");
          });
      }
    }
  }

  public deleteDetail(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.locationDetailService.delete(id)
          .subscribe(result => {
            this.locationDetails = _.filter(this.locationDetails,
              (o: LocationDetail) => o.id !== result.id);
          });
      }
    });
  }

  public showProductLocationModal(): void {
    this.dataModal.type = "ADD";
    this.dataModal.title = "Agregar Producto";
    this.dataModal.locationDetail = new LocationDetail();
    this.productLocationModal.show();
  }

  public editProductLocationModal(data: LocationDetail): void {
    this.dataModal.type = "EDIT";
    this.dataModal.title = "Editar Producto";
    this.dataModal.locationDetail = data;
    this.productLocationModal.show();
  }

  public back(): void {
    this.router.navigate([
      "/", this.companyId, "inventories", "location"
    ]).then(() => console.log("back"));
  }

}
