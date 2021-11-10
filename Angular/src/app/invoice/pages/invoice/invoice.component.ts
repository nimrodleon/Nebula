import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faPlus, faSave, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

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

  constructor() {
  }

  ngOnInit(): void {
  }

}
