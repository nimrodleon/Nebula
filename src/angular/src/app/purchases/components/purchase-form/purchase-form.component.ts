import {Component, Input, OnInit} from "@angular/core";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {
  faCalendar,
  faCircleLeft,
  faEdit,
  faFilter,
  faFolderTree,
  faHashtag,
  faIdCardAlt,
  faMessage,
  faPlus,
  faSackDollar,
  faSave,
  faSearch,
  faTrashAlt,
  faUserAlt
} from "@fortawesome/free-solid-svg-icons";
import {EnumIdModal, FormType, deleteConfirm, getIgvSunat, toastSuccess, toastError} from "app/common/interfaces";
import {
  CabeceraCompraDto,
  ItemCompraDto,
  ItemCompraForm,
  PurchaseDto,
  PurchaseInvoiceDetail
} from "../../interfaces/purchase-invoice";
import {Contact, ContactDataModal} from "app/contact/interfaces";
import {PurchaseInvoiceDetailService, PurchaseInvoiceService} from "../../services";
import {ContactService} from "app/contact/services";
import {UserDataService} from "app/common/user-data.service";
import _ from "lodash";
import {Company} from "../../../account/interfaces";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, NgClass, NgForOf} from "@angular/common";
import {SplitPipe} from "../../../common/pipes/split.pipe";
import {SearchContactModalComponent} from "app/common/contact/search-contact-modal/search-contact-modal.component";
import {ContactModalComponent} from "app/common/contact/contact-modal/contact-modal.component";
import {AddProductModalComponent} from "../add-product-modal/add-product-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-purchase-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
    CurrencyPipe,
    SplitPipe,
    RouterLink,
    NgForOf,
    SearchContactModalComponent,
    ContactModalComponent,
    AddProductModalComponent
  ],
  templateUrl: "./purchase-form.component.html"
})
export class PurchaseFormComponent implements OnInit {
  protected readonly faUserAlt = faUserAlt;
  protected readonly faPlus = faPlus;
  protected readonly faSearch = faSearch;
  protected readonly faIdCardAlt = faIdCardAlt;
  protected readonly faMessage = faMessage;
  protected readonly faSave = faSave;
  protected readonly faFilter = faFilter;
  protected readonly faCalendar = faCalendar;
  protected readonly faHashtag = faHashtag;
  protected readonly faSackDollar = faSackDollar;
  protected readonly faFolderTree = faFolderTree;
  protected readonly faEdit = faEdit;
  protected readonly faTrashAlt = faTrashAlt;
  protected readonly faCircleLeft = faCircleLeft;
  // ====================================================================================================
  @Input()
  typeForm: string = FormType.ADD;
  // ====================================================================================================
  purchaseDto = new PurchaseDto();
  cabeceraCompra = new CabeceraCompraDto();
  cabeceraForm: FormGroup = this.fb.group({
    fecEmision: [null, [Validators.required]],
    serieComprobante: [null, [Validators.required]],
    numComprobante: [null, [Validators.required]],
    tipoMoneda: ["PEN", [Validators.required]],
    tipoDeCambio: [1, [Validators.required, Validators.min(1)]],
    docType: [null, [Validators.required]],
    rznSocialProveedor: [null, [Validators.required]]
  });
  itemCompra = new ItemCompraForm();
  detalle = new Array<ItemCompraForm>();
  // totales del formulario.
  sumTotValCompra: number = 0;
  sumTotIgvItem: number = 0;
  sumTotIcbper: number = 0;
  sumImpCompra: number = 0;
  // ====================================================================================================
  searchContactModal: any;
  contactDataModal = new ContactDataModal();
  registerContactModal: any;
  addProductModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private contactService: ContactService,
    private globalService: UserDataService,
    private purchaseInvoiceService: PurchaseInvoiceService,
    private purchaseInvoiceDetailService: PurchaseInvoiceDetailService) {
  }

  ngOnInit(): void {
    if (this.typeForm === FormType.EDIT) {
      const id: string = this.activatedRoute.snapshot.params["id"];
      this.purchaseInvoiceService.show(id).subscribe(result => {
        this.purchaseDto = result;
        this.setCabeceraCompra(result);
        this.setCabeceraForm(result);
        this.setDetalle(result.purchaseInvoiceDetails);
      });
    }
    this.searchContactModal = new bootstrap.Modal(EnumIdModal.ID_SEARCH_CONTACT_MODAL);
    this.registerContactModal = new bootstrap.Modal(EnumIdModal.ID_CONTACT_REGISTER_MODAL);
    this.addProductModal = new bootstrap.Modal(EnumIdModal.ID_ADD_PRODUCT_MODAL_PURCHASES);
  }

  // todo: refactoring.
  public get configuration(): Company {
    return new Company();
  }

  private setCabeceraForm({purchaseInvoice}: PurchaseDto): void {
    this.cabeceraForm.controls["fecEmision"].setValue(purchaseInvoice.fecEmision);
    this.cabeceraForm.controls["serieComprobante"].setValue(purchaseInvoice.serie);
    this.cabeceraForm.controls["numComprobante"].setValue(purchaseInvoice.number);
    this.cabeceraForm.controls["tipoMoneda"].setValue(purchaseInvoice.tipMoneda);
    this.cabeceraForm.controls["tipoDeCambio"].setValue(purchaseInvoice.tipoCambio);
    this.cabeceraForm.controls["docType"].setValue(purchaseInvoice.docType);
    this.cabeceraForm.controls["rznSocialProveedor"].setValue(purchaseInvoice.rznSocialProveedor);
  }

  private setCabeceraCompra({purchaseInvoice}: PurchaseDto): void {
    this.cabeceraCompra.contactId = purchaseInvoice.contactId;
    this.cabeceraCompra.tipDocProveedor = purchaseInvoice.tipDocProveedor;
    this.cabeceraCompra.numDocProveedor = purchaseInvoice.numDocProveedor;
    this.cabeceraCompra.rznSocialProveedor = purchaseInvoice.rznSocialProveedor;
  }

  private setContactoCabecera(contacto: Contact): void {
    this.cabeceraCompra.contactId = contacto.id;
    this.cabeceraCompra.tipDocProveedor = contacto.docType;
    this.cabeceraCompra.numDocProveedor = contacto.document;
    this.cabeceraCompra.rznSocialProveedor = contacto.name;
    this.cabeceraForm.controls["rznSocialProveedor"].setValue(contacto.name);
  }

  private setDetalle(detalleCompra: Array<PurchaseInvoiceDetail>): void {
    this.detalle = [];
    _.forEach(detalleCompra, (o: PurchaseInvoiceDetail) => {
      const itemCompra = new ItemCompraForm();
      itemCompra.id = o.id;
      itemCompra.productId = o.codProducto;
      itemCompra.tipoItem = o.tipoItem;
      itemCompra.ctdUnidadItem = o.ctdUnidadItem;
      itemCompra.mtoPrecioCompraUnitario = o.mtoPrecioCompraUnitario;
      itemCompra.codUnidadMedida = o.codUnidadMedida;
      itemCompra.desItem = o.desItem;
      itemCompra.triIcbper = o.codTriIcbper !== "-";
      itemCompra.mtoValorCompraItem = o.mtoValorCompraItem;
      itemCompra.igvSunat = getIgvSunat(o.codTriIgv);
      itemCompra.mtoIgvItem = o.mtoIgvItem;
      itemCompra.mtoTriIcbperItem = o.mtoTriIcbperItem;
      itemCompra.mtoTotalItem = o.ctdUnidadItem * o.mtoPrecioCompraUnitario;
      this.detalle = _.concat(this.detalle, itemCompra);
    });
    this.calcularImpCompra();
  }

  private calcularImpCompra(): void {
    this.sumTotValCompra = _.sumBy(this.detalle, (o: ItemCompraForm) => o.mtoValorCompraItem);
    this.sumTotIgvItem = _.sumBy(this.detalle, (o: ItemCompraForm) => o.mtoIgvItem);
    const triIcbperRows = _.filter(this.detalle, (o: ItemCompraForm) => o.triIcbper);
    this.sumTotIcbper = _.sumBy(triIcbperRows, (o: ItemCompraForm) => o.mtoTriIcbperItem);
    this.sumImpCompra = this.sumTotValCompra + this.sumTotIgvItem + this.sumTotIcbper;
  }

  public showRegisterContactModal(): void {
    this.contactDataModal.type = FormType.ADD;
    this.contactDataModal.title = "Agregar Contacto";
    this.contactDataModal.contact = new Contact();
    this.registerContactModal.show();
  }

  public saveContactRecordModal(dataModal: ContactDataModal): void {
    if (dataModal.type === FormType.ADD) {
      this.contactService.create(dataModal.contact).subscribe(result => {
        toastSuccess(`El Proveedor, ${result.name} ha sido registrado!`);
        this.setContactoCabecera(result);
        this.registerContactModal.hide();
      });
    }
  }

  public showSearchContactModal(): void {
    this.searchContactModal.show();
  }

  public selectContactModal(contacto: Contact): void {
    this.setContactoCabecera(contacto);
    this.searchContactModal.hide();
  }

  public showAddProductModal(): void {
    this.itemCompra = new ItemCompraForm();
    this.addProductModal.show();
  }

  public agregarItemComprobante(item: ItemCompraForm): void {
    const itemCompra = new ItemCompraDto();
    itemCompra.productId = item.productId;
    itemCompra.tipoItem = item.tipoItem;
    itemCompra.ctdUnidadItem = item.ctdUnidadItem;
    itemCompra.codUnidadMedida = item.codUnidadMedida;
    itemCompra.desItem = item.desItem;
    itemCompra.triIcbper = item.triIcbper;
    itemCompra.igvSunat = item.igvSunat;
    itemCompra.mtoPrecioCompraUnitario = item.mtoPrecioCompraUnitario;
    if (item.typeOper === FormType.ADD) {
      itemCompra.id = undefined;
      const purchaseId = this.purchaseDto.purchaseInvoice.id;
      this.purchaseInvoiceDetailService.create(purchaseId, itemCompra)
        .subscribe(result => {
          item.id = result.id;
          this.detalle = _.concat(this.detalle, item);
          this.calcularImpCompra();
          this.addProductModal.hide();
          toastSuccess("Nuevo producto agregado a la tabla!");
        });
    }
    if (item.typeOper === FormType.EDIT) {
      itemCompra.id = item.id;
      this.purchaseInvoiceDetailService.update(itemCompra.id, itemCompra)
        .subscribe(() => {
          this.detalle = _.map(this.detalle, (o: ItemCompraForm) => {
            if (o.id === item.id) o = item;
            return o;
          });
          this.calcularImpCompra();
          this.addProductModal.hide();
          toastSuccess("El producto de la tabla ha sido actualizado!");
        });
    }
  }

  public editItemModal(item: ItemCompraForm): void {
    this.itemCompra = item;
    this.itemCompra.typeOper = FormType.EDIT;
    this.addProductModal.show();
  }

  public deleteItemComprobante(item: ItemCompraForm): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.purchaseInvoiceDetailService.delete(item.id)
          .subscribe(result => {
            this.detalle = _.filter(this.detalle, item => item.id !== result.id);
            this.calcularImpCompra();
          });
      }
    });
  }

  public inputIsInvalid(field: string) {
    return this.cabeceraForm.controls[field].errors
      && this.cabeceraForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.cabeceraForm.invalid) {
      this.cabeceraForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.cabeceraCompra = {...this.cabeceraCompra, ...this.cabeceraForm.value};
    if (this.typeForm === FormType.ADD) {
      this.purchaseInvoiceService.create(this.cabeceraCompra)
        .subscribe(result => {
          toastSuccess(`El comprobante ${result.serie}-${result.number} ha sido registrado!`);
          this.router.navigate(["/purchases/edit", result.id]);
        });
    }
    if (this.typeForm === FormType.EDIT) {
      const {purchaseInvoice} = this.purchaseDto;
      this.purchaseInvoiceService.update(purchaseInvoice.id, this.cabeceraCompra)
        .subscribe(result => {
          toastSuccess(`El comprobante ${result.serie}-${result.number} ha sido actualizado!`);
        });
    }
  }

  public irAListaDeCompras(): void {
    this.router.navigate(["/purchases"]);
  }

}
