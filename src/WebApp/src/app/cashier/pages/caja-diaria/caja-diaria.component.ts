import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars, faPlus, faTrashAlt, faSearch} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {InvoiceSerie} from 'src/app/system/interfaces';
import {InvoiceSerieService} from 'src/app/system/services';
import {CajaDiariaService, CashierDetailService} from '../../services';
import {CajaDiaria} from '../../interfaces';
import {accessDenied, deleteConfirm, errorAlBorrarCajaDiaria} from 'src/app/global/interfaces';
import {AuthService} from 'src/app/user/services';

declare var bootstrap: any;

@Component({
  selector: 'app-caja-diaria',
  templateUrl: './caja-diaria.component.html',
  styleUrls: ['./caja-diaria.component.scss']
})
export class CajaDiariaComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faBars = faBars;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  aperturaCajaModal: any;
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  cajasDiarias: Array<CajaDiaria> = new Array<CajaDiaria>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });
  // ====================================================================================================
  aperturaForm: FormGroup = this.fb.group({
    invoiceSerie: ['', [Validators.required]],
    total: [0, [Validators.required, Validators.min(0)]],
    turno: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private invoiceSerieService: InvoiceSerieService,
    private cajaDiariaService: CajaDiariaService,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    this.cargarCajasDiarias();
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector('#aperturaCaja'));
    // reset aperturaForm al iniciar el modal.
    if (document.querySelector('#aperturaCaja')) {
      const myModal: any = document.querySelector('#aperturaCaja');
      myModal.addEventListener('hide.bs.modal', () => {
        this.aperturaForm.reset({serieId: '', total: 0});
      });
    }
    // cargar series de facturación.
    this.invoiceSerieService.index().subscribe(result => this.invoiceSeries = result);
  }

  // cargar cajas diarias.
  public cargarCajasDiarias(): void {
    const year = this.queryForm.get('year')?.value;
    const month = this.queryForm.get('month')?.value;
    this.cajaDiariaService.index(year, month).subscribe(result => this.cajasDiarias = result);
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
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.cajaDiariaService.create(this.aperturaForm.value)
      .subscribe(result => {
        if (result.ok) {
          this.cargarCajasDiarias();
          this.aperturaCajaModal.hide();
        }
      });
  }

  // borrar registro caja diaria.
  public borrarCajaDiaria(id: string): void {
    this.authService.getMe().subscribe(async (result) => {
      if (result.role !== 'Admin') {
        await accessDenied();
      } else {
        deleteConfirm().then(result => {
          if (result.isConfirmed) {
            this.cashierDetailService.countDocuments(id)
              .subscribe(async (result) => {
                if (result > 0) {
                  await errorAlBorrarCajaDiaria(result);
                } else {
                  this.cajaDiariaService.delete(id).subscribe(result => {
                    if (result.ok) this.cargarCajasDiarias();
                  });
                }
              });
          }
        });
      }
    });
  }

}
