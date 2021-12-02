import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-invoice-serie-modal',
  templateUrl: './invoice-serie-modal.component.html',
  styleUrls: ['./invoice-serie-modal.component.scss']
})
export class InvoiceSerieModalComponent implements OnInit {
  @Input()
  title: string = '';
  faBars = faBars;

  constructor() {
  }

  ngOnInit(): void {
  }

}
