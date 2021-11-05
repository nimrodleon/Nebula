import {Component, OnInit} from '@angular/core';
import {faEnvelope, faPrint, faTimes} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {
  faTimes = faTimes;
  faEnvelope = faEnvelope;
  faPrint = faPrint;

  constructor() {
  }

  ngOnInit(): void {
  }

}
