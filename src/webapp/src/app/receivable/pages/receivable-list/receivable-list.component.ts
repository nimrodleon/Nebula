import {Component, OnInit} from "@angular/core";
import {faChartBar, faCoins, faEdit, faEye, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {faCalendar} from "@fortawesome/free-regular-svg-icons/faCalendar";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ReceivableService} from "../../services";
import {Receivable, ReceivableDataModal, ReceivableDetailDataModal, ReceivableRequestParams} from "../../interfaces";
import {
  PaginationResult,
  accessDenied, deleteConfirm, deleteError, toastSuccess,
} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import {ActivatedRoute, RouterLink} from "@angular/router";
import moment from "moment";
import _ from "lodash";
import {
  ReceivableContainerComponent
} from "app/common/containers/receivable-container/receivable-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, DatePipe, JsonPipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {CalcularDiasVencimientoPipe} from "../../../common/pipes/calcular-dias-vencimiento.pipe";
import {CargoModalComponent} from "../../components/cargo-modal/cargo-modal.component";
import {AbonoModalComponent} from "../../components/abono-modal/abono-modal.component";
import {AbonoDetailComponent} from "../../components/abono-detail/abono-detail.component";
import flatpickr from "flatpickr";
import {Spanish} from "flatpickr/dist/l10n/es";

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
    JsonPipe,
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
  faCalendar = faCalendar;
  // ====================================================================================================
  page: number = 1;
  cargoModal: any;
  abonoModal: any;
  abonoDetailModal: any;
  cuentasPorCobrar: PaginationResult<Receivable> = new PaginationResult<Receivable>();
  queryForm: FormGroup = this.fb.group({
    fromDate: [moment().subtract(30, "days").format("YYYY-MM-DD"), [Validators.required]],
    toDate: [moment().format("YYYY-MM-DD"), [Validators.required]],
    searchParam: [""],
    status: ["PENDIENTE", [Validators.required]]
  });
  cargoDetail: ReceivableDetailDataModal = new ReceivableDetailDataModal();
  cargoDataModal: ReceivableDataModal = new ReceivableDataModal();
  currentCargoId: string = "-";
  dateRange: any;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userDataService: UserDataService,
    private receivableService: ReceivableService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.page = params["page"];
      this.cargarCuentasPorCobrar();
    });
    this.cargoModal = new bootstrap.Modal("#cargo-modal");
    this.abonoModal = new bootstrap.Modal("#abono-modal");
    this.abonoDetailModal = new bootstrap.Modal("#abono-detail");
    this.dateRange = flatpickr("#dateRange", {
      mode: "range",
      locale: Spanish,
      dateFormat: "Y-m-d",
      defaultDate: [
        moment().subtract(30, "days").format("YYYY-MM-DD"),
        moment().format("YYYY-MM-DD"),
      ],
      onChange: (selectedDates, dateStr, instance) => {
        const fromDate = moment(selectedDates[0]).format("YYYY-MM-DD");
        const toDate = moment(selectedDates[1]).format("YYYY-MM-DD");
        this.queryForm.get("fromDate")?.setValue(fromDate);
        this.queryForm.get("toDate")?.setValue(toDate);
        this.cargarCuentasPorCobrar();
      }
    });
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  public cargarCuentasPorCobrar(): void {
    this.receivableService.index(this.queryForm.value, this.page)
      .subscribe(result => this.cuentasPorCobrar = result);
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
    const {receivable} = data;
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
