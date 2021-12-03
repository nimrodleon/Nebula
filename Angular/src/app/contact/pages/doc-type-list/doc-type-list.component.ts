import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

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
  docTypeModal: any;

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    // modal tipo de documento.
    this.docTypeModal = new bootstrap.Modal(document.querySelector('#doc-type-modal'));
  }

  // abrir modal tipo de documento.
  public showDocTypeModal(): void {
    this.title = 'Agregar Tipo de documento';
    this.docTypeModal.show();
  }

}
