import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {
  faArrowLeft, faEdit, faIdCardAlt,
  faPlus, faSave, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {Comprobante, CpeBase, CpeDetail, Cuota, TypeOperationSunat} from '../../interfaces';
import {FacturadorService, InvoiceSaleService, SunatService} from '../../services';
import {Contact} from 'src/app/contact/interfaces';
import {confirmTask, ResponseData} from 'src/app/global/interfaces';
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
  private appURL: string = environment.applicationUrl;
  currentContact: Contact = new Contact();
  contactModal: any;
  // TODO: debug -> $comprobante
  // datos del comprobante.
  comprobante: Comprobante = new Comprobante();
  // lista series de facturación.
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  // lista tipos de operación.
  typeOperation: Array<TypeOperationSunat> = new Array<TypeOperationSunat>();
  // TODO: debug -> $serieId
  // ID de la serie de facturación, se usa solo si es una venta.
  serieId: FormControl = this.fb.control('', [Validators.required]);
  // TODO: debug -> $comprobanteForm
  // formulario comprobante.
  comprobanteForm: FormGroup = this.fb.group({
    contactId: [null, [Validators.required]],
    startDate: [moment().format('YYYY-MM-DD'), [Validators.required]],
    docType: ['', [Validators.required]],
    formaPago: ['Contado', [Validators.required]],
    typeOperation: ['', [Validators.required]],
    serie: [''],
    number: [''],
    endDate: [null],
    remark: [''],
  });
  itemComprobanteModal: any;
  productId: string = '';
  currentCuota: Cuota = new Cuota();
  cuotaModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private invoiceSerieService: InvoiceSerieService,
    private facturadorService: FacturadorService,
    private invoiceService: InvoiceSaleService,
    private sunatService: SunatService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      switch (params.get('type')) {
        case 'purchase':
          // this.comprobante.invoiceType = 'COMPRA';
          this.serieId.disable();
          this.comprobanteForm.controls['serie'].setValidators([Validators.required]);
          this.comprobanteForm.controls['number'].setValidators([Validators.required]);
          if (params.get('id')) this.cargarComprobante(<any>params.get('id'));
          break;
        case 'sale':
          // this.comprobante.invoiceType = 'VENTA';
          this.serieId.setValidators([Validators.required]);
          if (params.get('id')) this.historyBack();
          break;
      }
    });
    // establecer valores por defecto en el modelo de datos.
    this.comprobante.formaPago = 'Contado';
    this.comprobante.startDate = moment().format('YYYY-MM-DD');
    // buscador de contactos.
    jQuery('#contactId').select2({
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

  // cargar comprobante modo edición.
  private cargarComprobante(id: number): void {
    this.invoiceService.show(id).subscribe(result => {
      // this.comprobante.invoiceId = result.id;
      // datos de contacto.
      this.comprobante.contactId = result.contactId;
      this.comprobanteForm.controls['contactId'].setValue(result.contactId);
      const newOption = new Option(`${result.numDocUsuario} - ${result.rznSocialUsuario}`,
        result.contactId, true, true);
      jQuery('#contactId').append(newOption).trigger('change');
      // fecha de emisión.
      this.comprobante.startDate = result.fecEmision;
      this.comprobanteForm.controls['startDate'].setValue(result.fecEmision);
      // tipo comprobante.
      this.comprobante.docType = result.docType;
      this.comprobanteForm.controls['docType'].setValue(result.docType);
      // forma de pago.
      this.comprobante.formaPago = result.formaPago;
      this.comprobanteForm.controls['formaPago'].setValue(result.formaPago);
      // tipo de operación.
      this.comprobante.typeOperation = result.tipOperacion;
      this.comprobanteForm.controls['typeOperation'].setValue(result.tipOperacion);
      // número y serie de comprobante.
      this.comprobante.serie = result.serie;
      this.comprobanteForm.controls['serie'].setValue(result.serie);
      this.comprobante.number = result.number;
      this.comprobanteForm.controls['number'].setValue(result.number);
      // fecha de vencimiento.
      if (result.formaPago === 'Credito') {
        if (result.fecVencimiento && result.fecVencimiento !== '-') {
          this.comprobante.endDate = result.fecVencimiento;
          this.comprobanteForm.controls['endDate'].setValue(result.fecVencimiento);
        }
        // agregar cuotas al comprobante.
        // result.invoiceAccounts.forEach(item => {
        //   const cuota: Cuota = new Cuota();
        //   cuota.id = item.id;
        //   cuota.numCuota = item.cuota;
        //   cuota.endDate = item.endDate;
        //   cuota.amount = item.amount;
        //   this.comprobante.addCuota(cuota);
        // });
      }
      // agregar detalles del comprobante.
      // result.invoiceDetails.forEach(item => {
      //   const itemDetail: CpeDetail = CpeBase.getItemDetail(item);
      //   itemDetail.calcularItem();
      //   this.comprobante.addItemWithData(itemDetail);
      // });
    });
  }

  // guardar comprobante de compra.
  private guardarCompra(): void {
    // this.invoiceService.createPurchase(this.comprobante)
    //   .subscribe(async (result) => {
    //     if (result.ok) {
    //       if (result.data) {
    //         const {data} = result;
    //         console.info(result.msg);
    //         await this.router.navigate(['/invoice/detail/purchase', data?.invoiceId]);
    //       }
    //     }
    //   });
  }

  // editar comprobante de compra.
  private editarCompra(): void {
    // this.invoiceService.UpdatePurchase(this.comprobante)
    //   .subscribe(async (result) => {
    //     if (result.ok) {
    //       if (result.data) {
    //         const {data} = result;
    //         console.info(result.msg);
    //         await this.router.navigate(['/invoice/detail/purchase', data?.invoiceId]);
    //       }
    //     }
    //   });
  }

  // guardar comprobante de venta.
  private guardarVenta(): void {
    // this.invoiceService.createSale(this.serieId.value, this.comprobante)
    //   .subscribe(result => {
    //     if (result.ok) {
    //       if (result.data) {
    //         const {data} = result;
    //         console.info(result.msg);
    //         this.crearXML(data);
    //       }
    //     }
    //   });
  }

  // crear archivo XML facturador SUNAT.
  private crearXML(data: Comprobante): void {
    // cargar la Lista de archivos JSON.
    this.facturadorService.ActualizarPantalla()
      .subscribe(result => {
        if (result.listaBandejaFacturador.length > 0) {
          // generar fichero XML del comprobante.
          // this.facturadorService.GenerarComprobante(data?.invoiceId)
          //   .subscribe(result => {
          //     if (result.listaBandejaFacturador.length > 0) {
          //       // generar fichero PDF del comprobante.
          //       this.facturadorService.GenerarPdf(data?.invoiceId)
          //         .subscribe(async (result) => {
          //           console.info(result);
          //           await this.router.navigate(['/invoice/detail/sale', data?.invoiceId]);
          //         });
          //     }
          //   });
        }
      });
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
    this.productId = '';
    this.itemComprobanteModal.show();
  }

  // editar modal item-comprobante.
  public editItemComprobanteModal(id: string): void {
    this.productId = id;
    this.itemComprobanteModal.show();
  }

  // ocultar modal item-comprobante.
  public hideItemComprobante(data: CpeDetail): void {
    this.comprobante.addItemWithData(data);
    this.itemComprobanteModal.hide();
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.comprobanteForm.controls[field].errors && this.comprobanteForm.controls[field].touched;
  }

  // registrar comprobante.
  public registerVoucher(): void {
    if (this.validHasError()) return;
    // Guardar datos, sólo si es válido el formulario.
    confirmTask().then(result => {
      if (result.isConfirmed) {
        // this.comprobante = {...this.comprobante, ...this.comprobanteForm.value};
        // if (this.comprobante.invoiceId !== undefined) {
        //   if (this.comprobante.invoiceType === 'COMPRA')
        //     this.editarCompra();
        // } else {
        //   // registrar comprobante.
        //   switch (this.comprobante.invoiceType) {
        //     case 'COMPRA':
        //       this.guardarCompra();
        //       break;
        //     case 'VENTA':
        //       this.guardarVenta();
        //       break;
        //   }
        // }
      }
    });
  }

  // abrir modal cuota.
  public showCuotaModal(): void {
    this.currentCuota = new Cuota();
    this.cuotaModal.show();
  }

  // editar modal cuota.
  public editCuotaModal(numCuota: number): void {
    const objTmp = this.comprobante.cuotas.find(x => x.numCuota === numCuota);
    if (objTmp) {
      objTmp.endDate = moment(objTmp.endDate).format('YYYY-MM-DD');
      this.currentCuota = objTmp;
    }
    this.cuotaModal.show();
  }

  // ocultar modal cuota.
  public hideCuotaModal(data: Cuota): void {
    if (data) {
      if (!data.numCuota) {
        this.comprobante.addCuota(data);
        this.cuotaModal.hide();
      } else {
        this.comprobante.cuotas.forEach(item => {
          if (item.numCuota === data.numCuota) {
            item.endDate = data.endDate;
            item.amount = data.amount;
            this.cuotaModal.hide();
          }
        });
      }
    }
  }

  // borrar item cuota.
  public deleteCuota(numCuota: number): void {
    this.comprobante.deleteCuota(numCuota);
  }

  // abrir modal agregar contacto.
  public showContactModal(): void {
    this.currentContact = new Contact();
    this.contactModal.show();
  }

  // ocultar modal contacto.
  public hideContactModal(response: ResponseData<Contact>): void {
    if (response.ok) {
      const newOption = new Option(response.data?.name, <any>response.data?.id, true, true);
      jQuery('#contactId').append(newOption).trigger('change');
      this.comprobanteForm.controls['contactId'].setValue(response.data?.id);
      this.contactModal.hide();
    }
  }

  // volver una página atrás.
  public historyBack(): void {
    window.history.back();
  }

  // validar formulario.
  private validHasError(): boolean {
    let hasError: boolean = false;
    if (this.comprobanteForm.invalid) {
      this.comprobanteForm.markAllAsTouched();
      hasError = true;
    }
    // if (this.comprobante.invoiceType === 'VENTA') {
    //   if (this.serieId.invalid) {
    //     this.serieId.markAsTouched();
    //     hasError = true;
    //   }
    // }
    if (hasError) return hasError;

    // validar detalle de factura.
    if (this.comprobante.details.length <= 0) {
      Swal.fire(
        'Información',
        'Debe existir al menos un Item para facturar!',
        'info'
      );
      hasError = true;
    }
    if (hasError) return hasError;

    // validar lista de cuotas si la factura es ha crédito.
    if (this.comprobanteForm.get('formaPago')?.value === 'Credito') {
      if (this.comprobante.cuotas.length <= 0) {
        Swal.fire(
          'Información',
          'Debe existir al menos una Cuota para facturar!',
          'info'
        );
        hasError = true;
      }
      if (hasError) return hasError;

      // validar si existe fecha de vencimiento.
      if (!this.comprobanteForm.get('endDate')?.value) {
        Swal.fire(
          'Información',
          'Debe ingresar fecha de vencimiento!',
          'info'
        );
        hasError = true;
      }
    }
    return hasError;
  }
}
