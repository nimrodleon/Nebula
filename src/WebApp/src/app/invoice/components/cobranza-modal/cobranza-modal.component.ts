import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-cobranza-modal',
  templateUrl: './cobranza-modal.component.html',
  styleUrls: ['./cobranza-modal.component.scss']
})
export class CobranzaModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
