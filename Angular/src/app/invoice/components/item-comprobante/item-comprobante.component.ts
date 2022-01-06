import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {ConfigurationService} from 'src/app/system/services';
import {Configuration} from 'src/app/system/interfaces';
import {Product} from 'src/app/products/interfaces';
import {ResponseData} from 'src/app/global/interfaces';
import {ProductService} from 'src/app/products/services';
import {CpeBase, CpeDetail} from '../../interfaces';

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
  private configuration: Configuration = new Configuration();
  currentProduct: Product = new Product();
  @Input()
  productId: number = 0;
  @Output()
  responseData = new EventEmitter<CpeDetail>();
  detalleVenta: CpeDetail = new CpeDetail();
  itemComprobanteForm: FormGroup = this.fb.group({
    productId: [null],
    price: [0],
    quantity: [0],
    amount: [0]
  });
  productModal: any;

  constructor(
    private fb: FormBuilder,
    private configurationService: ConfigurationService,
    private productService: ProductService) {
  }

  ngOnInit(): void {
    this.configurationService.show()
      .subscribe(result => this.configuration = result);
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
      this.cargarProducto(data.id);
    });
    // resetear formulario.
    const myModal: any = document.querySelector('#item-comprobante');
    myModal.addEventListener('shown.bs.modal', () => {
      this.detalleVenta = new CpeDetail();
      if (this.productId > 0) this.cargarProducto(this.productId);
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.itemComprobanteForm.reset();
      jQuery('#productId').val(null).trigger('change');
    });
  }

  // cargar producto al modelo y formulario.
  private cargarProducto(id: number): void {
    this.productService.show(id).subscribe(result => {
      this.detalleVenta = CpeBase.configItemDetail(this.configuration, result);
      this.detalleVenta.calcularItem();
      this.itemComprobanteForm.reset({...this.detalleVenta});
      // validar modo de edición.
      if (this.productId > 0) {
        const newOption = new Option(result.description, <any>result.id, true, true);
        jQuery('#productId').append(newOption).trigger('change');
      }
    });
  }

  // mostrar modal producto.
  public showProductModal(e: any): void {
    e.preventDefault();
    this.currentProduct = new Product();
    this.productModal.show();
  }

  // ocultar modal producto.
  public hideProductModal(response: ResponseData<Product>): void {
    if (response.ok) {
      this.productService.show(<any>response.data?.id)
        .subscribe(result => {
          const newOption = new Option(result.description, <any>result.id, true, true);
          jQuery('#productId').append(newOption).trigger('change');
          this.detalleVenta = CpeBase.configItemDetail(this.configuration, result);
          this.detalleVenta.calcularItem();
          this.itemComprobanteForm.reset({...this.detalleVenta});
          this.productModal.hide();
        });
    }
  }

  // calcular importe producto.
  public calcAmount(): void {
    const price = Number(this.itemComprobanteForm.get('price')?.value);
    const quantity = Number(this.itemComprobanteForm.get('quantity')?.value);
    this.detalleVenta.price = price;
    this.detalleVenta.quantity = quantity;
    this.detalleVenta.calcularItem();
    this.itemComprobanteForm.reset({...this.detalleVenta});
  }

  // botón agregar producto.
  public saveChanges(): void {
    this.responseData.emit(this.detalleVenta);
  }

}
