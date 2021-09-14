import {Component, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-cash-in-out-modal',
  templateUrl: './cash-in-out-modal.component.html',
  styleUrls: ['./cash-in-out-modal.component.scss']
})
export class CashInOutModalComponent implements OnInit {
  faBars = faBars;

  constructor() {
  }

  ngOnInit(): void {
  }

}
