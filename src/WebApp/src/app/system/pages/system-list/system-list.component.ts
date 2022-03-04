import {Component, OnInit} from '@angular/core';
import {faCog, faReceipt, faTags, faUserCog, faWarehouse} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-system-list',
  templateUrl: './system-list.component.html',
  styleUrls: ['./system-list.component.scss']
})
export class SystemListComponent implements OnInit {
  faCog = faCog;
  faUserCog = faUserCog;
  faWarehouse = faWarehouse;
  faReceipt = faReceipt;
  faTags = faTags;

  constructor() {
  }

  ngOnInit(): void {
  }

}
