import {Component, Injector, Input, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {
  deleteConfirm,
  FormType,
  initializeSelect2Injector,
  select2Contactos, toastSuccess
} from "app/common/interfaces";
import {
  faBox, faBoxesAlt, faCircleLeft, faCoins, faEdit, faFilter,
  faIdCardAlt, faLaptop, faPlus, faPrint,
  faSave, faScrewdriverWrench, faSearch, faTrashAlt, faUserAlt
} from "@fortawesome/free-solid-svg-icons";
import {Contact, ContactDataModal} from "app/contact/interfaces";
import {ContactService} from "app/contact/services";
import {ItemRepairOrder, ItemRepairOrderDataModal, RepairOrder} from "../../interfaces";
import {ItemRepairOrderService, RepairOrderService} from "../../services";
import {InvoiceSerie, Warehouse} from "app/account/company/interfaces";
import {InvoiceSerieService, WarehouseService} from "app/account/company/services";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {MaterialRepairOrderModalComponent} from "../material-repair-order-modal/material-repair-order-modal.component";
import {SearchContactModalComponent} from "app/common/contact/search-contact-modal/search-contact-modal.component";
import {ContactModalComponent} from "app/common/contact/contact-modal/contact-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-orden-reparacion-form",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgClass,
    CurrencyPipe,
    NgForOf,
    RouterLink,
    MaterialRepairOrderModalComponent,
    SearchContactModalComponent,
    ContactModalComponent,
    NgIf
  ],
  templateUrl: "./orden-reparacion-form.component.html"
})
export class OrdenReparacionFormComponent implements OnInit {
  @Input() type: string = FormType.ADD;
  faSearch = faSearch;
  faIdCardAlt = faIdCardAlt;
  faPlus = faPlus;
  faUserAlt = faUserAlt;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
  faPrint = faPrint;
  faCoins = faCoins;
  faBoxesAlt = faBoxesAlt;
  faScrewdriverWrench = faScrewdriverWrench;
  faLaptop = faLaptop;
  faFilter = faFilter;
  faBox = faBox;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  EnumTypeForm = FormType;
  // ====================================================================================================
  companyId: string = "";
  contactModal: any;
  contactDataModal: ContactDataModal = new ContactDataModal();
  searchContactModal: any;
  materialRepairOrderModal: any;
  itemRepairOrderDataModal: ItemRepairOrderDataModal = new ItemRepairOrderDataModal();
  // ====================================================================================================
  repairOrderId: string = "";
  almacenes: Warehouse[] = new Array<Warehouse>();
  invoiceSeries: InvoiceSerie[] = new Array<InvoiceSerie>();
  itemsRepairOrder: ItemRepairOrder[] = new Array<ItemRepairOrder>();
  repairOrderForm: FormGroup = this.fb.group({
    idCliente: ["", [Validators.required]],
    nombreCliente: ["", [Validators.required]],
    datosEquipo: ["", [Validators.required]],
    tareaRealizar: ["", [Validators.required]],
    warehouseId: ["", [Validators.required]],
    technicalId: ["", [Validators.required]],
    technicalName: ["", [Validators.required]],
    status: ["", [Validators.required]],
    invoiceSerieId: ["", [Validators.required]],
    repairAmount: [null, [Validators.required]],
  });
  montoTotalMateriales: number = 0;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private contactService: ContactService,
    private warehouseService: WarehouseService,
    private invoiceSerieService: InvoiceSerieService,
    private repairOrderService: RepairOrderService,
    private itemRepairOrderService: ItemRepairOrderService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    // cargar modal contacto y buscar contacto.
    this.contactModal = new bootstrap.Modal("#contact-modal");
    this.searchContactModal = new bootstrap.Modal("#searchContactModal");
    // cargar modal de materiales orden de reparación.
    this.materialRepairOrderModal = new bootstrap.Modal("#materialRepairOrderModal");
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.almacenes = result);
    // cargar lista de contactos para seleccionar el técnico asignado.
    const userTech = select2Contactos("#userTech")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.repairOrderForm.controls["technicalId"].setValue(data.id);
        this.repairOrderForm.controls["technicalName"].setValue(data.name);
      });
    // cargar lista de series de facturación.
    this.invoiceSerieService.index()
      .subscribe(result => this.invoiceSeries = result);
    // cargar datos por defecto si está en modo edición.
    if (this.type === FormType.EDIT) {
      this.activatedRoute.paramMap.subscribe(params => {
        this.repairOrderId = params.get("id") || "";
        this.repairOrderService.show(this.repairOrderId)
          .subscribe(result => {
            this.repairOrderForm.controls["idCliente"].setValue(result.idCliente);
            this.repairOrderForm.controls["nombreCliente"].setValue(result.nombreCliente);
            this.repairOrderForm.controls["datosEquipo"].setValue(result.datosEquipo);
            this.repairOrderForm.controls["tareaRealizar"].setValue(result.tareaRealizar);
            this.repairOrderForm.controls["warehouseId"].setValue(result.warehouseId);
            this.repairOrderForm.controls["status"].setValue(result.status);
            this.repairOrderForm.controls["invoiceSerieId"].setValue(result.invoiceSerieId);
            this.repairOrderForm.controls["repairAmount"].setValue(result.repairAmount);
            // cargar datos del técnico.
            this.contactService.show(result.technicalId)
              .subscribe(result => {
                this.repairOrderForm.controls["technicalId"].setValue(result.id);
                this.repairOrderForm.controls["technicalName"].setValue(result.name);
                const newOption = new Option(result.name, result.id, true, true);
                userTech.append(newOption).trigger("change");
              });
          });
        // cargar materiales usados en la reparación.
        this.itemRepairOrderService.index(this.repairOrderId)
          .subscribe(result => {
            this.itemsRepairOrder = result;
            this.calcularMontoMateriales();
          });
      });
    }
  }

  public abrirModalContacto(): void {
    this.contactDataModal = new ContactDataModal();
    this.contactDataModal.title = "Agregar Contacto";
    this.contactDataModal.type = FormType.ADD;
    this.contactModal.show();
  }

  public abrirModalBuscarContacto(): void {
    this.searchContactModal.show();
  }

  public abrirModalMaterial(): void {
    this.itemRepairOrderDataModal.type = FormType.ADD;
    this.itemRepairOrderDataModal.title = "Agregar Material";
    this.itemRepairOrderDataModal.itemRepairOrder = new ItemRepairOrder();
    this.materialRepairOrderModal.show();
  }

  public editarModalMaterial(item: ItemRepairOrder): void {
    this.itemRepairOrderDataModal.type = FormType.EDIT;
    this.itemRepairOrderDataModal.title = "Editar Material";
    this.itemRepairOrderDataModal.itemRepairOrder = item;
    this.materialRepairOrderModal.show();
  }

  private calcularMontoMateriales(): void {
    this.montoTotalMateriales = _.sumBy(this.itemsRepairOrder, item => item.monto);
  }

  public borrarModalMaterial(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.itemRepairOrderService.delete(id)
          .subscribe(result => {
            this.itemsRepairOrder = _.filter(this.itemsRepairOrder, item => item.id !== result.id);
          });
      }
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.repairOrderForm.controls[field].errors && this.repairOrderForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.repairOrderForm.invalid) {
      this.repairOrderForm.markAllAsTouched();
      toastSuccess("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const repairOrder: RepairOrder = new RepairOrder();
    repairOrder.idCliente = this.repairOrderForm.get("idCliente")?.value;
    repairOrder.nombreCliente = this.repairOrderForm.get("nombreCliente")?.value;
    repairOrder.datosEquipo = this.repairOrderForm.get("datosEquipo")?.value;
    repairOrder.tareaRealizar = this.repairOrderForm.get("tareaRealizar")?.value;
    repairOrder.warehouseId = this.repairOrderForm.get("warehouseId")?.value;
    const warehouseFilter = _.find(this.almacenes, item => item.id === repairOrder.warehouseId);
    if (warehouseFilter !== undefined) repairOrder.warehouseName = warehouseFilter.name;
    repairOrder.technicalId = this.repairOrderForm.get("technicalId")?.value;
    repairOrder.technicalName = this.repairOrderForm.get("technicalName")?.value;
    repairOrder.status = this.repairOrderForm.get("status")?.value;
    repairOrder.invoiceSerieId = this.repairOrderForm.get("invoiceSerieId")?.value;
    repairOrder.repairAmount = this.repairOrderForm.get("repairAmount")?.value;
    if (this.type === FormType.ADD) {
      this.repairOrderService.create(repairOrder)
        .subscribe(result => {
          toastSuccess(`Nueva orden de reparación ${result.serie}-${result.number} registrada!`);
          this.router.navigate([
            "/", this.companyId, "taller-reparaciones", "orden-reparacion", "edit", result.id
          ]).then(() => console.log(result.id));
        });
    }
    if (this.type === FormType.EDIT) {
      repairOrder.id = this.repairOrderId;
      this.repairOrderService.update(this.repairOrderId, repairOrder)
        .subscribe(result => {
          toastSuccess(`La orden de reparación ${result.serie}-${result.number}, ha sido actualizado!`);
        });
    }
  }

  /**
   * Guardar datos de materiales usados en la reparación.
   * @param formData item material
   */
  public saveChangesDetail(formData: ItemRepairOrderDataModal): void {
    formData.itemRepairOrder.repairOrderId = this.repairOrderId;
    if (formData.type === FormType.ADD) {
      this.itemRepairOrderService.create(formData.itemRepairOrder)
        .subscribe(result => {
          toastSuccess("Nuevo material agregado a la tabla!");
          this.itemsRepairOrder = _.concat(this.itemsRepairOrder, result);
          this.materialRepairOrderModal.hide();
          this.calcularMontoMateriales();
        });
    }
    if (formData.type === FormType.EDIT) {
      this.itemRepairOrderService.update(formData.itemRepairOrder.id, formData.itemRepairOrder)
        .subscribe(result => {
          toastSuccess("El material de la tabla ha sido actualizado!");
          this.itemsRepairOrder = _.map(this.itemsRepairOrder, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          this.materialRepairOrderModal.hide();
          this.calcularMontoMateriales();
        });
    }
  }

  /**
   * Seleccionar cliente.
   * @param contacto datos del cliente.
   */
  public seleccionarContacto(contacto: Contact): void {
    this.repairOrderForm.controls["idCliente"].setValue(contacto.id);
    this.repairOrderForm.controls["nombreCliente"].setValue(contacto.name);
    this.searchContactModal.hide();
  }

  /**
   * Guardar datos de contacto.
   * @param dataModal información devuelta por el modal
   */
  public guardarContacto(dataModal: ContactDataModal): void {
    if (dataModal.type === FormType.ADD) {
      this.contactService.create(dataModal.contact)
        .subscribe(result => {
          this.repairOrderForm.controls["idCliente"].setValue(result.id);
          this.repairOrderForm.controls["nombreCliente"].setValue(result.name);
          this.contactModal.hide();
        });
    }
  }

  public cancelar(): void {
    this.router.navigate(["/", this.companyId, "taller-reparaciones"]).then(() => console.log("redirect"));
  }

}
