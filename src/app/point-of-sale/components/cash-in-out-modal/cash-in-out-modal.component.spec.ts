import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashInOutModalComponent } from './cash-in-out-modal.component';

describe('CashInOutModalComponent', () => {
  let component: CashInOutModalComponent;
  let fixture: ComponentFixture<CashInOutModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CashInOutModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CashInOutModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
