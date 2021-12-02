import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {Router} from '@angular/router';
import {faArrowLeft, faClipboardList, faSave, faThumbtack} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {EnumBoolean, EnumMenu} from 'src/app/global/interfaces';
import {ContactService} from 'src/app/contact/services';
import {CompanyService} from '../../services';
import Swal from 'sweetalert2';

declare var jQuery: any;

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit {
  faClipboardList = faClipboardList;
  faThumbtack = faThumbtack;
  faArrowLeft = faArrowLeft;
  faSave = faSave;
  private appURL: string = environment.applicationUrl;
  companyForm: FormGroup = this.fb.group({
    id: [0],
    ruc: [''],
    rznSocial: [''],
    codLocalEmisor: [''],
    tipMoneda: [''],
    porcentajeIgv: [0],
    valorImpuestoBolsa: [0],
    completarDatosBoleta: [0],
    cuentaBancoDetraccion: [''],
    textoDetraccion: [''],
    montoDetraccion: [0],
    contactId: [null]
  });

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private contactService: ContactService,
    private companyService: CompanyService) {
  }

  ngOnInit(): void {
    // buscador de clientes.
    const contactId = jQuery('#contactId')
      .select2({
        theme: 'bootstrap-5',
        placeholder: 'BUSCAR CONTACTO',
        ajax: {
          url: this.appURL + 'Contact/Select2',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
          }
        }
      }).on('select2:select', ({params}: any) => {
        this.companyForm.controls['contactId'].setValue(params.data.id);
      });
    // establecer valores por defecto.
    this.companyService.show().subscribe(result => {
      this.companyForm.reset(result);
      this.contactService.show(result.contactId).subscribe(result => {
        const newOption = new Option(`${result.document} - ${result.name}`, <any>result.id, true, true);
        contactId.append(newOption).trigger('change');
      });
    });
  }

  // botón cancelar.
  public async cancel() {
    localStorage.setItem(EnumMenu.rootMenu, EnumBoolean.true);
    localStorage.setItem(EnumMenu.childMenuConfiguration, EnumBoolean.false);
    const mainMenu: any = document.getElementById('mainMenu');
    mainMenu.classList.remove('hiddenNavigation');
    await this.router.navigate(['/']);
  }

  // guardar cambios.
  public async saveChanges() {
    const id = this.companyForm.get('id')?.value;
    this.companyService.update(id, this.companyForm.value).subscribe(result => {
      Swal.fire(
        'Información',
        result.msg || '',
        'info'
      );
    });
  }

}
