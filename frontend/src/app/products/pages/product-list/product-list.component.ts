import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  // ====================================================================================================
  productModal: any;
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
    // modal formulario de productos.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
  }

  // agregar nuevo producto.
  addProduct(): void {
    this.title = 'Agregar Producto';
    this.productModal.show();
  }

}
