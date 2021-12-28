import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {
  faArrowLeft, faEdit, faIdCardAlt, faPlus,
  faSave, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {environment} from 'src/environments/environment';
import {Comprobante, CpeDetail, Cuota, TypeOperationSunat} from '../../interfaces';
import {InvoiceService, SunatService} from '../../services';
import {Contact} from 'src/app/contact/interfaces';
import {ResponseData} from 'src/app/global/interfaces';
import {InvoiceSerieService} from 'src/app/system/services';
import {InvoiceSerie} from 'src/app/system/interfaces';

declare var jQuery: any;
declare var bootstrap: any;

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  faSave = faSave;
  faPlus = faPlus;
  faArrowLeft = faArrowLeft;
  faIdCardAlt = faIdCardAlt;
  faEdit = faEdit;
  invoiceType: string = '';
  nomComprobante: string = '';
  private appURL: string = environment.applicationUrl;
  currentContact: Contact | any;
  contactModal: any;
  // datos del comprobante.
  comprobante: Comprobante = new Comprobante();
  // lista series de facturación.
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  // lista tipos de operación.
  typeOperation: Array<TypeOperationSunat> = new Array<TypeOperationSunat>();
  // ID de la serie de facturación, se usa solo si es una venta.
  serieId: FormControl = this.fb.control('');
  // formulario comprobante.
  comprobanteForm: FormGroup = this.fb.group({
    contactId: [null],
    startDate: [moment().format('YYYY-MM-DD')],
    docType: [''],
    formaPago: ['Contado'],
    typeOperation: [''],
    serie: [''],
    number: [''],
    endDate: [null],
    remark: [''],
  });
  itemComprobanteModal: any;
  cuotaModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private invoiceSerieService: InvoiceSerieService,
    private invoiceService: InvoiceService,
    private sunatService: SunatService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceType = params.get('type') || '';
      //  título del comprobante.
      switch (params.get('type')) {
        case 'purchase':
          this.nomComprobante = 'Compra';
          this.serieId.disable();
          break;
        case 'sale':
          this.nomComprobante = 'Venta';
          break;
      }
    });
    // establecer valores por defecto en el modelo de datos.
    this.comprobante.formaPago = 'Contado';
    this.comprobante.startDate = moment().format('YYYY-MM-DD');
    // buscador de contactos.
    jQuery('#clientId').select2({
      theme: 'bootstrap-5',
      placeholder: 'BUSCAR CONTACTO',
      ajax: {
        url: this.appURL + 'Contact/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    }).on('select2:select', (e: any) => {
      const data = e.params.data;
      this.comprobanteForm.controls['contactId'].setValue(data.id);
    });
    // series de facturación.
    this.invoiceSerieService.index().subscribe(result => this.invoiceSeries = result);
    // cargar lista tipos de operación.
    this.sunatService.typeOperation().subscribe(result => this.typeOperation = result);
    // modal formulario contacto.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
    // modal item comprobante.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
    // modal cuota crédito.
    this.cuotaModal = new bootstrap.Modal(document.querySelector('#cuota-modal'));
  }

  // borrar item comprobante.
  public deleteItem(prodId: number | any): void {
    this.comprobante.deleteItem(prodId);
  }

  // verificar forma de pago a Crédito.
  public checkFormaPago(): boolean {
    return this.comprobanteForm.get('formaPago')?.value === 'Credito';
  }

  // abrir modal item-comprobante.
  public showItemComprobanteModal(): void {
    this.itemComprobanteModal.show();
  }

  // ocultar modal item-comprobante.
  public hideItemComprobante(data: CpeDetail): void {
    this.comprobante.addItemWithData(data);
    this.itemComprobanteModal.hide();
  }

  // registrar comprobante.
  public async registerVoucher() {
    // if (this.comprobanteForm.get('paymentType')?.value === 'Contado') {
    //   this.comprobanteForm.controls['endDate'].setValue('1992-04-05');
    // }
    // this.invoiceService.create({
    //   ...this.comprobanteForm.value, invoiceType: this.invoiceType,
    //   details: this.detalleComprobante
    // }).subscribe(result => {
    //   let URI: string = '';
    //   switch (this.invoiceType.toUpperCase()) {
    //     case 'SALE':
    //       URI = '/sales';
    //       break;
    //     case 'PURCHASE':
    //       URI = '/shopping';
    //       break;
    //   }
    //   if (result.ok) {
    //     this.router.navigate([URI]);
    //   }
    // });
  }

  // abrir modal cuota.
  public showCuotaModal(): void {
    this.cuotaModal.show();
  }

  // ocultar modal cuota.
  public hideCuotaModal(data: Cuota): void {
    if (data) {
      this.comprobante.addCuota(data);
      this.cuotaModal.hide();
    }
  }

  // borrar item cuota.
  public deleteCuota(numCuota: number): void {
    this.comprobante.deleteCuota(numCuota);
  }

  // abrir modal agregar contacto.
  public showContactModal(): void {
    this.currentContact = null;
    this.contactModal.show();
  }

  // ocultar modal contacto.
  public hideContactModal(response: ResponseData<Contact>): void {
    if (response.ok) {
      const newOption = new Option(response.data?.name, <any>response.data?.id, true, true);
      jQuery('#clientId').append(newOption).trigger('change');
      this.comprobanteForm.controls['contactId'].setValue(response.data?.id);
      this.contactModal.hide();
    }
  }

}
