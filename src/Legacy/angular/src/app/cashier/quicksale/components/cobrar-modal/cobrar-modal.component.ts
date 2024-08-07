import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {faEnvelope, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {faCheckSquare} from "@fortawesome/free-regular-svg-icons";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {InvoiceSaleCashierService} from "../../../services";
import {FormaPagoComprobante} from "app/sales/interfaces";
import {ComprobanteDto, Cuota, PaymentTerms, ResponseCobrarModal} from "../../interfaces";
import {confirmTask} from "app/common/interfaces";
import {Company} from "app/account/interfaces";
import moment from "moment";
import _ from "lodash";
import {CurrencyPipe, NgClass, NgIf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-cobrar-modal",
  templateUrl: "./cobrar-modal.component.html",
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    NgClass,
    FaIconComponent,
    CurrencyPipe
  ],
  styleUrls: ["./cobrar-modal.component.scss"]
})
export class CobrarModalComponent implements OnInit {
  faCheckSquare = faCheckSquare;
  faTrashAlt = faTrashAlt;
  faEnvelope = faEnvelope;
  // ====================================================================================================
  @Input()
  company: Company = new Company();
  @Input()
  comprobanteDto: ComprobanteDto = new ComprobanteDto();
  @Output()
  responseData: EventEmitter<ResponseCobrarModal> = new EventEmitter<ResponseCobrarModal>();
  responseCobrarModal: ResponseCobrarModal = new ResponseCobrarModal();
  cobrarForm: FormGroup = this.fb.group({
    formaPago: ["Contado:Contado", [Validators.required]],
    tipoDoc: ["NOTA", [Validators.required]],
    montoRecibido: [null, [Validators.required, Validators.min(0)]],
    remark: [""]
  });
  loadingText: string = "En proceso...";
  showLoadingProgress: boolean = false;
  showFormaDePagoBoleta: boolean = true;
  showFormCobrar: boolean = true;
  // ====================================================================================================
  email: FormControl = this.fb.control("");

  constructor(
    private fb: FormBuilder,
    private invoiceSaleCashierService: InvoiceSaleCashierService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector("#cobrarModal");
    myModal.addEventListener("hide.bs.modal", () => {
      this.cobrarForm.reset({formaPago: "Contado:Contado", tipoDoc: "NOTA", montoRecibido: null, remark: ""});
      this.loadingText = "En proceso...";
      this.showLoadingProgress = false;
      this.showFormaDePagoBoleta = true;
      this.showFormCobrar = true;
    });
  }

  public vuelto(): number {
    const montoRecibido = this.cobrarForm.get("montoRecibido")?.value;
    return montoRecibido - this.comprobanteDto.getTotalCobrar();
  }

  public showOptionFactura(): boolean {
    return this.comprobanteDto.cabecera.tipDocUsuario === "6:R.U.C";
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.cobrarForm.controls[field].errors && this.cobrarForm.controls[field].touched;
  }

  public cobrarVenta(): void {
    if (this.cobrarForm.invalid) {
      this.cobrarForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.showLoadingProgress = true;
        this.loadingText = "En proceso...";
        // establecer valores del formulario.
        this.comprobanteDto.cabecera.tipoDoc = this.cobrarForm.get("tipoDoc")?.value;
        const formaPagoInput = this.cobrarForm.get("formaPago")?.value;
        const formaPago: PaymentTerms = new PaymentTerms();
        formaPago.tipo = formaPagoInput.split(":")[0].trim();
        formaPago.monto = this.comprobanteDto.getTotalCobrar();
        this.comprobanteDto.formaPago = formaPago;
        this.comprobanteDto.cabecera.paymentMethod = formaPagoInput;
        // comprobar si comprobante es factura y a crédito.
        if (this.comprobanteDto.cabecera.paymentMethod === FormaPagoComprobante.Credito) {
          if (this.comprobanteDto.cabecera.tipoDoc !== "03") {
            this.comprobanteDto.cabecera.fecVencimiento = moment().add(this.company.diasPlazo, "days").format("YYYY-MM-DD");
            const cuota: Cuota = new Cuota();
            cuota.fechaPago = this.comprobanteDto.cabecera.fecVencimiento;
            cuota.monto = this.comprobanteDto.getTotalCobrar();
            this.comprobanteDto.cuotas = _.concat(this.comprobanteDto.cuotas, cuota);
          }
        }
        this.comprobanteDto.cabecera.remark = this.cobrarForm.get("remark")?.value;
        // generar comprobante.
        this.invoiceSaleCashierService.generarVenta(this.comprobanteDto)
          .subscribe(result => {
            this.responseCobrarModal = result;
            if (this.comprobanteDto.cabecera.tipoDoc === "NOTA")
              this.loadingText = "La nota de venta ha sido registrado!";
            if (this.comprobanteDto.cabecera.tipoDoc !== "NOTA")
              this.loadingText = result.billingResponse.cdrDescription;
            this.showLoadingProgress = false;
            this.showFormCobrar = false;
          });
      }
    });
  }

  public changeFormaDePago({value}: any): void {
    this.showFormaDePagoBoleta = value !== FormaPagoComprobante.Credito;
  }

  public enviarCorreo(e: Event): void {
    e.preventDefault();
    console.log("Enviar Correo...");
  }

  public cerrar(): void {
    if (!this.showFormCobrar) {
      this.responseCobrarModal.imprimir = false;
      this.responseData.emit(this.responseCobrarModal);
    } else {
      this.responseCobrarModal.invoiceSaleId = "-";
      this.responseCobrarModal.imprimir = false;
      this.responseData.emit(this.responseCobrarModal);
    }
  }

  public print(): void {
    this.responseCobrarModal.imprimir = true;
    this.responseData.emit(this.responseCobrarModal);
  }

}
