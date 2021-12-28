import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {Product} from 'src/app/products/interfaces';
import {ResponseData} from 'src/app/global/interfaces';
import {ProductService} from 'src/app/products/services';
import {CpeDetail} from '../../interfaces';

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
  currentProduct: Product | any;
  @Output()
  responseData = new EventEmitter<CpeDetail>();
  itemComprobanteForm: FormGroup = this.fb.group({
    productId: [null],
    description: [''],
    price: [0],
    quantity: [0],
    amount: [0]
  });
  productModal: any;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService) {
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
    }).on('select2:select', (e: any) => {
      const data = e.params.data;
      this.itemComprobanteForm.controls['productId'].setValue(data.id);
      this.productService.show(data.id).subscribe(result => {
        this.itemComprobanteForm.controls['description'].setValue(result.description);
        this.itemComprobanteForm.controls['price'].setValue(result.price1);
      });
    });
    // resetear formulario.
    const myModal: any = document.querySelector('#item-comprobante');
    myModal.addEventListener('hide.bs.modal', () => {
      this.itemComprobanteForm.reset();
      jQuery('#productId').val(null).trigger('change');
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

  // calcular importe producto.
  public calcAmount(): void {
    const price = Number(this.itemComprobanteForm.get('price')?.value);
    const quantity = Number(this.itemComprobanteForm.get('quantity')?.value);
    this.itemComprobanteForm.controls['amount'].setValue(price * quantity);
  }

  // botón agregar producto.
  public saveChanges(): void {
    this.responseData.emit(this.itemComprobanteForm.value);
  }

}
