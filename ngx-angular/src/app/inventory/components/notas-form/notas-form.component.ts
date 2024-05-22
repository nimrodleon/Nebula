import {
  faCircleCheck,
  faCircleLeft,
  faEdit,
  faMessage,
  faNoteSticky,
  faPlus,
  faSave,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import {Component, Injector, Input, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {
  confirmTask,
  deleteConfirm,
  select2Contactos,
  initializeSelect2Injector,
  accessDenied, FormType, toastError, toastSuccess
} from "app/common/interfaces";
import {InventoryNotas, InventoryNotasDetail, InventoryProduct, InventoryProductDataModal} from "../../interfaces";
import {InventoryNotasDetailService, InventoryNotasService} from "../../services";
import {UserDataService} from "app/common/user-data.service";
import {WarehouseService} from "app/account/company/services";
import {Warehouse} from "app/account/company/interfaces";
import Swal from "sweetalert2";
import moment from "moment";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {InventoryProductModalComponent} from "../inventory-product-modal/inventory-product-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-notas-form",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgIf,
    NgClass,
    NgForOf,
    InventoryProductModalComponent
  ],
  templateUrl: "./notas-form.component.html"
})
export class NotasFormComponent implements OnInit {
  @Input()
  type: "ADD" | "EDIT" = "ADD";
  faNoteSticky = faNoteSticky;
  faMessage = faMessage;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
  faCircleCheck = faCircleCheck;
  companyId: string = "";
  inventoryProductModal: any;
  inventoryNotas: InventoryNotas = new InventoryNotas();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  dataModal: InventoryProductDataModal = new InventoryProductDataModal();
  notaForm: FormGroup = this.fb.group({
    warehouseId: [null, [Validators.required]],
    contactId: [null, [Validators.required]],
    remark: [null, [Validators.required]]
  });
  inventoryNotasDetails: Array<InventoryNotasDetail> = new Array<InventoryNotasDetail>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private warehouseService: WarehouseService,
    private inventoryNotasService: InventoryNotasService,
    private inventoryNotasDetailService: InventoryNotasDetailService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.inventoryProductModal = new bootstrap.Modal("#inventory-product-modal");
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    const contacto = select2Contactos("#contactId")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.notaForm.controls["contactId"].setValue(data.id);
        this.inventoryNotas.contactName = data.name;
      });
    if (this.type === "EDIT") {
      this.activatedRoute.paramMap.subscribe(params => {
        const id: string = params.get("id") || "";
        this.inventoryNotasService.show(id)
          .subscribe(result => {
            this.inventoryNotas = result;
            this.notaForm.reset({
              warehouseId: result.warehouseId,
              contactId: result.contactId,
              remark: result.remark
            });
            // reset input select2.
            const {contactId, contactName} = result;
            const contactOption = new Option(contactName, contactId, true, true);
            contacto.append(contactOption).trigger("change");
          });
        this.inventoryNotasDetailService.index(id)
          .subscribe(result => this.inventoryNotasDetails = result);
      });
    }
  }

  public get authUser() {
    return this.userDataService.userAuth;
  }

  public inputIsInvalid(field: string) {
    return this.notaForm.controls[field].errors
      && this.notaForm.controls[field].touched;
  }

  public saveChange(): void {
    this.inventoryNotas = {...this.inventoryNotas, ...this.notaForm.value};
    const warehouse = _.find(this.warehouses, (o: Warehouse) => o.id === this.inventoryNotas.warehouseId);
    if (warehouse !== undefined) this.inventoryNotas.warehouseName = warehouse.name;
    if (this.notaForm.invalid) {
      this.notaForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    if (this.type === "ADD") {
      this.inventoryNotas.user = this.authUser.userName;
      this.inventoryNotas.createdAt = moment().format("YYYY-MM-DD");
      this.inventoryNotas.status = "BORRADOR";
      // configurar tipo de inventario. ENTRADA/SALIDA.
      Swal.fire({
        title: "TIPO DE INVENTARIO",
        input: "radio",
        inputOptions: {
          "ENTRADA": "ENTRADA",
          "SALIDA": "SALIDA"
        },
        inputValidator(inputValue: string): any {
          if (!inputValue) return "¡Necesitas elegir Algo!";
        }
      }).then(result => {
        if (result.isConfirmed) {
          this.inventoryNotas.transactionType = result.value;
          this.inventoryNotasService.create(this.inventoryNotas)
            .subscribe(result => {
              this.router.navigate([
                "/", this.companyId, "inventories", "notas", "edit", result.id
              ]).then(() => {
                toastSuccess(`La nota de ${this.inventoryNotas.transactionType} ha sido registrado!`);
              });
            });
        }
      });
    }
    if (this.type === "EDIT") {
      this.inventoryNotasService.update(this.inventoryNotas.id, this.inventoryNotas)
        .subscribe(result => {
          this.inventoryNotas = result;
          this.notaForm.reset({
            warehouseId: result.warehouseId,
            contactId: result.contactId,
            remark: result.remark,
          });
          toastSuccess("Los cambios han sido guardados!");
        });
    }
  }

  public saveChangeDetail(data: InventoryProductDataModal): void {
    if (this.type === "EDIT") {
      const inventoryNotasDetail: InventoryNotasDetail = new InventoryNotasDetail();
      inventoryNotasDetail.inventoryNotasId = this.inventoryNotas.id;
      inventoryNotasDetail.productId = data.inventoryProduct.productId;
      inventoryNotasDetail.productName = data.inventoryProduct.productName;
      inventoryNotasDetail.demanda = data.inventoryProduct.cantidad;
      if (data.type === "ADD") {
        this.inventoryNotasDetailService.create(inventoryNotasDetail)
          .subscribe(result => {
            this.inventoryNotasDetails = _.concat(this.inventoryNotasDetails, result);
            this.inventoryProductModal.hide();
            toastSuccess("Nuevo producto agregado a la tabla!");
          });
      }
      if (data.type === "EDIT") {
        inventoryNotasDetail.id = data.inventoryProduct.id;
        this.inventoryNotasDetailService.update(inventoryNotasDetail.id, inventoryNotasDetail)
          .subscribe(result => {
            this.inventoryNotasDetails = _.map(this.inventoryNotasDetails, (o: InventoryNotasDetail) => {
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
        if (this.inventoryNotas.status === "BORRADOR") {
          this.inventoryNotasDetailService.delete(id)
            .subscribe(result => {
              this.inventoryNotasDetails = _.filter(this.inventoryNotasDetails,
                (o: InventoryNotasDetail) => o.id !== result.id);
            });
        }
        if (this.inventoryNotas.status === "VALIDADO") {
          if (!this.userDataService.canDelete()) {
            accessDenied().then(() => console.log("accessDenied"));
          } else {
            this.inventoryNotasDetailService.delete(id)
              .subscribe(result => {
                this.inventoryNotasDetails = _.filter(this.inventoryNotasDetails,
                  (o: InventoryNotasDetail) => o.id !== result.id);
              });
          }
        }
      }
    });
  }

  public showInventoryProductModal(): void {
    this.dataModal.type = FormType.ADD;
    this.dataModal.title = "Agregar Producto";
    this.dataModal.inventoryProduct = new InventoryProduct();
    this.inventoryProductModal.show();
  }

  public editInventoryProductModal(data: InventoryNotasDetail): void {
    this.dataModal.type = FormType.EDIT;
    this.dataModal.title = "Editar Producto";
    const inventoryProduct = new InventoryProduct();
    inventoryProduct.id = data.id;
    inventoryProduct.productId = data.productId;
    inventoryProduct.productName = data.productName;
    inventoryProduct.cantidad = data.demanda;
    this.dataModal.inventoryProduct = inventoryProduct;
    this.inventoryProductModal.show();
  }

  public validar(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.inventoryNotasService.validate(this.inventoryNotas.id)
          .subscribe(result => {
            this.inventoryNotas = result.inventoryNotas;
            this.inventoryNotasDetails = result.inventoryNotasDetail;
            toastSuccess("La validación ha sido exitosa!");
          });
      }
    });
  }

  public back(): void {
    this.router.navigate([
      "/", this.companyId, "inventories", "notas"
    ]).then(() => console.log("back"));
  }

  public validButton(): boolean {
    return this.type === "EDIT" && this.inventoryNotas.status === "BORRADOR";
  }

  public invalidEditItem(): boolean {
    return this.type === "ADD" || this.inventoryNotas.status === "VALIDADO";
  }

}
