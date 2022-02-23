import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {
  faBarcode, faCogs, faCoins, faIdCardAlt, faMinus, faPlus,
  faSearch, faSignOutAlt, faTags, faThList, faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {ProductService} from 'src/app/products/services';
import {deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {Product} from '../../../products/interfaces';
import {Contact} from '../../../contact/interfaces';
import {CajaDiaria, CashierDetail, Sale} from '../../interfaces';
import {CajaDiariaService} from '../../services';
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
  faThList = faThList;
  // ====================================================================================================
  cajaDiariaId: number = 0;
  cobrarModal: any;
  cashInOutModal: any;
  productModal: any;
  contactModal: any;
  // ====================================================================================================
  private appURL: string = environment.applicationUrl;
  queryProduct: FormControl = this.fb.control('');
  configuration: Configuration = new Configuration();
  cajaDiaria: CajaDiaria = new CajaDiaria();
  currentProduct: Product = new Product();
  currentContact: Contact = new Contact();
  products: Array<Product> = new Array<Product>();
  sale: Sale = new Sale();
  toastText: string = '';

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productService: ProductService,
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
        this.sale.contactId = data.id;
      });
    // limpiar el cliente seleccionado.
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hidden.bs.modal', () => {
      if (this.sale.contactId === null) {
        clientId.val(null).trigger('change');
      }
    });
    // cargar valor inicial.
    this.sale = new Sale();
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
    this.configurationService.show().subscribe(result => {
      this.configuration = result;
      // cargar cliente por defecto.
      // TODO: corregir esta linea de código.
      // this.contactService.show(result.contactId).subscribe(result => {
      //   this.sale.contactId = result.id;
      //   const newOption = new Option(`${result.document} - ${result.name}`,
      //     <any>result.id, true, true);
      //   jQuery('#clientId').append(newOption).trigger('change');
      // });
    });
  }

  // buscar productos.
  public searchProducts(): void {
    // TODO: corregir esta linea de código.
    // this.productService.terminal(this.queryProduct.value)
    //   .subscribe(result => this.products = result);
  }

  // agregar nuevo producto.
  public addProductModal(): void {
    this.currentProduct = new Product();
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
    this.currentContact = new Contact();
    this.contactModal.show();
  }

  // cerrar modal contacto.
  public hideContactModal(result: ResponseData<Contact>): void {
    if (result.ok) {
      const newOption = new Option(`${result.data?.document} - ${result.data?.name}`,
        <any>result.data?.id, true, true);
      jQuery('#clientId').append(newOption).trigger('change');
      this.sale.contactId = result.data?.id;
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
      this.sale = new Sale();
    }
  }

  // borrar venta.
  public deleteSale(): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.sale = new Sale();
      }
    });
  }

  // agregar producto a la tabla.
  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      this.sale.addItemDetail(this.configuration, result);
    });
  }

  // cambiar la cantidad del item de la tabla.
  public changeQuantity(prodId: any, target: any): void {
    const value: number = Number(target.value);
    this.sale.changeQuantity(prodId, value);
  }

  // cambiar precio del Item.
  public changePrice(e: Event, id: number): void {
    e.preventDefault();
    const toastSuccess = new bootstrap.Toast(document.querySelector('#toast-success'));
    const toastDanger = new bootstrap.Toast(document.querySelector('#toast-danger'));
    const item = this.sale.details.find(x => x.productId === id);
    if (item) {
      this.productService.show(item.productId).subscribe(result => {
        if (item.quantity >= result.fromQty) {
          this.sale.changePrice(result.id, result.price2);
          this.toastText = 'El precio se ha cambiado correctamente!';
          toastSuccess.show();
        } else {
          this.toastText = `La cantidad debe ser mayor o igual que ${result.fromQty}!`;
          toastDanger.show();
        }
      });
    }
  }

  // borrar item de la tabla.
  public deleteItem(prodId: any): void {
    this.sale.deleteItem(prodId);
  }

}
