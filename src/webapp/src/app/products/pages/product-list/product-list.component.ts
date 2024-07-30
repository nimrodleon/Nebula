import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {faBox, faEdit, faFilter, faList, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {ProductService, ProductStockService} from "../../services";
import {Product, ProductDataModal, ProductStockInfo} from "../../interfaces";
import {UserDataService} from "app/common/user-data.service";
import {accessDenied, deleteConfirm, FormType, PaginationResult, toastSuccess} from "app/common/interfaces";
import _ from "lodash";
import {ProductContainerComponent} from "app/common/containers/product-container/product-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {SplitPipe} from "app/common/pipes/split.pipe";
import {LoaderComponent} from "../../../common/loader/loader.component";
import {ProductModalComponent} from "../../components/product-modal/product-modal.component";
import {ProductStockModalComponent} from "../../components/product-stock-modal/product-stock-modal.component";
import {CargaMasivaProductosComponent} from "../../components/carga-masiva-productos/carga-masiva-productos.component";

declare const bootstrap: any;

@Component({
  selector: "app-product-list",
  standalone: true,
  imports: [
    ProductContainerComponent,
    FaIconComponent,
    ReactiveFormsModule,
    RouterLink,
    NgForOf,
    NgIf,
    NgClass,
    SplitPipe,
    DatePipe,
    CurrencyPipe,
    LoaderComponent,
    ProductModalComponent,
    ProductStockModalComponent,
    CargaMasivaProductosComponent
  ],
  templateUrl: "./product-list.component.html"
})
export class ProductListComponent implements OnInit {
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faPlus = faPlus;
  faFilter = faFilter;
  faBox = faBox;
  faList = faList;
  // ====================================================================================================
  products: PaginationResult<Product> = new PaginationResult<Product>();
  query: FormControl = this.fb.control("");
  productStockInfos: ProductStockInfo[] = new Array<ProductStockInfo>();
  productDataModal: ProductDataModal = new ProductDataModal();
  productModal: any;
  productStockModal: any;
  cargaMasivaProductos: any;
  loading: boolean = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userDataService: UserDataService,
    private productService: ProductService,
    private productStockService: ProductStockService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarListaDeProductos(page);
    });
    this.productModal = new bootstrap.Modal("#product-modal");
    this.productStockModal = new bootstrap.Modal("#productStockModal");
    this.cargaMasivaProductos = new bootstrap.Modal("#cargaMasivaProductos");
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  public get hasPermission(): boolean {
    return this.userDataService.isUserAdmin();
  }

  public saveChangesDetail(data: ProductDataModal): void {
    if (data.type === FormType.ADD) {
      data.product.id = undefined;
      this.productService.create(data.product)
        .subscribe(result => {
          this.products.data = _.concat(result, this.products.data);
          toastSuccess("El producto ha sido registrado!");
          this.productModal.hide();
        });
    }
    if (data.type === FormType.EDIT) {
      this.productService.update(data.product.id, data.product)
        .subscribe(result => {
          this.products.data = _.map(this.products.data, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          toastSuccess("El producto ha sido actualizado!");
          this.productModal.hide();
        });
    }
  }

  // lista de productos.
  private cargarListaDeProductos(page: number = 1): void {
    this.productService.index(this.query.value, page)
      .subscribe(result => this.products = result);
  }

  // botón buscar productos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.cargarListaDeProductos();
  }

  // agregar nuevo producto.
  public addProductModal(): void {
    this.productDataModal.type = "ADD";
    this.productDataModal.title = "Agregar Producto";
    this.productDataModal.product = new Product();
    this.productModal.show();
  }

  public editProductModal(product: Product): void {
    this.productDataModal.type = "EDIT";
    this.productDataModal.title = "Editar Producto";
    this.productDataModal.product = product;
    this.productModal.show();
  }

  public deleteProduct(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.productService.delete(id)
            .subscribe(result => {
              this.products.data = _.filter(this.products.data, item => item.id !== result.id);
              this.router.navigate(["/products"]).then(() => console.log(result.id));
            });
        }
      });
    }
  }

  public showProductStock(productId: string): void {
    this.productStockService.getStockInfos(productId)
      .subscribe(result => {
        this.productStockInfos = result;
        this.productStockModal.show();
      });
  }

  public showCargaMasivaProductos(event: Event): void {
    event.preventDefault();
    this.cargaMasivaProductos.show();
  }

  public changeLoading(value: boolean): void {
    this.loading = value;
  }

  public statusCargaMasivaProductos(value: boolean): void {
    if (value) {
      this.cargaMasivaProductos.hide();
      this.cargarListaDeProductos();
    }
  }

}
