import {Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {faBars, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {environment} from 'src/environments/environment';
import {CategoryService, ProductService} from '../../services';
import {ResponseData} from 'src/app/global/interfaces';
import {Category, Product} from '../../interfaces';

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
  productForm: FormGroup = this.fb.group({
    id: [null],
    description: ['', [Validators.required]],
    barcode: ['', [Validators.required]],
    price1: [0, [Validators.required, Validators.min(0)]],
    price2: [0, [Validators.required, Validators.min(0)]],
    fromQty: [0, [Validators.required, Validators.min(0)]],
    igvSunat: ['', [Validators.required]],
    icbper: ['', [Validators.required]],
    category: ['', [Validators.required]],
    undMedida: ['', [Validators.required]],
    type: ['', [Validators.required]],
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
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    // cargar lista de categorías.
    const category = jQuery('#category')
      .select2({
        theme: 'bootstrap-5',
        placeholder: 'BUSCAR CATEGORÍA',
        dropdownParent: jQuery('#product-modal'),
        ajax: {
          url: this.appURL + 'Category/Select2',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
          }
        }
      }).on('select2:select', (e: any) => {
        const data = e.params.data;
        this.productForm.controls['category'].setValue(`${data.id}:${data.text}`);
      });
    // cargar valores por defecto.
    if (document.querySelector('#product-modal')) {
      const myModal: any = document.querySelector('#product-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        this.productForm.reset({...this.product});
        // @ts-ignore
        this.inputFile?.nativeElement.value = null;
        this.fileImage = null;
        // cargar categoría de producto.
        if (this.product?.category === undefined
          || this.product?.category === null) {
          category.val(null).trigger('change');
        } else {
          this.categoryService.show(this.product.category.split(':')[0]?.trim())
            .subscribe(result => {
              const newOption = new Option(result.name, <any>result.id, true, true);
              category.append(newOption).trigger('change');
            });
        }
      });
    }
    // formulario modal categoría.
    this.categoryModal = new bootstrap.Modal(document.querySelector('#category-modal'));
  }

  // seleccionar imagen.
  public selectedImage(event: any): void {
    this.fileImage = event.target.files[0];
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.productForm.controls[field].errors && this.productForm.controls[field].touched;
  }

  // guardar todos los cambios.
  public saveChanges(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const formData = new FormData();
    formData.append('description', this.productForm.get('description')?.value);
    formData.append('barcode', this.productForm.get('barcode')?.value);
    formData.append('price1', this.productForm.get('price1')?.value);
    formData.append('price2', this.productForm.get('price2')?.value);
    formData.append('fromQty', this.productForm.get('fromQty')?.value);
    formData.append('igvSunat', this.productForm.get('igvSunat')?.value);
    formData.append('icbper', this.productForm.get('icbper')?.value);
    formData.append('category', this.productForm.get('category')?.value);
    formData.append('undMedida', this.productForm.get('undMedida')?.value);
    formData.append('type', this.productForm.get('type')?.value);
    if (this.fileImage) formData.append('file', this.fileImage);
    if (this.productForm.get('id')?.value == null) {
      this.productService.create(<any>formData)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    } else {
      formData.append('id', <any>this.product.id);
      this.productService.update(<any>this.product.id, <any>formData)
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
      this.productForm.controls['categoryId'].setValue(response.data?.id);
      this.categoryModal.hide();
    }
  }

}
