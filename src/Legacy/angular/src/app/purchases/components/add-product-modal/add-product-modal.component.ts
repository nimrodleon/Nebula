import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {faBox, faCalculator, faTag, faWarehouse} from "@fortawesome/free-solid-svg-icons";
import {ItemCompraForm} from "../../interfaces/purchase-invoice";
import {UserDataService} from "app/common/user-data.service";
import {EnumIdModal, FormType, select2Productos, toastError} from "app/common/interfaces";
import {Company} from "../../../account/interfaces";
import {NgClass} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-add-product-modal",
  templateUrl: "./add-product-modal.component.html",
  imports: [
    ReactiveFormsModule,
    NgClass,
    FaIconComponent
  ],
  standalone: true
})
export class AddProductModalComponent implements OnInit {
  faBox = faBox;
  faWarehouse = faWarehouse;
  faCalculator = faCalculator;
  faTag = faTag;
  @Input()
  itemCompra = new ItemCompraForm();
  itemCompraForm: FormGroup = this.fb.group({
    id: ["-", [Validators.required]],
    productId: ["", [Validators.required]],
    typeOper: ["ADD", [Validators.required]],
    tipoItem: ["BIEN", [Validators.required]],
    ctdUnidadItem: [0, [Validators.required, Validators.min(1)]],
    mtoPrecioCompraUnitario: [0.0, [Validators.required]],
    codUnidadMedida: ["NIU:UNIDAD (BIENES)", [Validators.required]],
    desItem: ["", [Validators.required]],
    triIcbper: [false, [Validators.required]],
    mtoValorCompraItem: [0.0, [Validators.required, Validators.min(0)]],
    igvSunat: ["GRAVADO", [Validators.required]],
    mtoIgvItem: [0.0, [Validators.required]],
    mtoTriIcbperItem: [0.0, [Validators.required]],
    mtoTotalItem: [0.0, [Validators.required, Validators.min(0)]],
  });
  @Output()
  responseData = new EventEmitter<ItemCompraForm>();

  constructor(
    private fb: FormBuilder,
    private globalService: UserDataService,) {
  }

  ngOnInit(): void {
    const select2 = select2Productos("#producto",
      EnumIdModal.ID_ADD_PRODUCT_MODAL_PURCHASES).off("select2:select");
    select2.on("select2:select", (e: any) => {
      const data = e.params.data;
      console.log(data);
      this.itemCompraForm.controls["productId"].setValue(data.id);
      this.itemCompraForm.controls["tipoItem"].setValue(data.type);
      this.itemCompraForm.controls["ctdUnidadItem"].setValue(1);
      this.itemCompraForm.controls["codUnidadMedida"].setValue(data.undMedida);
      this.itemCompraForm.controls["desItem"].setValue(data.description);
      this.itemCompraForm.controls["triIcbper"].setValue(data.icbper === "SI");
      if (data.icbper === "SI") this.itemCompraForm.controls["mtoTriIcbperItem"].setValue(this.configuration.valorImpuestoBolsa);
      if (data.icbper === "NO") this.itemCompraForm.controls["mtoTriIcbperItem"].setValue(0.0);
      this.itemCompraForm.controls["igvSunat"].setValue(data.igvSunat);
    });
    const myModal: any = document.querySelector(EnumIdModal.ID_ADD_PRODUCT_MODAL_PURCHASES);
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.itemCompra.typeOper === FormType.EDIT) {
        this.itemCompraForm.reset(this.itemCompra);
        const newOption = new Option(this.itemCompra.desItem, this.itemCompra.productId, true, true);
        select2.append(newOption).trigger("change");
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.itemCompraForm.reset(new ItemCompraForm());
      select2.val(null).trigger("change");
    });
  }

  /**
   * Información de configuración.
   * TODO: refactoring.
   */
  public get configuration(): Company {
    return new Company();
  }

  /**
   * Retorna el porcentaje del IGV, según el tipo de operación.
   * @param igvSunat tipo operación: GRAVADO|EXONERADO|INAFECTO
   * @private
   */
  private getPorcentajeIGV(igvSunat: string): number {
    return igvSunat === "GRAVADO" ? (this.configuration.porcentajeIgv / 100) + 1 : 1;
  }

  /**
   * Establece la cantidad de productos por Item.
   * @param value Cantidad de productos.
   */
  public cambiarCantidad({value}: any): void {
    this.calcularMtoTriIcbperItem(value);
    const igvSunat = this.itemCompraForm.get("igvSunat")?.value;
    const mtoTotalItem = Number(this.itemCompraForm.get("mtoTotalItem")?.value);
    this.calcularMtoValorCompraItem(igvSunat, mtoTotalItem);
    this.calcularMtoPrecioCompraUnitario(value, mtoTotalItem);
    this.itemCompraForm.controls["ctdUnidadItem"].setValue(Number(value));
  }

  /**
   * Calcula el Total ICBPER.
   * @param value cantidad de bolsas plásticas
   * @private
   */
  private calcularMtoTriIcbperItem(value: number): void {
    if (this.itemCompraForm.get("triIcbper")?.value === true)
      this.itemCompraForm.controls["mtoTriIcbperItem"].setValue(Number((value * this.configuration.valorImpuestoBolsa).toFixed(4)));
    if (this.itemCompraForm.get("triIcbper")?.value === false)
      this.itemCompraForm.controls["mtoTriIcbperItem"].setValue(0.0);
  }

  /**
   * Calcula el Valor de compra del Item.
   * @param igvSunat tipo operación.
   * @param mtoTotalItem monto total del item.
   * @private
   * @return {Number} Valor unitario por Item.
   */
  private calcularMtoValorCompraItem(igvSunat: string, mtoTotalItem: number): number {
    const porcentajeIGV = this.getPorcentajeIGV(igvSunat);
    const mtoValorCompraItem = Number((mtoTotalItem / porcentajeIGV).toFixed(4));
    this.itemCompraForm.controls["mtoValorCompraItem"].setValue(mtoValorCompraItem);
    // Calcular monto IGV del Item y valor de venta.
    const mtoIgvItem = Number((mtoTotalItem - mtoValorCompraItem).toFixed(4));
    this.itemCompraForm.controls["mtoIgvItem"].setValue(mtoIgvItem);
    return mtoValorCompraItem;
  }

  /**
   * Establece el precio unitario del Item.
   * @param ctdUnidadItem cantidad de unidades por item.
   * @param mtoTotalItem monto total del item.
   * @private
   */
  private calcularMtoPrecioCompraUnitario(ctdUnidadItem: number, mtoTotalItem: number): void {
    this.itemCompraForm.controls["mtoPrecioCompraUnitario"].setValue(mtoTotalItem / ctdUnidadItem);
  }

  /**
   * Establece el monto total del Item.
   * @param value monto total del item.
   */
  public cambiarMtoTotalItem({value}: any): void {
    const ctdUnidadItem = Number(this.itemCompraForm.get("ctdUnidadItem")?.value);
    this.calcularMtoTriIcbperItem(ctdUnidadItem);
    const igvSunat = this.itemCompraForm.get("igvSunat")?.value;
    this.calcularMtoValorCompraItem(igvSunat, value);
    this.calcularMtoPrecioCompraUnitario(ctdUnidadItem, value);
    this.itemCompraForm.controls["mtoTotalItem"].setValue(Number(value));
  }

  /**
   * Valida las entradas del formulario.
   * @param field entrada del formulario.
   */
  public inputIsInvalid(field: string) {
    return this.itemCompraForm.controls[field].errors && this.itemCompraForm.controls[field].touched;
  }

  /**
   * Envía los datos del formulario al componente padre.
   */
  public saveChanges(): void {
    if (this.itemCompraForm.invalid) {
      this.itemCompraForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.responseData.emit({...this.itemCompraForm.value});
  }

}
