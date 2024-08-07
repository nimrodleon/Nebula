import {
  faCircleCheck,
  faCircleLeft,
  faEdit,
  faMessage,
  faPlus,
  faRetweet,
  faSave,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import {Component, Injector, Input, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {InventoryProduct, InventoryProductDataModal, Transferencia, TransferenciaDetail} from "../../interfaces";
import {TransferenciaDetailService, TransferenciaService} from "../../services";
import {UserDataService} from "app/common/user-data.service";
import {
  accessDenied,
  confirmTask,
  deleteConfirm,
  initializeSelect2Injector,
  toastError,
  toastSuccess
} from "app/common/interfaces";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";
import _ from "lodash";
import moment from "moment";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {InventoryProductModalComponent} from "../inventory-product-modal/inventory-product-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-transferencias-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    NgForOf,
    FaIconComponent,
    InventoryProductModalComponent,
    NgIf
  ],
  templateUrl: "./transferencias-form.component.html"
})
export class TransferenciasFormComponent implements OnInit {
  @Input()
  type: "ADD" | "EDIT" = "ADD";
  faRetweet = faRetweet;
  faMessage = faMessage;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
  faCircleCheck = faCircleCheck;
  companyId: string = "";
  inventoryProductModal: any;
  transferencia = new Transferencia();
  dataModal = new InventoryProductDataModal();
  warehousesOrigin: Array<Warehouse> = new Array<Warehouse>();
  warehousesTarget: Array<Warehouse> = new Array<Warehouse>();
  transferenciaForm: FormGroup = this.fb.group({
    warehouseOrigin: [null, [Validators.required]],
    warehouseTarget: [null, [Validators.required]],
    remark: [null, [Validators.required]],
  });
  transferenciaDetails = new Array<TransferenciaDetail>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private warehouseService: WarehouseService,
    private transferenciaService: TransferenciaService,
    private transferenciaDetailService: TransferenciaDetailService,
    private userDataService: UserDataService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.inventoryProductModal = new bootstrap.Modal("#inventory-product-modal");
    this.warehouseService.index().subscribe(result => {
      this.warehousesOrigin = result;
      if (this.type === "EDIT") {
        this.transferenciaService.show(id).subscribe(result => {
          this.transferencia = result;
          this.warehousesTarget = _.filter(this.warehousesOrigin, (o: Warehouse) => o.id !== result.warehouseOriginId);
          this.transferenciaForm.reset({
            warehouseOrigin: result.warehouseOriginId,
            warehouseTarget: result.warehouseTargetId,
            remark: result.remark
          });
        });
        this.transferenciaDetailService.index(id)
          .subscribe(result => this.transferenciaDetails = result);
      }
    });
  }

  public inputIsInvalid(field: string) {
    return this.transferenciaForm.controls[field].errors
      && this.transferenciaForm.controls[field].touched;
  }

  public saveChange(): void {
    if (this.transferenciaForm.invalid) {
      this.transferenciaForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const {warehouseOrigin, warehouseTarget, remark} = this.transferenciaForm.value;
    const origen = _.find(this.warehousesOrigin, (o: Warehouse) => o.id === warehouseOrigin);
    if (origen !== undefined) {
      this.transferencia.warehouseOriginId = origen.id;
      this.transferencia.warehouseOriginName = origen.name;
    }
    const destino = _.find(this.warehousesTarget, (o: Warehouse) => o.id === warehouseTarget);
    if (destino !== undefined) {
      this.transferencia.warehouseTargetId = destino.id;
      this.transferencia.warehouseTargetName = destino.name;
    }
    this.transferencia.remark = remark;
    if (this.type === "ADD") {
      this.transferencia.user = "this.authUser.user.userName"; // todo: refactoring.
      this.transferencia.createdAt = moment().format("YYYY-MM-DD");
      this.transferencia.status = "BORRADOR";
      this.transferenciaService.create(this.transferencia)
        .subscribe(result => {
          this.router.navigate([
            "/", this.companyId, "inventories", "transferencias", "edit", result.id
          ]).then(() => {
            toastSuccess("Nueva transferencia entre almacenes registrado!");
          });
        });
    }
    if (this.type === "EDIT") {
      this.transferenciaService.update(this.transferencia.id, this.transferencia)
        .subscribe(result => {
          this.transferencia = result;
          this.transferenciaForm.reset({
            warehouseOrigin: result.warehouseOriginId,
            warehouseTarget: result.warehouseTargetId,
            remark: result.remark
          });
          toastSuccess("Los cambios han sido guardados!");
        });
    }
  }

  public saveChangeDetail(data: InventoryProductDataModal): void {
    if (this.type === "EDIT") {
      const transferenciaDetail = new TransferenciaDetail();
      transferenciaDetail.transferenciaId = this.transferencia.id;
      transferenciaDetail.productId = data.inventoryProduct.productId;
      transferenciaDetail.productName = data.inventoryProduct.productName;
      transferenciaDetail.cantExistente = -1;
      transferenciaDetail.cantTransferido = data.inventoryProduct.cantidad;
      transferenciaDetail.cantRestante = -1;
      if (data.type === "ADD") {
        this.transferenciaDetailService.create(transferenciaDetail)
          .subscribe(result => {
            this.transferenciaDetails = _.concat(this.transferenciaDetails, result);
            this.inventoryProductModal.hide();
            toastSuccess("Nuevo producto agregado a la tabla!");
          });
      }
      if (data.type === "EDIT") {
        transferenciaDetail.id = data.inventoryProduct.id;
        this.transferenciaDetailService.update(transferenciaDetail.id, transferenciaDetail)
          .subscribe(result => {
            this.transferenciaDetails = _.map(this.transferenciaDetails, (o: TransferenciaDetail) => {
              if (o.id === result.id) o = result;
              return o;
            });
            this.inventoryProductModal.hide();
            toastSuccess("El producto de la tabla ha sido actualizado!");
          });
      }
    }
  }

  public deleteDetail(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        if (this.transferencia.status === "BORRADOR") {
          this.transferenciaDetailService.delete(id)
            .subscribe(result => {
              this.transferenciaDetails = _.filter(this.transferenciaDetails,
                (o: TransferenciaDetail) => o.id !== result.id);
            });
        }
        if (this.transferencia.status === "VALIDADO") {
          if (!this.userDataService.canDelete()) {
            accessDenied().then(() => console.log("accessDenied"));
          } else {
            this.transferenciaDetailService.delete(id)
              .subscribe(result => {
                this.transferenciaDetails = _.filter(this.transferenciaDetails,
                  (o: TransferenciaDetail) => o.id !== result.id);
              });
          }
        }
      }
    });
  }

  public changeWarehouseOrigin(target: any): void {
    this.warehousesTarget = _.filter(this.warehousesOrigin, (o: Warehouse) => o.id !== target.value);
    this.transferenciaForm.controls["warehouseTarget"].setValue(null);
  }

  public showInventoryProductModal(): void {
    this.dataModal.type = "ADD";
    this.dataModal.title = "Agregar Producto";
    this.dataModal.inventoryProduct = new InventoryProduct();
    this.inventoryProductModal.show();
  }

  public editInventoryProductModal(data: TransferenciaDetail): void {
    this.dataModal.type = "EDIT";
    this.dataModal.title = "Editar Producto";
    const inventoryProduct = new InventoryProduct();
    inventoryProduct.id = data.id;
    inventoryProduct.productId = data.productId;
    inventoryProduct.productName = data.productName;
    inventoryProduct.cantidad = data.cantTransferido;
    this.dataModal.inventoryProduct = inventoryProduct;
    this.inventoryProductModal.show();
  }

  public back(): void {
    this.router.navigate([
      "/", this.companyId, "inventories", "transferencias"
    ]).then(() => console.log("back"));
  }

  public validar(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.transferenciaService.validate(this.transferencia.id)
          .subscribe(result => {
            this.transferencia = result.transferencia;
            this.transferenciaDetails = result.transferenciaDetails;
            toastSuccess("La validación ha sido exitosa!");
          });
      }
    });
  }

  public validButton(): boolean {
    return this.type === "EDIT" && this.transferencia.status === "BORRADOR";
  }

  public invalidEditItem(): boolean {
    return this.type === "ADD" || this.transferencia.status === "VALIDADO";
  }

}
