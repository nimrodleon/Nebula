import {Component, OnInit} from '@angular/core';
import {faBars, faMoneyBillAlt, faMoneyCheckAlt, faPiggyBank} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-cobrar-modal',
  templateUrl: './cobrar-modal.component.html',
  styleUrls: ['./cobrar-modal.component.scss']
})
export class CobrarModalComponent implements OnInit {
  faBars = faBars;
  faMoneyBillAlt = faMoneyBillAlt;
  faMoneyCheckAlt = faMoneyCheckAlt;
  faPiggyBank = faPiggyBank;

  constructor() {
  }

  ngOnInit(): void {
  }

}
