import {Component, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {Product} from 'src/app/products/interfaces';
import {ResponseData} from '../../../global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-item-comprobante',
  templateUrl: './item-comprobante.component.html',
  styleUrls: ['./item-comprobante.component.scss']
})
export class ItemComprobanteComponent implements OnInit {
  faBars = faBars;
  currentProduct: Product = new Product();
  productModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // modal agregar producto.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
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
