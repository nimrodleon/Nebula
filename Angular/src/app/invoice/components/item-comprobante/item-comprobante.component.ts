import {Component, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {environment} from '../../../../environments/environment';
import {Product} from 'src/app/products/interfaces';
import {ResponseData} from '../../../global/interfaces';

declare var jQuery: any;
declare var bootstrap: any;

@Component({
  selector: 'app-item-comprobante',
  templateUrl: './item-comprobante.component.html',
  styleUrls: ['./item-comprobante.component.scss']
})
export class ItemComprobanteComponent implements OnInit {
  faBars = faBars;
  private appURL: string = environment.applicationUrl;
  currentProduct: Product = new Product();
  productModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // modal agregar producto.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
    // buscar producto.
    jQuery('#productId').select2({
      theme: 'bootstrap-5',
      dropdownParent: jQuery('#item-comprobante'),
      placeholder: 'BUSCADOR DE PRODUCTOS Y SERVICIOS',
      ajax: {
        url: this.appURL + 'Product/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    });

  }

  // mostrar modal producto.
  public showProductModal(e: any): void {
    e.preventDefault();
    this.productModal.show();
  }

  // ocultar modal producto.
  public hideProductModal(data: ResponseData<Product>): void {
    if (data.ok) {
      this.productModal.hide();
    }
  }

}
