import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {
  faArrowLeft, faEdit, faIdCardAlt, faPlus,
  faSave, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {environment} from 'src/environments/environment';
import {Caja} from 'src/app/cashier/interfaces';
import {DetailComprobante, TypeOperationSunat} from '../../interfaces';
import {CajaService} from 'src/app/cashier/services';
import {SunatService} from '../../services';

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
  docType: string = '';
  nomComprobante: string = '';
  private appURL: string = environment.applicationUrl;
  listaDeCajas: Array<Caja> = new Array<Caja>();
  typeOperation: Array<TypeOperationSunat> = new Array<TypeOperationSunat>();
  comprobanteForm: FormGroup = this.fb.group({
    contactId: [null],
    startDate: [moment().format('YYYY-MM-DD')],
    docType: [''],
    cajaId: [''],
    formaPago: ['Contado'],
    typeOperation: [''],
    serie: [''],
    numero: [''],
    endDate: [null],
    sumTotValVenta: [0],
    sumTotTributos: [0],
    icbper: [0],
    sumImpVenta: [0],
    remark: [''],
  });
  detailComprobante: Array<DetailComprobante> = new Array<DetailComprobante>();
  itemComprobanteModal: any;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private cajaService: CajaService,
    private sunatService: SunatService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.docType = params.get('type') || '';
      //  título del comprobante.
      switch (params.get('type')) {
        case 'purchase':
          this.nomComprobante = 'Compra';
          // deshabilitar caja si es un comprobante de compras.
          this.comprobanteForm.controls['cajaId'].disable();
          break;
        case 'sale':
          this.nomComprobante = 'Venta';
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
    // cargar lista de cajas.
    this.cajaService.index().subscribe(result => this.listaDeCajas = result);
    // cargar lista tipos de operación.
    this.sunatService.typeOperation().subscribe(result => this.typeOperation = result);
    // modal item comprobante.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
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

  public checkCreditoFormaPago(): boolean {
    return this.comprobanteForm.get('formaPago')?.value === 'Credito';
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

}
