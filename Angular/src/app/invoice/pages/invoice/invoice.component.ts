import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {
  faArrowLeft, faEdit, faIdCardAlt, faPlus,
  faSave, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {environment} from 'src/environments/environment';
import {Caja, Cuota} from 'src/app/cashier/interfaces';
import {DetailComprobante, TypeOperationSunat} from '../../interfaces';
import {CajaService} from 'src/app/cashier/services';
import {InvoiceService, SunatService} from '../../services';
import {Contact} from '../../../contact/interfaces';
import {ResponseData} from '../../../global/interfaces';

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
  listaDeCajas: Array<Caja> = new Array<Caja>();
  typeOperation: Array<TypeOperationSunat> = new Array<TypeOperationSunat>();
  comprobanteForm: FormGroup = this.fb.group({
    contactId: [null],
    startDate: [moment().format('YYYY-MM-DD')],
    docType: [''],
    cajaId: [''],
    paymentType: ['Contado'],
    typeOperation: [''],
    serie: [''],
    number: [''],
    endDate: [null],
    sumTotValVenta: [0],
    sumTotTributos: [0],
    icbper: [0],
    sumImpVenta: [0],
    remark: [''],
  });
  detailComprobante: Array<DetailComprobante> = new Array<DetailComprobante>();
  itemComprobanteModal: any;
  listaDeCuotas: Array<Cuota> = new Array<Cuota>();
  cuotaModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private cajaService: CajaService,
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
          // deshabilitar caja si es un comprobante de compras.
          this.comprobanteForm.controls['cajaId'].disable();
          break;
        case 'sale':
          this.nomComprobante = 'Venta';
          this.comprobanteForm.controls['startDate'].disable();
          break;
      }
    });
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
    // modal formulario contacto.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
    // cargar lista de cajas.
    this.cajaService.index().subscribe(result => this.listaDeCajas = result);
    // cargar lista tipos de operación.
    this.sunatService.typeOperation().subscribe(result => this.typeOperation = result);
    // modal item comprobante.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
    // modal cuota crédito.
    this.cuotaModal = new bootstrap.Modal(document.querySelector('#cuota-modal'));
  }

  // calcular importe venta.
  private calcImporteVenta(): void {
    let total = 0;
    this.detailComprobante.forEach(item => {
      total = total + item.amount;
    });
    let subTotal: number = total / 1.18;
    this.comprobanteForm.controls['sumImpVenta'].setValue(total);
    this.comprobanteForm.controls['sumTotValVenta'].setValue(subTotal);
    this.comprobanteForm.controls['sumTotTributos'].setValue(total - subTotal);
  }

  // verificar forma de pago a Crédito.
  public checkCreditPaymentType(): boolean {
    return this.comprobanteForm.get('paymentType')?.value === 'Credito';
  }

  // abrir modal item-comprobante.
  public showItemComprobanteModal(): void {
    this.itemComprobanteModal.show();
  }

  // ocultar modal item-comprobante.
  public hideItemComprobante(data: DetailComprobante): void {
    data.numItem = this.detailComprobante.length + 1;
    this.detailComprobante.push(data);
    this.calcImporteVenta();
    this.itemComprobanteModal.hide();
  }

  // borrar item comprobante.
  public deleteItemComprobante(numItem: number): void {
    let deleted: Boolean = false;
    this.detailComprobante.forEach((value, index, array) => {
      if (value.numItem === numItem) {
        array.splice(index, 1);
        deleted = true;
      }
    });
    if (deleted) {
      for (let i = 0; i < this.detailComprobante.length; i++) {
        this.detailComprobante[i].numItem = i + 1;
      }
    }
  }

  // registrar comprobante.
  public async registerVoucher() {
    if (this.comprobanteForm.get('paymentType')?.value === 'Contado') {
      this.comprobanteForm.controls['endDate'].setValue('1992-04-05');
    }
    this.invoiceService.store({
      ...this.comprobanteForm.value, invoiceType: this.invoiceType,
      details: this.detailComprobante
    }).subscribe(result => {
      let URI: string = '';
      switch (this.invoiceType.toUpperCase()) {
        case 'SALE':
          URI = '/sales';
          break;
        case 'PURCHASE':
          URI = '/shopping';
          break;
      }
      if (result.ok) {
        this.router.navigate([URI]);
      }
    });
  }

  // abrir modal cuota.
  public showCuotaModal(): void {
    this.cuotaModal.show();
  }

  // ocultar modal cuota.
  public hideCuotaModal(data: Cuota): void {
    if (data) {
      data.numCuota = this.listaDeCuotas.length + 1;
      this.listaDeCuotas.push(data);
      this.cuotaModal.hide();
    }
  }

  // borrar item cuota.
  public deleteItemCuota(numCuota: number): void {
    let deleted: Boolean = false;
    this.listaDeCuotas.forEach((value, index, array) => {
      if (value.numCuota === numCuota) {
        array.splice(index, 1);
        deleted = true;
      }
    });
    if (deleted) {
      for (let i = 0; i < this.listaDeCuotas.length; i++) {
        this.listaDeCuotas[i].numCuota = i + 1;
      }
    }
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
