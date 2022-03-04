import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-router-link',
  templateUrl: './router-link.component.html',
  styleUrls: ['./router-link.component.scss']
})
export class RouterLinkComponent implements OnInit {
  @Input() icon: any;
  @Input() title: string = '';
  @Input() detail: string = '';
  @Input() href: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
