import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-warehouse-modal',
  templateUrl: './warehouse-modal.component.html',
  styleUrls: ['./warehouse-modal.component.scss']
})
export class WarehouseModalComponent implements OnInit {
  @Input()
  title: string = '';
  faBars = faBars;

  constructor() {
  }

  ngOnInit(): void {
  }

}
