import {Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {faBars, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {environment} from 'src/environments/environment';
import {ProductService, UndMedidaService} from '../../services';
import {Category, Product, UndMedida} from '../../interfaces';
import {ResponseData} from '../../../global/interfaces';

declare var jQuery: any;
declare var bootstrap: any;

@Component({
  selector: 'app-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent implements OnInit {
  faBars = faBars;
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  private appURL: string = environment.applicationUrl;
  undMedidas: Array<UndMedida> = new Array<UndMedida>();
  productForm: FormGroup = this.fb.group({
    id: [null],
    description: [''],
    barcode: [''],
    icbper: [''],
    price: [0],
    igvSunat: [''],
    type: [''],
    undMedidaId: [null]
  });
  fileImage: any;
  @ViewChild('inputFile')
  inputFile: ElementRef | undefined;
  // ====================================================================================================
  @Input()
  title: string = '';
  @Input()
  product: Product = new Product();
  @Output()
  responseData = new EventEmitter<ResponseData<Product>>();
  categoryModal: any;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private undMedidaService: UndMedidaService) {
  }

  ngOnInit(): void {
    // cargar lista de categorías.
    jQuery('#categoryId').select2({
      theme: 'bootstrap-5',
      placeholder: 'BUSCAR CATEGORÍA',
      dropdownParent: jQuery('#product-modal'),
      ajax: {
        url: this.appURL + 'Category/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    });
    // cargar las unidades de medidas.
    this.undMedidaService.index().subscribe(result => this.undMedidas = result);
    // suscribir formGroup.
    this.productForm.valueChanges.subscribe(value => this.product = value);
    // cargar valores por defecto.
    if (document.querySelector('#product-modal')) {
      const myModal: any = document.querySelector('#product-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        this.productForm.reset({...this.product});
        // @ts-ignore
        this.inputFile?.nativeElement.value = null;
        this.fileImage = null;
      });
    }
    // formulario modal categoría.
    this.categoryModal = new bootstrap.Modal(document.querySelector('#category-modal'));
  }

  // seleccionar imagen.
  public selectedImage(event: any): void {
    this.fileImage = event.target.files[0];
  }

  // guardar todos los cambios.
  public saveChanges(): void {
    const formData = new FormData();
    formData.append('description', this.product.description);
    formData.append('barcode', this.product.barcode);
    formData.append('icbper', this.product.icbper);
    formData.append('price', this.product.price.toString());
    formData.append('igvSunat', this.product.igvSunat);
    formData.append('type', this.product.type);
    formData.append('undMedidaId', this.product.undMedidaId);
    if (this.fileImage) formData.append('file', this.fileImage);
    if (this.productForm.get('id')?.value == null) {
      this.productService.store(formData)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    } else {
      formData.append('id', <any>this.product.id);
      this.productService.update(<any>this.product.id, formData)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    }
  }

  // agregar categoría.
  public showCategoryModal(e: Event): void {
    e.preventDefault();
    this.categoryModal.show();
  }

  // ocultar categoría.
  public hideCategoryModal(response: ResponseData<Category>): void {
    if (response.ok) {
      const newOption = new Option(response.data?.name, <any>response.data?.id, true, true);
      jQuery('#categoryId').append(newOption).trigger('change');
      this.categoryModal.hide();
    }
  }

}
