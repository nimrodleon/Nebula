import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-invoice-note',
  templateUrl: './invoice-note.component.html',
  styleUrls: ['./invoice-note.component.scss']
})
export class InvoiceNoteComponent implements OnInit {
  faPlus = faPlus;
  faArrowLeft = faArrowLeft;
  faSave = faSave;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faIdCardAlt = faIdCardAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
