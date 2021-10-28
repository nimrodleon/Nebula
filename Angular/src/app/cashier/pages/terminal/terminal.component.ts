import {Component, OnInit} from '@angular/core';
import {
  faBarcode, faBars,
  faCashRegister, faCogs,
  faCoins, faIdCardAlt, faMinus, faPlus, faQrcode,
  faSearch, faSignOutAlt, faTags, faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormControl} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {ProductService} from '../../../products/services';
import {ResponseData} from '../../../global/interfaces';
import {Product} from '../../../products/interfaces';

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
  faBars = faBars;
  faTags = faTags;
  faIdCardAlt = faIdCardAlt;
  faTimes = faTimes;
  faCogs = faCogs;
  faQrcode = faQrcode;
  // ====================================================================================================
  cajaDiariaId: string = '';
  cobrarModal: any;
  cashInOutModal: any;
  // ====================================================================================================
  queryProduct: FormControl = this.fb.control('');
  currentProduct: Product = new Product();
  products: Array<Product> = new Array<Product>();
  productModal: any;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productService: ProductService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.cajaDiariaId = params.get('id') || '';
    });
    // cargar lista de productos.
    this.searchProducts();
    // modal formulario de productos.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
    // formulario modal cobrar.
    this.cobrarModal = new bootstrap.Modal(document.querySelector('#cobrar-modal'));
    // formulario entrada/salida de efectivo.
    this.cashInOutModal = new bootstrap.Modal(document.querySelector('#cash-in-out-modal'));
  }

  // buscar productos.
  public searchProducts(): void {
    this.productService.terminal(this.queryProduct.value)
      .subscribe(result => this.products = result);
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

  // // movimientos de efectivo.
  // btnCashInOutClick(): void {
  //   this.cashInOutModal.show();
  // }

  // botón vender.
  btnVenderClick(): void {
    this.cobrarModal.show();
  }

}
