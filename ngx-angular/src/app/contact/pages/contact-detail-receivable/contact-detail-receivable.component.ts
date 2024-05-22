import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faCloudArrowDown, faSearch} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {CuentaPorCobrarClienteAnualParam, ReceivableRequestParams} from "app/receivable/interfaces";
import {ContactDetailService} from "../../services";
import _ from "lodash";
import moment from "moment/moment";
import {
  ContactDetailContainerComponent
} from "../../components/contact-detail-container/contact-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {CalcularDiasVencimientoPipe} from "app/common/pipes/calcular-dias-vencimiento.pipe";

@Component({
  selector: "app-contact-detail-receivable",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ContactDetailContainerComponent,
    FaIconComponent,
    NgIf,
    DatePipe,
    NgForOf,
    RouterLink,
    CurrencyPipe,
    CalcularDiasVencimientoPipe
  ],
  templateUrl: "./contact-detail-receivable.component.html"
})
export class ContactDetailReceivableComponent implements OnInit {
  companyId: string = "";
  faSearch = faSearch;
  faCloudArrowDown = faCloudArrowDown;
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
    status: "PENDIENTE"
  });

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private contactDetailService: ContactDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }

  public get contact() {
    return this.contactDetailService.contact;
  }

  public get cuentasPorCobrar() {
    return this.contactDetailService.cuentasPorCobrar;
  }

  public getCuentasPorCobrar(): void {
    const requestParam = new CuentaPorCobrarClienteAnualParam();
    requestParam.year = this.queryForm.get("year")?.value;
    requestParam.status = this.queryForm.get("status")?.value;
    this.contactDetailService.getCuentasPorCobrar(requestParam);
  }

  public get totalCargos(): number {
    return _.sumBy(this.cuentasPorCobrar, item => item.cargo);
  }

  public get totalSaldo(): number {
    return _.sumBy(this.cuentasPorCobrar, item => item.saldo);
  }

  // public exportarDeuda(): void {
  //   confirmTask().then(result => {
  //     if (result.isConfirmed) {
  //       const year = this.queryForm.get("year")?.value;
  //       window.open(`${this.appURL}Receivable/ExportDeudaExcel/${this.contact.id}?Year=${year}`, "_blank");
  //     }
  //   });
  // }


}
