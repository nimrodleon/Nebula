import {Component, OnInit} from '@angular/core';
import {faPlus, faSignOutAlt, faSyncAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-caja-diaria',
  templateUrl: './caja-diaria.component.html',
  styleUrls: ['./caja-diaria.component.scss']
})
export class CajaDiariaComponent implements OnInit {
  faSyncAlt = faSyncAlt;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faSignOutAlt = faSignOutAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
