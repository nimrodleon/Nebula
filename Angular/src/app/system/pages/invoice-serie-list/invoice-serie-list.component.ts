import {Component, OnInit} from '@angular/core';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-invoice-serie-list',
  templateUrl: './invoice-serie-list.component.html',
  styleUrls: ['./invoice-serie-list.component.scss']
})
export class InvoiceSerieListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
