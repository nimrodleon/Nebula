import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ProductService} from '../../services';
import {AuthService} from 'src/app/user/services';
import {accessDenied, deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {Product} from '../../interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html'
})
export class ProductListComponent implements OnInit {
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  faFilter = faFilter;
  // ====================================================================================================
  currentProduct: Product = new Product();
  products: Array<Product> = new Array<Product>();
  query: FormControl = this.fb.control('');
  productModal: any;
  title: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private productService: ProductService) {
  }

  ngOnInit(): void {
    // modal formulario de productos.
    this.productModal = new bootstrap.Modal(document.querySelector('#product-modal'));
    // cargar lista de productos.
    this.cargarListaDeProductos();
  }

  // lista de productos.
  private cargarListaDeProductos(): void {
    this.productService.index(this.query.value)
      .subscribe(result => this.products = result);
  }

  // botón buscar productos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.cargarListaDeProductos();
  }

  // agregar nuevo producto.
  public addProductModal(): void {
    this.title = 'Agregar Producto';
    this.currentProduct = new Product();
    this.productModal.show();
  }

  // editar producto seleccionado.
  public editProductModal(id: any): void {
    this.title = 'Editar Producto';
    this.productService.show(id).subscribe(result => {
      this.currentProduct = result;
      this.productModal.show();
    });
  }

  // cerrar modal producto.
  public hideProductModal(data: ResponseData<Product>): void {
    if (data.ok) {
      this.productModal.hide();
      this.cargarListaDeProductos();
    }
  }

  // borrar producto.
  public deleteProduct(id: string): void {
    this.authService.getMe().subscribe(async (result) => {
      if (result.role !== 'Admin') {
        await accessDenied();
      } else {
        deleteConfirm().then(result => {
          if (result.isConfirmed) {
            this.productService.delete(id).subscribe(result => {
              if (result.ok) this.cargarListaDeProductos();
            });
          }
        });
      }
    });
  }

}
