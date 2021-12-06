import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {PeopleDocType} from '../../interfaces';
import {PeopleDocTypeService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-doc-type-list',
  templateUrl: './doc-type-list.component.html',
  styleUrls: ['./doc-type-list.component.scss']
})
export class DocTypeListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  query: FormControl = this.fb.control('');
  currentDocType: PeopleDocType | any;
  peopleDocTypes: Array<PeopleDocType> = new Array<PeopleDocType>();
  docTypeModal: any;

  constructor(
    private fb: FormBuilder,
    private peopleDocTypeService: PeopleDocTypeService) {
  }

  ngOnInit(): void {
    // modal tipo de documento.
    this.docTypeModal = new bootstrap.Modal(document.querySelector('#doc-type-modal'));
    // cargar lista de unidades de medida.
    this.getPeopleDocTypes();
  }

  // cargar unidades de medida.
  private getPeopleDocTypes(): void {
    this.peopleDocTypeService.index(this.query.value)
      .subscribe(result => this.peopleDocTypes = result);
  }

  // buscar unidades de medida.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.getPeopleDocTypes();
  }

  // abrir modal tipo de documento.
  public showDocTypeModal(): void {
    this.title = 'Agregar Tipo de documento';
    this.currentDocType = null;
    this.docTypeModal.show();
  }

  // abrir modal modo edición.
  public editDocTypeModal(id: any): void {
    this.title = 'Editar Tipo de documento';
    this.peopleDocTypeService.show(id).subscribe(result => {
      this.currentDocType = result;
      this.docTypeModal.show();
    })
  }

  // ocultar docType modal.
  public hideDocTypeModal(response: ResponseData<PeopleDocType>): void {
    if(response.ok){
      this.getPeopleDocTypes();
      this.docTypeModal.hide();
    }
  }

}
