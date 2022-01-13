import {Component, OnInit} from '@angular/core';
import {faEdit, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InvoiceNoteService} from 'src/app/invoice/services';
import {InvoiceNote} from 'src/app/invoice/interfaces';

@Component({
  selector: 'app-invoice-note-list',
  templateUrl: './invoice-note-list.component.html',
  styleUrls: ['./invoice-note-list.component.scss']
})
export class InvoiceNoteListComponent implements OnInit {
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  invoiceNotes: Array<InvoiceNote> = new Array<InvoiceNote>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
    query: ['']
  });

  constructor(
    private fb: FormBuilder,
    private invoiceNoteService: InvoiceNoteService) {
  }

  ngOnInit(): void {
    this.submitQuery();
  }

  // buscar notas crédito/débito.
  public submitQuery(): void {
    this.invoiceNoteService.index('COMPRA', this.queryForm.value)
      .subscribe(result => this.invoiceNotes = result);
  }

}
