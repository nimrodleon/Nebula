import { Component, Injector, OnInit } from "@angular/core";
import {
  faChevronCircleRight,
  faCircleLeft,
  faEdit,
  faFolderTree,
  faIdCardAlt,
  faMessage,
  faNoteSticky,
  faPlus,
  faSave,
  faSearch,
  faTrashAlt,
  faUserAlt
} from "@fortawesome/free-solid-svg-icons";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { CreditInformationDto, FormaPagoComprobante, ItemComprobanteModal } from "../../interfaces";
import { Contact, ContactDataModal } from "app/contact/interfaces";
import { InvoiceSaleService } from "../../services";
import { ContactService } from "app/contact/services";
import { UserDataService } from "app/common/user-data.service";
import {
  EnumIdModal,
  FormType,
  deleteConfirm,
  confirmTask,
  initializeSelect2Injector,
  isValidObjectId, toastError
} from "app/common/interfaces";
import { ComprobanteDto, Cuota, ItemComprobanteDto } from "app/cashier/quicksale/interfaces";
import { InvoiceSerie } from "app/account/company/interfaces";
import { InvoiceSerieService } from "app/account/company/services";
import { Company } from "app/account/interfaces";
import Swal from "sweetalert2";
import _ from "lodash";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { CurrencyPipe, NgClass, NgForOf, NgIf, NgStyle } from "@angular/common";
import { SalesContainerComponent } from "app/common/containers/sales-container/sales-container.component";
import { SplitPipe } from "app/common/pipes/split.pipe";
import { SearchContactModalComponent } from "app/common/contact/search-contact-modal/search-contact-modal.component";
import { ContactModalComponent } from "app/common/contact/contact-modal/contact-modal.component";
import { AddEntradaModalComponent } from "../../components/add-entrada-modal/add-entrada-modal.component";
import {
  CreditInformationModalComponent
} from "../../components/credit-information-modal/credit-information-modal.component";
import { AddProductModalComponent } from "../../components/add-product-modal/add-product-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-sales-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
    NgForOf,
    SalesContainerComponent,
    SplitPipe,
    CurrencyPipe,
    NgStyle,
    NgIf,
    SearchContactModalComponent,
    ContactModalComponent,
    AddEntradaModalComponent,
    CreditInformationModalComponent,
    AddProductModalComponent
  ],
  templateUrl: "./sales-form.component.html"
})
export class SalesFormComponent implements OnInit {
  faFolderTree = faFolderTree;
  faUserAlt = faUserAlt;
  faPlus = faPlus;
  faSearch = faSearch;
  faIdCardAlt = faIdCardAlt;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faMessage = faMessage;
  faNoteSticky = faNoteSticky;
  faSave = faSave;
  faChevronCircleRight = faChevronCircleRight;
  faCircleLeft = faCircleLeft;
  // ====================================================================================================
  addEntradaModal: any;
  addProductModal: any;
  creditInformationModal: any;
  progressModal: any;
  searchContactModal: any;
  registerContactModal: any;
  contactDataModal: ContactDataModal = new ContactDataModal();
  // ====================================================================================================
  companyId: string = "";
  contactId: string = "-";
  invoiceSaleId: string = "-";
  invoiceSeries = new Array<InvoiceSerie>();
  itemComprobanteModal = new ItemComprobanteModal();
  cabeceraForm: FormGroup = this.fb.group({
    tipoDoc: [null, [Validators.required]],
    invoiceSerieId: [null, [Validators.required]],
    rznSocialUsuario: [null, [Validators.required]],
    remark: [""]
  });
  comprobanteDto: ComprobanteDto = new ComprobanteDto();
  sumTotValVenta: number = 0;
  sumTotIgvItem: number = 0;
  sumImpVenta: number = 0;
  // ====================================================================================================
  titleProgressModal: string = "";
  valueProgressBar: number = 0;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private contactService: ContactService,
    private invoiceSerieService: InvoiceSerieService,
    private invoiceSaleService: InvoiceSaleService,
    private userDataService: UserDataService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.activatedRoute.queryParams.subscribe(params => {
      this.contactId = params["cid"] || "";
      const origin: number = Number(params["origin"]);
      if (origin === 1) {
        this.contactService.show(this.contactId)
          .subscribe(result => {
            this.comprobanteDto.setCliente(result);
            this.cabeceraForm.controls["rznSocialUsuario"].setValue(result.name);
          });
      }
    });
    this.invoiceSerieService.index()
      .subscribe(result => this.invoiceSeries = result);
    this.creditInformationModal = new bootstrap.Modal("#credit-information-modal");
    this.progressModal = new bootstrap.Modal("#sales-form-progress-modal");
    this.searchContactModal = new bootstrap.Modal(EnumIdModal.ID_SEARCH_CONTACT_MODAL);
    this.registerContactModal = new bootstrap.Modal(EnumIdModal.ID_CONTACT_REGISTER_MODAL);
    this.addEntradaModal = new bootstrap.Modal(EnumIdModal.ID_ADD_ENTRADA_MANUAL_SALES);
    this.addProductModal = new bootstrap.Modal(EnumIdModal.ID_ADD_PRODUCT_MODAL_SALES);
  }

  public get company(): Company {
    return this.userDataService.currentCompany;
  }

  private calcularImpVenta(): void {
    this.sumTotValVenta = this.comprobanteDto.getValorVenta(this.company);
    this.sumTotIgvItem = this.comprobanteDto.getMtoIgv(this.company);
    this.sumImpVenta = this.comprobanteDto.getTotalCobrar();
  }

  public inputIsInvalid(field: string) {
    return this.cabeceraForm.controls[field].errors && this.cabeceraForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.cabeceraForm.invalid) {
      this.cabeceraForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    if (this.comprobanteDto.detalle.length <= 0) {
      toastError("Debe existir al menos un Item para facturar!");
      return;
    }
    if (this.cabeceraForm.get("tipoDoc")?.value === "01") {
      if (this.comprobanteDto.cabecera.tipDocUsuario !== "6:R.U.C") {
        toastError("Seleccione Cliente con R.U.C!");
        return;
      }
      if (this.comprobanteDto.cabecera.numDocUsuario.trim().length !== 11) {
        toastError("Ingrese Número de R.U.C Valido!");
        return;
      }
    }
    confirmTask().then(result => {
      if (result.isConfirmed) {
        // Guardar datos, sólo si es válido el formulario.
        if (this.cabeceraForm.get("tipoDoc")?.value === "03") {
          this.comprobanteDto.cabecera.paymentMethod = FormaPagoComprobante.Contado;
          this.comprobanteDto.formaPago.tipo = FormaPagoComprobante.Contado.split(":")[0].trim();
          this.comprobanteDto.formaPago.monto = this.comprobanteDto.getTotalCobrar();
          this.registrarComprobante();
        }
        if (this.cabeceraForm.get("tipoDoc")?.value === "01") {
          Swal.fire({
            title: "FORMA DE PAGO",
            input: "radio",
            inputOptions: {
              [FormaPagoComprobante.Credito]: "CRÉDITO",
              [FormaPagoComprobante.Contado]: "CONTADO",
            },
            confirmButtonText: "REGISTRAR FORMA DE PAGO",
            inputValidator(inputValue: string): any {
              if (!inputValue) return "¡Necesitas elegir Algo!";
            },
          }).then(result => {
            if (result.isConfirmed) {
              this.comprobanteDto.cabecera.paymentMethod = result.value;
              this.comprobanteDto.formaPago.tipo = result.value.split(":")[0].trim();
              if (result.value === FormaPagoComprobante.Credito) {
                this.creditInformationModal.show();
              }
              if (result.value === FormaPagoComprobante.Contado) {
                this.comprobanteDto.formaPago.monto = this.comprobanteDto.getTotalCobrar();
                this.registrarComprobante();
              }
            }
          });
        }
      }
    });
  }

  public saveChangesCreditInformation(data: CreditInformationDto): void {
    this.comprobanteDto.formaPago.monto = this.comprobanteDto.getTotalCobrar();
    this.comprobanteDto.cabecera.fecVencimiento = data.fecVencimiento;
    this.comprobanteDto.cuotas = _.map(data.cuotasPagoDto, item => {
      const cuota: Cuota = new Cuota();
      cuota.fechaPago = item.fecCuotaPago;
      cuota.monto = item.mtoCuotaPago;
      return cuota;
    });
    this.registrarComprobante();
    this.creditInformationModal.hide();
  }

  public irAListaComprobantes(): void {
    this.router.navigate(["/", this.companyId, "sales"]);
  }

  public showAddProductModal(): void {
    this.itemComprobanteModal = new ItemComprobanteModal();
    this.itemComprobanteModal.typeOper = "ADD";
    this.addProductModal.show();
  }

  public showAddEntradaModal(): void {
    this.itemComprobanteModal = new ItemComprobanteModal();
    this.itemComprobanteModal.typeOper = "ADD";
    this.addEntradaModal.show();
  }

  public agregarItemComprobante(item: ItemComprobanteModal): void {
    item.itemComprobanteDto.description = item.itemComprobanteDto.description.toUpperCase().trim();
    this.comprobanteDto.agregarItemAlDetalle(item.itemComprobanteDto);
    if (item.itemComprobanteDto.recordType === "PRODUCTO") this.addProductModal.hide();
    if (item.itemComprobanteDto.recordType === "ENTRADA") this.addEntradaModal.hide();
    this.calcularImpVenta();
  }

  public editItemModal(value: ItemComprobanteDto): void {
    this.itemComprobanteModal.itemComprobanteDto = value;
    this.itemComprobanteModal.typeOper = "EDIT";
    if (value.recordType === "PRODUCTO") this.addProductModal.show();
    if (value.recordType === "ENTRADA") this.addEntradaModal.show();
  }

  public deleteItemComprobante(item: ItemComprobanteDto): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.comprobanteDto.deleteItemByProductAndWarehouse(item.productId, item.warehouseId);
        this.calcularImpVenta();
      }
    });
  }

  public showRegisterContactModal(): void {
    this.contactDataModal.type = FormType.ADD;
    this.contactDataModal.title = "Agregar Contacto";
    this.contactDataModal.contact = new Contact();
    this.registerContactModal.show();
  }

  public saveContactRecordModal(dataModal: ContactDataModal): void {
    if (dataModal.type === FormType.ADD) {
      this.contactService.create(dataModal.contact)
        .subscribe(result => {
          this.comprobanteDto.setCliente(result);
          this.registerContactModal.hide();
        });
    }
  }

  public showSearchContactModal(): void {
    this.searchContactModal.show();
  }

  public selectContactModal(contact: Contact): void {
    this.comprobanteDto.setCliente(contact);
    this.cabeceraForm.controls["rznSocialUsuario"].setValue(contact.name);
    this.searchContactModal.hide();
  }

  private registrarComprobante(): void {
    this.valueProgressBar = 0;
    this.titleProgressModal = "Crear Comprobante!";
    this.progressModal.show();
    this.comprobanteDto.cabecera = {
      ...this.comprobanteDto.cabecera,
      ...this.cabeceraForm.value
    };
    this.invoiceSaleService.create(this.comprobanteDto)
      .subscribe(result => {
        this.titleProgressModal = "Comprobante registrado!";
        this.valueProgressBar = this.valueProgressBar + 50;
        if (result.data.success) {
          this.invoiceSaleId = result.invoiceId;
          this.titleProgressModal = result.data.cdrDescription;
          this.valueProgressBar = this.valueProgressBar + 50;
        } else {
          this.valueProgressBar = this.valueProgressBar + 50;
          toastError(result.data.cdrDescription);
        }
      });
  }

  public finalizar(): void {
    this.progressModal.hide();

    // Obtiene todos los elementos de entrada de tipo radio con el atributo "name" igual a "printFormat"
    const radioButtons = document.querySelectorAll("input[type=\"radio\"][name=\"printFormat\"]");

    // Recorre los botones de radio y verifica cuál está seleccionado
    let selectedValue: string = "-";
    radioButtons.forEach((radio: any) => {
      if (radio.checked) {
        selectedValue = radio.value;
      }
    });

    if (!isValidObjectId(this.invoiceSaleId)) {
      this.irAListaComprobantes();
    } else {
      if (selectedValue === "Ticket") {
        this.router.navigate([
          "/", this.companyId, "sales", "invoice-ticket", this.invoiceSaleId
        ]).then(() => console.log(selectedValue));
      }
      if (selectedValue === "A4") {
        this.router.navigate([
          "/", this.companyId, "sales", "invoice-formato-a4", this.invoiceSaleId
        ]).then(() => console.log(selectedValue));
      }
    }
  }

}
