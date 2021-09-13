import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashDetailComponent } from './cash-detail.component';

describe('CashDetailComponent', () => {
  let component: CashDetailComponent;
  let fixture: ComponentFixture<CashDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CashDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CashDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
