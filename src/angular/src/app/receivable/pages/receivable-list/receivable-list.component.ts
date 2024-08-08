import { Component, Injector, OnInit } from "@angular/core";
import { faChartBar, faCoins, faEdit, faEye, faPlus, faSearch, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { FormBuilder, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ReceivableService } from "../../services";
import { Receivable, ReceivableDataModal, ReceivableDetailDataModal, ReceivableRequestParams } from "../../interfaces";
import {
  PaginationResult,
  accessDenied, deleteConfirm, deleteError, initializeSelect2Injector, toastSuccess,
} from "app/common/interfaces";
import { UserDataService } from "app/common/user-data.service";
import { ActivatedRoute, RouterLink } from "@angular/router";
import moment from "moment";
import _ from "lodash";
import {
  ReceivableContainerComponent
} from "app/common/containers/receivable-container/receivable-container.component";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf } from "@angular/common";
import { CalcularDiasVencimientoPipe } from "../../../common/pipes/calcular-dias-vencimiento.pipe";
import { CargoModalComponent } from "../../components/cargo-modal/cargo-modal.component";
import { AbonoModalComponent } from "../../components/abono-modal/abono-modal.component";
import { AbonoDetailComponent } from "../../components/abono-detail/abono-detail.component";

declare const bootstrap: any;

@Component({
  selector: "app-receivable-list",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ReceivableContainerComponent,
    FaIconComponent,
    RouterLink,
    NgIf,
    CurrencyPipe,
    CalcularDiasVencimientoPipe,
    CargoModalComponent,
    AbonoModalComponent,
    AbonoDetailComponent,
    DatePipe,
    NgForOf,
    NgClass,
  ],
  templateUrl: "./receivable-list.component.html"
})
export class ReceivableListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faCoins = faCoins;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faEye = faEye;
  faChartBar = faChartBar;
  // ====================================================================================================
  companyId: string = "";
  cargoModal: any;
  abonoModal: any;
  abonoDetailModal: any;
  cuentasPorCobrar: PaginationResult<Receivable> = new PaginationResult<Receivable>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
    status: "PENDIENTE"
  });
  cargoDetail: ReceivableDetailDataModal = new ReceivableDetailDataModal();
  cargoDataModal: ReceivableDataModal = new ReceivableDataModal();
  currentCargoId: string = "-";

  constructor(
    private fb: FormBuilder,
    private injector: Injector,
    private route: ActivatedRoute,
    private userDataService: UserDataService,
    private receivableService: ReceivableService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.route.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarCuentasPorCobrar(page);
    });
    this.cargoModal = new bootstrap.Modal("#cargo-modal");
    this.abonoModal = new bootstrap.Modal("#abono-modal");
    this.abonoDetailModal = new bootstrap.Modal("#abono-detail");
  }

  public get companyName(): string {
    return this.userDataService.companyName;
  }

  public cargarCuentasPorCobrar(page: number = 1): void {
    const requestParam = new ReceivableRequestParams();
    requestParam.year = String(this.queryForm.get("year")?.value);
    requestParam.month = this.queryForm.get("month")?.value;
    requestParam.status = this.queryForm.get("status")?.value;
    this.receivableService.index(requestParam, page).subscribe(result => this.cuentasPorCobrar = result);
  }

  public agregarCargo(): void {
    this.cargoDataModal = new ReceivableDataModal("Agregar Cargo", "ADD", new Receivable());
    this.cargoModal.show();
  }

  public editarCargo(cargo: Receivable): void {
    if (cargo.type === "CARGO") {
      this.receivableService.show(cargo.id).subscribe(result => {
        this.cargoDataModal = new ReceivableDataModal("Editar Cargo", "EDIT", result);
        this.cargoModal.show();
      });
    }
  }

  public agregarAbono(cargo: Receivable): void {
    if (cargo.type === "CARGO") {
      this.currentCargoId = cargo.id;
      this.abonoModal.show();
    }
  }

  public hideAbonoModal(data: Receivable): void {
    this.cargarCuentasPorCobrar();
    this.abonoModal.hide();
    toastSuccess("El abono ha sido registrado!");
  }

  public showAbonoDetail(cargo: Receivable): void {
    if (cargo.type === "CARGO") {
      this.cargoDetail.cargo = cargo;
      this.receivableService.abonos(cargo.id).subscribe(result => {
        this.cargoDetail.abonos = result;
        this.abonoDetailModal.show();
      });
    }
  }

  public responseAbonoDetail(status: boolean): void {
    if (status) this.cargarCuentasPorCobrar();
  }

  public saveChangesCargo(data: ReceivableDataModal): void {
    const { receivable } = data;
    if (data.type === "ADD") {
      this.receivableService.create(receivable)
        .subscribe(result => {
          this.cargarCuentasPorCobrar();
          this.cargoModal.hide();
          toastSuccess("El cargo ha sido registrado!");
        });
    }
    if (data.type === "EDIT") {
      this.receivableService.update(receivable?.id, receivable)
        .subscribe(result => {
          this.cargarCuentasPorCobrar();
          this.cargoModal.hide();
          toastSuccess("El cargo ha sido actualizado!");
        });
    }
  }

  public deleteCargo(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.receivableService.totalAbonos(id)
            .subscribe(async (totalAbonos) => {
              if (totalAbonos > 0) {
                await deleteError();
              } else {
                this.receivableService.delete(id)
                  .subscribe(result => {
                    this.cargarCuentasPorCobrar();
                  });
              }
            });
        }
      });
    }
  }

  public get totalCargo(): number {
    return _.sumBy(this.cuentasPorCobrar.data, item => item.cargo);
  }

  public get totalSaldo(): number {
    return _.sumBy(this.cuentasPorCobrar.data, item => item.saldo);
  }

}
