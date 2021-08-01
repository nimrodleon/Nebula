import {Component, OnInit, Output, EventEmitter, Input} from '@angular/core';

@Component({
  selector: 'app-system-option',
  templateUrl: './system-option.component.html',
  styleUrls: ['./system-option.component.scss']
})
export class SystemOptionComponent implements OnInit {
  @Input()
  iconLink: any;

  @Input()
  titleLink: string = '';

  @Input()
  detailLink: string = '';

  @Input()
  hrefLink: string = '';

  @Output()
  onClick = new EventEmitter<any>();

  constructor() {
  }

  ngOnInit(): void {
  }

  sendEvent(e: any): void {
    e.preventDefault();
    if (!this.hrefLink) {
      this.onClick.emit(e);
    }
  }

}
