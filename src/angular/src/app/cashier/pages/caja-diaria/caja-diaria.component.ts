import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { faPlus, faTrashAlt, faSearch } from "@fortawesome/free-solid-svg-icons";
import { PaginationResult, accessDenied, deleteConfirm, errorAlBorrarCajaDiaria, toastError, toastSuccess } from "app/common/interfaces";
import { InvoiceSerie } from "app/account/company/interfaces";
import { CajaDiariaService, CashierDetailService } from "../../services";
import { InvoiceSerieService } from "app/account/company/services";
import { UserDataService } from "app/common/user-data.service";
import { CajaDiaria } from "../../interfaces";
import moment from "moment";
import { CashierContainerComponent } from "app/common/containers/cashier-container/cashier-container.component";
import { CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf } from "@angular/common";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";

declare const bootstrap: any;

@Component({
  selector: "app-caja-diaria",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CashierContainerComponent,
    DatePipe,
    NgForOf,
    RouterLink,
    CurrencyPipe,
    NgClass,
    FaIconComponent,
    NgIf
  ],
  templateUrl: "./caja-diaria.component.html"
})
export class CajaDiariaComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  companyId: string = "";
  aperturaCajaModal: any;
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  cajasDiarias: PaginationResult<CajaDiaria> = new PaginationResult<CajaDiaria>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });
  // ====================================================================================================
  aperturaForm: FormGroup = this.fb.group({
    invoiceSerie: ["", [Validators.required]],
    total: [0, [Validators.required, Validators.min(0)]],
    turno: ["", [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userDataService: UserDataService,
    private invoiceSerieService: InvoiceSerieService,
    private cajaDiariaService: CajaDiariaService,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.route.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarCajasDiarias(page);
    });
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector("#aperturaCaja"));
    // reset aperturaForm al iniciar el modal.
    if (document.querySelector("#aperturaCaja")) {
      const myModal: any = document.querySelector("#aperturaCaja");
      myModal.addEventListener("hide.bs.modal", () => {
        this.aperturaForm.reset({ serieId: "", total: 0 });
      });
    }
    // cargar series de facturación.
    this.invoiceSerieService.index().subscribe(result => this.invoiceSeries = result);
  }

  public get companyName(): string {
    return this.userDataService.companyName;
  }

  // cargar cajas diarias.
  public cargarCajasDiarias(page: number = 1): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    this.cajaDiariaService.index(year, month, page)
      .subscribe(result => this.cajasDiarias = result);
  }

  // botón apertura caja.
  public aperturaCajaClick(): void {
    this.aperturaCajaModal.show();
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.aperturaForm.controls[field].errors && this.aperturaForm.controls[field].touched;
  }

  // guardar apertura de caja.
  public guardarAperturaCaja(): void {
    if (this.aperturaForm.invalid) {
      this.aperturaForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.cajaDiariaService.create(this.aperturaForm.value)
      .subscribe(result => {
        this.cargarCajasDiarias();
        this.aperturaCajaModal.hide();
        toastSuccess("Se ha registrado la apertura de una nueva caja!");
      });
  }

  // borrar registro caja diaria.
  public borrarCajaDiaria(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.cashierDetailService.countDocuments(id)
            .subscribe(async (result) => {
              if (result > 0) {
                await errorAlBorrarCajaDiaria(result);
              } else {
                this.cajaDiariaService.delete(id).subscribe(result => {
                  this.cargarCajasDiarias();
                });
              }
            });
        }
      });
    }
  }

}
