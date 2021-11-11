import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faIdCardAlt, faPlus, faSave, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {Caja} from 'src/app/cashier/interfaces';
import {CajaService} from 'src/app/cashier/services';
import {SunatService} from '../../services';
import {TypeOperationSunat} from '../../interfaces';

declare var jQuery: any;

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  faSave = faSave;
  faPlus = faPlus;
  faArrowLeft = faArrowLeft;
  faIdCardAlt = faIdCardAlt;
  private appURL: string = environment.applicationUrl;
  listaDeCajas: Array<Caja> = new Array<Caja>();
  typeOperation: Array<TypeOperationSunat> = new Array<TypeOperationSunat>();

  constructor(
    private cajaService: CajaService,
    private sunatService: SunatService) {
  }

  ngOnInit(): void {
    // buscador de contactos.
    jQuery('#clientId').select2({
      theme: 'bootstrap-5',
      placeholder: 'Buscar contacto',
      ajax: {
        url: this.appURL + 'Contact/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    });
    // cargar lista de cajas.
    this.cajaService.index().subscribe(result => this.listaDeCajas = result);
    // cargar lista tipos de operación.
    this.sunatService.typeOperation().subscribe(result => this.typeOperation = result);
  }

}
