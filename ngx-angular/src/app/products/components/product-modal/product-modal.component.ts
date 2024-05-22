import {Component, EventEmitter, Injector, Input, OnInit, Output} from "@angular/core";
import {faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {DecimalPipe, NgClass} from "@angular/common";
import {initializeSelect2Injector, select2Category, toastError} from "app/common/interfaces";
import {Product, ProductDataModal} from "../../interfaces";
import {Company} from "app/account/interfaces";
import {CategoryService} from "../../services";

@Component({
  selector: "app-product-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    DecimalPipe
  ],
  templateUrl: "./product-modal.component.html"
})
export class ProductModalComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  @Input()
  company: Company = new Company();
  productForm: FormGroup = this.fb.group({
    id: [undefined],
    companyId: [""],
    description: ["", Validators.required],
    category: ["", Validators.required],
    barcode: ["-"],
    igvSunat: ["GRAVADO", Validators.required],
    precioVentaUnitario: [0, [Validators.required, Validators.min(0)]],
    type: ["BIEN", Validators.required],
    undMedida: ["NIU:UNIDAD (BIENES)", Validators.required],
  });
  // ====================================================================================================
  @Input()
  productDataModal: ProductDataModal = new ProductDataModal();
  @Output()
  responseData = new EventEmitter<ProductDataModal>();

  constructor(
    private fb: FormBuilder,
    private injector: Injector,
    private categoryService: CategoryService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    // cargar lista de categorías.
    const categoryEl = select2Category("#category", "#product-modal")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.productForm.controls["category"].setValue(`${data.id}:${data.text}`);
      });
    // cargar valores por defecto.
    if (document.querySelector("#product-modal")) {
      const myModal: any = document.querySelector("#product-modal");
      myModal.addEventListener("shown.bs.modal", () => {
        this.productForm.reset({...this.productDataModal.product});
        // cargar categoría de producto.
        if (this.productDataModal.type === "EDIT") {
          const {category} = this.productDataModal.product;
          this.categoryService.show(category.split(":")[0]?.trim())
            .subscribe(result => {
              const newOption = new Option(result.name, <any>result.id, true, true);
              categoryEl.append(newOption).trigger("change");
            });
        }
      });
      // limpiar formulario al cerrar modal.
      myModal.addEventListener("hidden.bs.modal", () => {
        this.productForm.reset(new Product());
        categoryEl.val(null).trigger("change");
      });
    }
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.productForm.controls[field].errors && this.productForm.controls[field].touched;
  }

  // guardar todos los cambios.
  public saveChanges(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.productDataModal.product = {
      ...this.productDataModal.product,
      ...this.productForm.value
    };
    this.responseData.emit(this.productDataModal);
  }

}
