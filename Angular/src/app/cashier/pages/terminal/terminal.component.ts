import {Component, OnInit} from '@angular/core';
import {
  faBarcode, faCashRegister, faCogs,
  faCoins, faIdCardAlt, faMinus, faPlus,
  faSearch, faSignOutAlt, faTags, faThLarge,
  faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormControl} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {ProductService} from 'src/app/products/services';
import {deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {Product} from '../../../products/interfaces';
import {Contact} from '../../../contact/interfaces';
import {CashierDetail} from '../../interfaces';
import {TerminalService} from '../../services';

declare var jQuery: any;
declare var bootstrap: any;

@Component({
  selector: 'app-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent implements OnInit {
  faUserCircle = faUserCircle;
  faPlus = faPlus;
  faSearch = faSearch;
  faCoins = faCoins;
  faTrashAlt = faTrashAlt;
  faMinus = faMinus;
  faSignOutAlt = faSignOutAlt;
  faCashRegister = faCashRegister;
  faBarcode = faBarcode;
  faTags = faTags;
  faIdCardAlt = faIdCardAlt;
  faTimes = faTimes;
  faCogs = faCogs;
  faThLarge = faThLarge;
  // ====================================================================================================
  cajaDiariaId: number = 0;
  cobrarModal: any;
  cashInOutModal: any;
  // ====================================================================================================
  private appURL: string = environment.applicationUrl;
  queryProduct: FormControl = this.fb.control('');
  currentProduct: Product | any;
  currentContact: Contact | any;
  products: Array<Product> = new Array<Product>();
  productModal: any;
  contactModal: any;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productService: ProductService,
    private terminalService: TerminalService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.cajaDiariaId = Number(params.get('id'));
    });
    // buscador de clientes.
    jQuery('#clientId').select2({
      theme: 'bootstrap-5',
      placeholder: 'BUSCAR CLIENTE',
      ajax: {
        url: this.appURL + 'Contact/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    }).on('select2:select', (e: any) => {
      const data = e.params.data;
      this.terminalService.setClientId(data.id);
    });
    // limpiar el cliente seleccionado.
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hidden.bs.modal', () => {
      if (this.sale.clientId === null) {
        jQuery('#clientId').val(null).trigger('change');
      }
    });
    // cargar lista de productos.
    this.searchProducts();
    // modal formulario de productos.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
    // modal formulario de contactos.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
    // formulario modal cobrar.
    this.cobrarModal = new bootstrap.Modal(document.querySelector('#cobrar-modal'));
    // formulario entrada/salida de efectivo.
    this.cashInOutModal = new bootstrap.Modal(document.querySelector('#cash-in-out-modal'));
  }

  // información de la venta.
  public get sale() {
    return this.terminalService.sale;
  }

  // buscar productos.
  public searchProducts(): void {
    this.productService.terminal(this.queryProduct.value)
      .subscribe(result => this.products = result);
  }

  // agregar nuevo producto.
  public addProductModal(): void {
    this.currentProduct = null;
    this.productModal.show();
  }

  // cerrar modal producto.
  public hideProductModal(data: ResponseData<Product>): void {
    if (data.ok) {
      this.productModal.hide();
      this.searchProducts();
    }
  }

  // agregar contacto.
  public addContactModal(): void {
    this.currentContact = null;
    this.contactModal.show();
  }

  // cerrar modal contacto.
  public hideContactModal(data: ResponseData<Contact>): void {
    if (data.ok) {
      this.contactModal.hide();
      // TODO: seleccionar contacto.
    }
  }

  // movimientos de efectivo.
  public btnCashInOutClick(): void {
    this.cashInOutModal.show();
  }

  // cerrar modal movimientos de efectivo.
  public hideCashInOutModal(data: ResponseData<CashierDetail>): void {
    if (data.ok) {
      this.cashInOutModal.hide();
    }
  }

  // botón cobrar.
  public async btnCobrarClick() {
    if (this.sale.clientId === null) {
      await Swal.fire(
        'Información',
        'La información del cliente es requerida!',
        'info'
      );
    } else {
      if (this.sale.details.length <= 0) {
        await Swal.fire(
          'Información',
          'Debe existir al menos un Item para facturar!',
          'info'
        );
      } else {
        this.cobrarModal.show();
      }
    }
  }

  // borrar venta.
  public deleteSale(): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.terminalService.deleteSale();
      }
    });
  }

  // agregar producto a la tabla.
  public addItem(prodId: any): void {
    this.terminalService.addItem(prodId);
  }

  // cambiar la cantidad del item de la tabla.
  public changeQuantity(prodId: any, target: any): void {
    const value: number = Number(target.value);
    this.terminalService.changeQuantity(prodId, value);
  }

  // calcular el descuento por item.
  public calcDiscount(prodId: any, target: any): void {
    const value: number = Number(target.value);
    this.terminalService.calcDiscount(prodId, value);
  }

  // borrar item de la tabla.
  public deleteItem(prodId: any): void {
    this.terminalService.deleteItem(prodId);
  }

}
