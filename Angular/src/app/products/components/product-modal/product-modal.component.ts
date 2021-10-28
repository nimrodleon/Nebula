import {Component, Input, OnInit} from '@angular/core';
import {faBars, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ProductService, UndMedidaService} from '../../services';
import {Product, UndMedida} from '../../interfaces';

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

  @Input()
  product: Product = new Product();

  @Input()
  title: string = '';

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private undMedidaService: UndMedidaService) {
  }

  ngOnInit(): void {
    // cargar las unidades de medidas.
    this.undMedidaService.index().subscribe(result => this.undMedidas = result);
    // suscribir formGroup.
    this.productForm.valueChanges.subscribe(value => this.product = value);
  }

  // capturar imagen del formulario.
  public uploadFile(event: any): void {
    this.fileImage = event.target.files[0];
    console.log(event.target.files);
    console.log(this.fileImage);
  }

  // guardar todos los cambios.
  public saveChanges(): void {
    const formData = new FormData();
    formData.append('description', 'AAA');
    formData.append('barcode', '-');
    formData.append('icbper', 'NO');
    formData.append('price', '0');
    formData.append('igvSunat', 'GRAVADO');
    formData.append('type', 'BIEN');
    formData.append('undMedidaId', '05b91f73-5e8e-4698-b5d0-811c204c08a7');
    if (this.fileImage) formData.append('file', this.fileImage);
    if (this.productForm.get('id')?.value == null) {
      this.productService.store(formData).subscribe(result => {
        console.log(result);
      });
    } else {

    }
  }

}
