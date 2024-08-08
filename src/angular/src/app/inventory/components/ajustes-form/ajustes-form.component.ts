import {
  faBox, faCircleCheck,
  faCircleLeft,
  faEdit,
  faMessage,
  faPlus,
  faSave,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, Router} from "@angular/router";
import {Component, Input, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import _ from "lodash";
import moment from "moment";
import Swal from "sweetalert2";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {AjusteInventarioDetailService, AjusteInventarioService, LocationService} from "../../services";
import {AjusteInventario, AjusteInventarioDetail, Location} from "../../interfaces";
import {accessDenied, confirmTask, deleteConfirm, toastError, toastSuccess} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";

@Component({
  selector: "app-ajustes-form",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgClass,
    NgForOf,
    NgIf
  ],
  templateUrl: "./ajustes-form.component.html"
})
export class AjustesFormComponent implements OnInit {
  @Input()
  type: "ADD" | "EDIT" = "ADD";
  faBox = faBox;
  faMessage = faMessage;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  faCircleLeft = faCircleLeft;
  faCircleCheck = faCircleCheck;
  faSave = faSave;
  companyId: string = "";
  ajusteInventario: AjusteInventario = new AjusteInventario();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  locations: Array<Location> = new Array<Location>();
  ajusteForm: FormGroup = this.fb.group({
    warehouseId: [null, [Validators.required]],
    locationId: [null, [Validators.required]],
    remark: [null, [Validators.required]],
  });
  ajusteInventarioDetails = new Array<AjusteInventarioDetail>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private warehouseService: WarehouseService,
    private locationService: LocationService,
    private ajusteInventarioService: AjusteInventarioService,
    private ajusteInventarioDetailService: AjusteInventarioDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.warehouseService.index()
      .subscribe(result => this.warehouses = result);
    if (this.type === "EDIT") {
      const id: string = this.activatedRoute.snapshot.params["id"];
      this.ajusteInventarioService.show(id).subscribe(result => {
        this.ajusteInventario = result;
        this.locationService.getByWarehouse(result.warehouseId)
          .subscribe(result => {
            this.locations = result;
            this.ajusteForm.reset({
              warehouseId: this.ajusteInventario.warehouseId,
              locationId: this.ajusteInventario.locationId,
              remark: this.ajusteInventario.remark,
            });
          });
      });
      this.ajusteInventarioDetailService.index(id)
        .subscribe(result => this.ajusteInventarioDetails = result);
    }
  }

  public inputIsInvalid(field: string) {
    return this.ajusteForm.controls[field].errors
      && this.ajusteForm.controls[field].touched;
  }

  public saveChange(): void {
    if (this.ajusteForm.invalid) {
      this.ajusteForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.ajusteInventario = {...this.ajusteInventario, ...this.ajusteForm.value};
    const warehouse = _.find(this.warehouses, (o: Warehouse) => o.id === this.ajusteInventario.warehouseId);
    if (warehouse !== undefined) this.ajusteInventario.warehouseName = warehouse.name;
    const location = _.find(this.locations, (o: Location) => o.id === this.ajusteInventario.locationId);
    if (location !== undefined) this.ajusteInventario.locationName = location.description;
    if (this.type === "ADD") {
      this.ajusteInventario.user = "this.authUser.user.userName;"; // todo: refactoring.
      this.ajusteInventario.createdAt = moment().format("YYYY-MM-DD");
      this.ajusteInventario.status = "BORRADOR";
      this.ajusteInventarioService.create(this.ajusteInventario)
        .subscribe(result => {
          this.router.navigate([
            "/", this.companyId, "inventories", "ajustes", "edit", result.id
          ]).then(() => {
            toastSuccess("Nuevo ajuste de inventario registrado!");
          });
        });
    }
    if (this.type === "EDIT") {
      this.ajusteInventarioService.update(this.ajusteInventario.id, this.ajusteInventario)
        .subscribe(result => {
          this.ajusteInventario = result;
          this.ajusteForm.reset({
            warehouseId: result.warehouseId,
            locationId: result.locationId,
            remark: result.remark,
          });
          toastSuccess("Los cambios han sido guardados!");
        });
    }
  }

  public deleteDetail(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        if (this.ajusteInventario.status === "BORRADOR") {
          this.ajusteInventarioDetailService.delete(id)
            .subscribe(result => {
              this.ajusteInventarioDetails = _.filter(this.ajusteInventarioDetails,
                (o: AjusteInventarioDetail) => o.id !== result.id);
            });
        }
        if (this.ajusteInventario.status === "VALIDADO") {
          if (!this.userDataService.canDelete()) {
            accessDenied().then(() => console.log("accessDenied"));
          } else {
            this.ajusteInventarioDetailService.delete(id)
              .subscribe(result => {
                this.ajusteInventarioDetails = _.filter(this.ajusteInventarioDetails,
                  (o: AjusteInventarioDetail) => o.id !== result.id);
              });
          }
        }
      }
    });
  }

  public editAjusteInventarioDetail(data: AjusteInventarioDetail): void {
    Swal.fire({
      title: "CAMBIAR CANTIDAD",
      input: "number",
      inputLabel: "INGRESE CANTIDAD DEL ARTÍCULO",
      inputAttributes: {"step": "1"},
      inputValue: 1,
      showCancelButton: true,
      confirmButtonText: "Sí, Aceptar",
      cancelButtonText: "Cancelar",
    }).then(result => {
      if (result.isConfirmed) {
        data.cantContada = Number(result.value);
        this.ajusteInventarioDetailService.update(data.id, data)
          .subscribe(result => {
            this.ajusteInventarioDetails = _.map(this.ajusteInventarioDetails, (o: AjusteInventarioDetail) => {
              if (o.id === result.id) o = result;
              return o;
            });
            toastSuccess("La cantidad del producto ha sido actualizado!");
          });
      }
    });
  }

  public changeWarehouses(target: any): void {
    this.locationService.getByWarehouse(target.value)
      .subscribe(result => this.locations = result);
  }

  public back(): void {
    this.router.navigate([
      "/", this.companyId, "inventories", "ajustes"
    ]).then(() => console.log("back"));
  }

  public validar(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.ajusteInventarioService.validate(this.ajusteInventario.id)
          .subscribe(result => {
            this.ajusteInventario = result.ajusteInventario;
            this.ajusteInventarioDetails = result.ajusteInventarioDetails;
            toastSuccess("La validación ha sido exitosa!");
          });
      }
    });
  }

  public validButton(): boolean {
    return this.type === "EDIT" && this.ajusteInventario.status === "BORRADOR";
  }

  public invalidEditItem(): boolean {
    return this.type === "ADD" || this.ajusteInventario.status === "VALIDADO";
  }

}
