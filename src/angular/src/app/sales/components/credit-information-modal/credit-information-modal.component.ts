import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {faCircleChevronRight, faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CreditInformationDto, CuotaDataModal, CuotaPagoDto} from '../../interfaces';
import {v4 as uuid} from 'uuid';
import moment from "moment";
import _ from "lodash";
import {CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CuotaModalComponent} from "../cuota-modal/cuota-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-credit-information-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    DatePipe,
    NgForOf,
    FaIconComponent,
    CurrencyPipe,
    CuotaModalComponent,
    NgIf
  ],
  templateUrl: "./credit-information-modal.component.html"
})
export class CreditInformationModalComponent implements OnInit {
  faCircleChevronRight = faCircleChevronRight;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  cuotaModal: any;
  @Input()
  pagoPendiente: number = 0;
  sumatoriaCuotas: number = 0;
  cuotasPagoDto = new Array<CuotaPagoDto>();
  cuotaDataModal = new CuotaDataModal();
  creditForm: FormGroup = this.fb.group({
    mtoNetoPendientePago: [0, [Validators.required, Validators.min(1)]],
    fecVencimiento: [moment().format('YYYY-MM-DD'), [Validators.required]],
  });
  @Output()
  responseData = new EventEmitter<CreditInformationDto>();
  showAlert: boolean = false;
  messageAlert: string = '-';

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.cuotaModal = new bootstrap.Modal('#cuota-modal');
    const myModalEl: any = document.querySelector('#credit-information-modal');
    myModalEl.addEventListener('show.bs.modal', () => {
      this.creditForm.controls['mtoNetoPendientePago'].setValue(this.pagoPendiente);
    });
  }

  public agregarCuota(): void {
    this.cuotaDataModal.type = 'ADD';
    this.cuotaDataModal.cuotaPagoDto = new CuotaPagoDto();
    this.cuotaModal.show();
  }

  public editarCuota(item: CuotaPagoDto): void {
    this.cuotaDataModal.type = 'EDIT';
    this.cuotaDataModal.cuotaPagoDto = item;
    this.cuotaModal.show();
  }

  public inputIsInvalid(field: string) {
    return this.creditForm.controls[field].errors && this.creditForm.controls[field].touched;
  }

  public closeAlert(): void {
    this.messageAlert = '-';
    this.showAlert = false;
  }

  public saveChanges(): void {
    if (this.creditForm.invalid) {
      this.creditForm.markAllAsTouched();
      return;
    }
    if (this.cuotasPagoDto.length <= 0) {
      this.messageAlert = 'Debe existir al menos una Cuota!';
      this.showAlert = true;
      return;
    }
    const totalCuotasPago = _.sumBy(this.cuotasPagoDto, item => item.mtoCuotaPago);
    if (this.pagoPendiente !== totalCuotasPago) {
      this.messageAlert = 'Las cuotas debe ser igual que pago pendiente!';
      this.showAlert = true;
      return;
    }
    const fecVencimiento = this.creditForm.get('fecVencimiento')?.value;
    if (!CreditInformationModalComponent.validarFechaVencimiento(fecVencimiento)) {
      this.messageAlert = 'La fecha de vencimiento no puede ser anterior o igual que la fecha de emisión del comprobante';
      this.showAlert = true;
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const creditInformationDto = new CreditInformationDto();
    creditInformationDto.fecVencimiento = this.creditForm.get('fecVencimiento')?.value;
    creditInformationDto.datoPagoDto.mtoNetoPendientePago = this.creditForm.get('mtoNetoPendientePago')?.value;
    creditInformationDto.cuotasPagoDto = this.cuotasPagoDto;
    this.responseData.emit(creditInformationDto);
  }

  private static validarFechaVencimiento(fecVencimiento: string): boolean {
    const date = new Date();
    const fechaActual = moment([date.getFullYear(), date.getMonth(), date.getDate()]);
    return moment(fecVencimiento).diff(fechaActual, 'days') > 0;
  }

  public saveChangesDetail(data: CuotaDataModal): void {
    const {cuotaPagoDto} = data;
    if (!CreditInformationModalComponent.validarFechaVencimiento(cuotaPagoDto?.fecCuotaPago)) {
      this.messageAlert = 'Fecha del pago único o de las cuotas no puede ser anterior o igual que la fecha de emisión del comprobante';
      this.showAlert = true;
      this.cuotaModal.hide();
    } else {
      if (data.type === 'ADD') {
        cuotaPagoDto.uuid = uuid();
        this.cuotasPagoDto = _.concat(this.cuotasPagoDto, cuotaPagoDto);
        this.cuotaModal.hide();
        this.calcularSumatoriaCuotas();
      }
      if (data.type === 'EDIT') {
        this.cuotasPagoDto = _.map(this.cuotasPagoDto, (o: CuotaPagoDto) => {
          if (o.uuid === cuotaPagoDto?.uuid) o = cuotaPagoDto;
          return o;
        });
        this.cuotaModal.hide();
        this.calcularSumatoriaCuotas();
      }
    }
  }

  private calcularSumatoriaCuotas(): void {
    this.sumatoriaCuotas = _.sumBy(this.cuotasPagoDto, item => item.mtoCuotaPago);
  }

  public deleteCuota(uuid: string): void {
    this.cuotasPagoDto = _.filter(this.cuotasPagoDto, (o: CuotaPagoDto) => o.uuid !== uuid);
  }

}
