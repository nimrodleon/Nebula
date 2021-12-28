import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {
  faBarcode, faCogs, faCoins, faIdCardAlt, faMinus, faPlus,
  faSearch, faSignOutAlt, faTags, faThLarge, faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {ProductService} from 'src/app/products/services';
import {deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {Product} from '../../../products/interfaces';
import {Contact} from '../../../contact/interfaces';
import {CajaDiaria, CashierDetail} from '../../interfaces';
import {CajaDiariaService, TerminalService} from '../../services';
import {Configuration} from '../../../system/interfaces';
import {ConfigurationService} from '../../../system/services';
import {ContactService} from '../../../contact/services';

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
  productModal: any;
  contactModal: any;
  // ====================================================================================================
  private appURL: string = environment.applicationUrl;
  queryProduct: FormControl = this.fb.control('');
  config: Configuration | any;
  cajaDiaria: CajaDiaria | any;
  currentProduct: Product | any;
  currentContact: Contact | any;
  products: Array<Product> = new Array<Product>();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productService: ProductService,
    private terminalService: TerminalService,
    private cajaDiariaService: CajaDiariaService,
    private configurationService: ConfigurationService,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.cajaDiariaId = Number(params.get('id'));
      // cargar caja diaria.
      this.cajaDiariaService.show(this.cajaDiariaId)
        .subscribe(result => this.cajaDiaria = result);
    });
    // buscador de clientes.
    const clientId = jQuery('#clientId')
      .select2({
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
        this.terminalService.setContactId(data.id);
      });
    // limpiar el cliente seleccionado.
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hidden.bs.modal', () => {
      if (this.sale.contactId === null) {
        clientId.val(null).trigger('change');
      }
    });
    // cargar valor inicial.
    this.terminalService.deleteSale();
    // cargar parámetros del configuración.
    this.getDefaultParams();
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

  // Cargar parámetros por defecto.
  private getDefaultParams(): void {
    this.configurationService.show()
      .subscribe(result => {
        this.config = result;
        this.terminalService.setConfig(result);
        // cargar cliente por defecto.
        this.contactService.show(result.contactId)
          .subscribe(result => {
            this.terminalService.setContactId(<any>result.id);
            const newOption = new Option(`${result.document} - ${result.name}`,
              <any>result.id, true, true);
            jQuery('#clientId').append(newOption).trigger('change');
          });
      });
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
  public hideContactModal(result: ResponseData<Contact>): void {
    if (result.ok) {
      const newOption = new Option(`${result.data?.document} - ${result.data?.name}`,
        <any>result.data?.id, true, true);
      jQuery('#clientId').append(newOption).trigger('change');
      this.terminalService.setContactId(<any>result.data?.id);
      this.contactModal.hide();
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
    if (this.sale.contactId === null) {
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

  // cerrar modal cobrar.
  public hideCobrarModal(value: boolean): void {
    if (!value) {
      this.getDefaultParams();
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

  // cambiar precio del Item.
  public changePrice(e: Event): void {
    e.preventDefault();
    alert('Cambiar precio');
  }

  // borrar item de la tabla.
  public deleteItem(prodId: any): void {
    this.terminalService.deleteItem(prodId);
  }

}
