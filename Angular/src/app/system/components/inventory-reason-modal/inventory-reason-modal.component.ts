import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-inventory-reason-modal',
  templateUrl: './inventory-reason-modal.component.html',
  styleUrls: ['./inventory-reason-modal.component.scss']
})
export class InventoryReasonModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
