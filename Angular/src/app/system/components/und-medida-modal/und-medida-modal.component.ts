import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-und-medida-modal',
  templateUrl: './und-medida-modal.component.html',
  styleUrls: ['./und-medida-modal.component.scss']
})
export class UndMedidaModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
