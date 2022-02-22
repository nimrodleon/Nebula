import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validator, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {faArrowLeft, faClipboardList, faFolderOpen, faSave, faThumbtack} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {ContactService} from 'src/app/contact/services';
import {ConfigurationService} from '../../services';
import Swal from 'sweetalert2';

declare var jQuery: any;

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss']
})
export class ConfigurationComponent implements OnInit {
  faClipboardList = faClipboardList;
  faThumbtack = faThumbtack;
  faArrowLeft = faArrowLeft;
  faSave = faSave;
  faFolderOpen = faFolderOpen;
  private appURL: string = environment.applicationUrl;
  configForm: FormGroup = this.fb.group({
    id: [0],
    ruc: ['', [Validators.required]],
    rznSocial: ['', [Validators.required]],
    codLocalEmisor: ['', [Validators.required]],
    tipMoneda: ['', [Validators.required]],
    porcentajeIgv: [0, [Validators.required]],
    valorImpuestoBolsa: [0, [Validators.required]],
    cpeSunat: ['SFS', [Validators.required]],
    cuentaBancoDetraccion: [''],
    textoDetraccion: [''],
    montoDetraccion: [0],
    contactId: [null, [Validators.required]],
    urlApi: ['', [Validators.required]],
    fileSunat: ['', [Validators.required]],
    fileControl: ['', [Validators.required]]
  });

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private contactService: ContactService,
    private configurationService: ConfigurationService) {
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
        this.configForm.controls['contactId'].setValue(params.data.id);
      });
    // establecer valores por defecto.
    this.configurationService.show().subscribe(result => {
      this.configForm.reset(result);
      // TODO: corregir esta linea de código.
      // this.contactService.show(result.contactId).subscribe(result => {
      //   const newOption = new Option(`${result.document} - ${result.name}`, <any>result.id, true, true);
      //   contactId.append(newOption).trigger('change');
      // });
    });
  }

  // volver una página atrás.
  public historyBack(): void {
    window.history.back();
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.configForm.controls[field].errors && this.configForm.controls[field].touched;
  }

  // guardar cambios.
  public async saveChanges() {
    if (this.configForm.invalid) {
      this.configForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const id = this.configForm.get('id')?.value;
    this.configurationService.update(id, this.configForm.value)
      .subscribe(result => {
        Swal.fire(
          'Información',
          result.msg || '',
          'info'
        );
      });
  }

}
