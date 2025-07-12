import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketItem } from './ticket-item';

describe('TicketItem', () => {
  let component: TicketItem;
  let fixture: ComponentFixture<TicketItem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketItem]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketItem);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
