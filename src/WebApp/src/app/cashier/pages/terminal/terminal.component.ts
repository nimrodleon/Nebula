import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {
  faBarcode, faCogs, faCoins, faIdCardAlt, faMinus, faPlus,
  faSearch, faSignOutAlt, faTags, faThList, faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import * as _ from 'lodash';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {ProductService} from 'src/app/products/services';
import {deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {Product} from 'src/app/products/interfaces';
import {Contact} from 'src/app/contact/interfaces';
import {ConfigurationService} from 'src/app/system/services';
import {ContactService} from 'src/app/contact/services';
import {AuthUser} from 'src/app/user/interfaces';
import {AuthService} from 'src/app/user/services';
import {CajaDiaria, CashierDetail, DetalleComprobante, GenerarVenta, ResponseCobrarModal} from '../../interfaces';
import {CajaDiariaService} from '../../services';

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
  cobrarModal: any;
  modalCajaChica: any;
  modalProducto: any;
  modalContacto: any;
  currentProduct: Product = new Product();
  currentContact: Contact = new Contact();
  // ====================================================================================================
  private appURL: string = environment.applicationUrl;
  queryProduct: FormControl = this.fb.control('');
  cajaDiaria: CajaDiaria = new CajaDiaria();
  products: Array<Product> = new Array<Product>();
  generarVenta: GenerarVenta = new GenerarVenta();
  authUser: AuthUser = new AuthUser();
  toastText: string = '';
  title: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private productService: ProductService,
    private cajaDiariaService: CajaDiariaService,
    private configurationService: ConfigurationService,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.cajaDiariaService.show(id).subscribe(result => {
      this.cajaDiaria = result;
      this.title = result.terminal;
    });
    this.authService.getMe().subscribe(result => this.authUser = result);
    // buscador de contactos.
    const contactSelect = jQuery('#contactSelect')
      .select2({
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
        this.generarVenta.comprobante.contactId = data.id;
      });
    // shortcode código de barra.
    const body: any = document.querySelector('body');
    body.addEventListener('keydown', (e: any) => {
      if (e.key === 'F2') (document.querySelector('#barcode') as any).focus();
    });
    this.loadConfigurationParameters();
    this.buscarProductos();

    this.cobrarModal = new bootstrap.Modal(document.querySelector('#cobrar-modal'));
    this.modalProducto = new bootstrap.Modal(document.querySelector('#product-modal'));
    this.modalContacto = new bootstrap.Modal(document.querySelector('#contact-modal'));
    this.modalCajaChica = new bootstrap.Modal(document.querySelector('#caja-chica-modal'));

    // limpiar el contacto seleccionado.
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hidden.bs.modal', () => {
      if (this.generarVenta.comprobante.contactId === '') contactSelect.val(null).trigger('change');
    });
  }

  public logout(): void {
    this.authService.logout();
  }

  private loadConfigurationParameters(): void {
    this.configurationService.show().subscribe(result => {
      this.generarVenta.configuration = result;
      this.contactService.show(result.contactId).subscribe(result => {
        this.generarVenta.comprobante.contactId = result.id;
        const newOption = new Option(`${result.document} - ${result.name}`, <any>result.id, true, true);
        jQuery('#contactSelect').append(newOption).trigger('change');
      });
    });
  }

  public seleccionarProducto(producto: Product): void {
    this.generarVenta.agregarProducto(producto);
  }

  public cambiarCantidad(productId: string, target: any): void {
    const value: number = Number(target.value);
    this.generarVenta.cambiarCantidad(productId, value);
  }

  public cambiarPrecio(e: Event, id: string): void {
    e.preventDefault();
    this.productService.show(id).subscribe(result => {
      const item = _.find(this.generarVenta.detallesComprobante, (o: DetalleComprobante) => o.productId === result.id);
      const toastSuccess = new bootstrap.Toast(document.querySelector('#toast-success'));
      const toastDanger = new bootstrap.Toast(document.querySelector('#toast-danger'));
      if (item) {
        if (item.quantity >= result.fromQty) {
          this.generarVenta.cambiarPrecio(result.id, result.price2);
          this.toastText = 'El precio se ha cambiado correctamente!';
          toastSuccess.show();
        } else {
          this.toastText = `La cantidad debe ser mayor o igual que ${result.fromQty}!`;
          toastDanger.show();
        }
      }
    });
  }

  public borrarItemVenta(productId: string): void {
    this.generarVenta.borrarItemVenta(productId);
  }

  public borrarVenta(): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.generarVenta.borrarVenta();
        this.loadConfigurationParameters();
      }
    });
  }

  public buscarProductos(): void {
    this.productService.index(this.queryProduct.value)
      .subscribe(result => this.products = result);
  }

  public modalAgregarProducto(): void {
    this.currentProduct = new Product();
    this.modalProducto.show();
  }

  public ocultarModalProducto(data: ResponseData<Product>): void {
    if (data.ok) {
      this.modalProducto.hide();
      this.buscarProductos();
    }
  }

  public modalAgregarContacto(): void {
    this.currentContact = new Contact();
    this.modalContacto.show();
  }

  public ocultarModalContacto(result: ResponseData<Contact>): void {
    if (result.ok) {
      const newOption = new Option(`${result.data?.document} - ${result.data?.name}`,
        <any>result.data?.id, true, true);
      jQuery('#contactSelect').append(newOption).trigger('change');
      this.generarVenta.comprobante.contactId = result.data?.id;
      this.modalContacto.hide();
    }
  }

  public abrirModalCajaChica(): void {
    this.modalCajaChica.show();
  }

  public ocultarModalCajaChica(data: ResponseData<CashierDetail>): void {
    if (data.ok) {
      this.modalCajaChica.hide();
    }
  }

  public async abrirCobrarModal() {
    if (this.generarVenta.detallesComprobante.length <= 0) {
      await Swal.fire(
        'Información',
        'Debe existir al menos un Item para facturar!',
        'info'
      );
    } else {
      this.cobrarModal.show();
    }
  }

  public ocultarCobrarModal(value: ResponseCobrarModal): void {
    if (value.status === 'COMPLETE') {
      this.loadConfigurationParameters();
      this.generarVenta.borrarVenta();
    }
    if (value.status === 'PRINT') {
      this.generarVenta.borrarVenta();
      this.router.navigate(['/cashier/ticket', value.data.invoiceSale]);
    }
  }

}
