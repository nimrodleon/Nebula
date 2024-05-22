import { Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { FormBuilder, FormControl, ReactiveFormsModule } from "@angular/forms";
import {
  faChartBar, faCoins, faFilter, faFolderTree, faLock, faPlus, faSearch, faTrashAlt, faWallet
} from "@fortawesome/free-solid-svg-icons";
import { PaginationResult, accessDenied, deleteConfirm, initializeSelect2Injector, } from "app/common/interfaces";
import { InvoiceSaleService } from "app/sales/services";
import { CajaDiaria, CashierDetail } from "../../interfaces";
import { CajaDiariaService, CashierDetailService } from "../../services";
import { UserDataService } from "app/common/user-data.service";
import Swal from "sweetalert2";
import _ from "lodash";
import { CashierContainerComponent } from "app/common/containers/cashier-container/cashier-container.component";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf } from "@angular/common";
import { CerrarCajaComponent } from "../../components/cerrar-caja/cerrar-caja.component";
import { CajaChicaModalComponent } from "../../components/caja-chica-modal/caja-chica-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-cash-detail",
  standalone: true,
  imports: [
    CashierContainerComponent,
    FaIconComponent,
    ReactiveFormsModule,
    RouterLink,
    CurrencyPipe,
    NgIf,
    NgClass,
    CerrarCajaComponent,
    CajaChicaModalComponent,
    DatePipe,
    NgForOf
  ],
  templateUrl: "./cash-detail.component.html"
})
export class CashDetailComponent implements OnInit {
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faLock = faLock;
  faSearch = faSearch;
  faFilter = faFilter;
  faChartBar = faChartBar;
  faFolderTree = faFolderTree;
  faWallet = faWallet;
  faCoins = faCoins;
  // ====================================================================================================
  companyId: string = "";
  cajaDiaria: CajaDiaria = new CajaDiaria();
  query: FormControl = this.fb.control("");
  cashierDetails: PaginationResult<CashierDetail> = new PaginationResult<CashierDetail>();
  cerrarCajaModal: any;
  cajaChicaModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private cajaDiariaService: CajaDiariaService,
    private invoiceSaleService: InvoiceSaleService,
    private cashierDetailService: CashierDetailService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      const id: string = params.get("id") || "";
      this.cajaDiariaService.show(id)
        .subscribe(result => {
          this.cajaDiaria = result;
          this.activatedRoute.queryParams.subscribe(params => {
            const page = params["page"];
            this.cargarDetallesDeCaja(page);
          });
        });
    });
    this.cerrarCajaModal = new bootstrap.Modal("#cerrar-caja");
    this.cajaChicaModal = new bootstrap.Modal("#caja-chica-modal");
  }

  // cargar detalle de caja.
  private cargarDetallesDeCaja(page: number = 1): void {
    this.cashierDetailService.index(this.cajaDiaria.id, this.query.value, page)
      .subscribe(result => this.cashierDetails = result);
  }

  public isCajaCerrada(): boolean {
    return this.cajaDiaria.status === "CERRADO";
  }

  public async openQuickSale() {
    if (this.cajaDiaria.status == "ABIERTO") {
      await this.router.navigate([
        "/", this.companyId, "cashier", "quicksale", this.cajaDiaria.id
      ]);
    } else {
      await Swal.fire(
        "Info?",
        "La caja esta cerrada!",
        "info"
      );
    }
  }

  // buscar documentos.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarDetallesDeCaja();
  }

  // botón cerrar caja.
  public async showCerrarCajaModal() {
    if (this.cajaDiaria.status === "ABIERTO") {
      this.cerrarCajaModal.show();
    } else {
      await Swal.fire(
        "Info?",
        "La caja esta cerrada!",
        "info"
      );
    }
  }

  // cerrar modal cierre de caja.
  public async hideCerrarCajaModal(result: CajaDiaria) {
    this.cerrarCajaModal.hide();
    await this.router.navigate(["/", this.companyId, "cashier"]);
  }

  // borrar detalle de caja.
  public borrarDetalleCaja(data: CashierDetail): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          if (data.typeOperation !== "COMPROBANTE_DE_VENTA") {
            this.cashierDetailService.delete(data.id).subscribe(result => {
              this.cargarDetallesDeCaja();
            });
          } else {
            this.cashierDetailService.delete(data.id).subscribe(result => {
              this.invoiceSaleService.delete(data.invoiceSaleId).subscribe(result => {
                this.cargarDetallesDeCaja();
              });
            });
          }
        }
      });
    }
  }

  public async reporteCaja(event: Event) {
    event.preventDefault();
    this.cashierDetailService.getResumenCajaDto(this.cajaDiaria.id)
      .subscribe(async (result) => {
        await Swal.fire({
          title: "<strong class=\"text-primary text-uppercase\"><u>Reporte</u></strong>",
          html: `
            <table class="table table-striped mb-0">
            <thead>
            <tr class="text-uppercase">
              <th>Concepto</th>
              <th>Total</th>
            </tr>
            </thead>
            <tbody class="text-uppercase">
            <tr>
              <td>Yape</td>
              <td>${result.yape.toFixed(2)}</td>
            </tr>
            <tr>
              <td>Crédito</td>
              <td>${result.credito.toFixed(2)}</td>
            </tr>
            <tr>
              <td>Contado</td>
              <td>${result.contado.toFixed(2)}</td>
            </tr>
            <tr>
              <td>Depósito</td>
              <td>${result.deposito.toFixed(2)}</td>
            </tr>
            <tr class="table-danger">
              <td>Salida</td>
              <td>${result.salida.toFixed(2)}</td>
            </tr>
            <tr class="table-dark">
              <td>Monto Total</td>
              <td>${result.montoTotal.toFixed(2)}</td>
            </tr>
            </tbody>
            </table>
            `,
          showCloseButton: true,
          showCancelButton: false,
          showConfirmButton: false
        });
      });
  }

  public abrirModalCajaChica(): void {
    this.cajaChicaModal.show();
  }

  public guardarModalCajaChica(item: CashierDetail): void {
    this.cashierDetails.data = _.concat(item, this.cashierDetails.data);
    this.cajaChicaModal.hide();
  }

}
