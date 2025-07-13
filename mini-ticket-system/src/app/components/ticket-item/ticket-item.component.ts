import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatBadgeModule } from '@angular/material/badge';
import { Ticket } from '../../models/ticket';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { TicketForm } from '../ticket-form/ticket-form.component';

@Component({
  selector: 'app-ticket-item',
  standalone: true,
  imports: [MatCardModule, MatBadgeModule, MatButtonModule],
  templateUrl: './ticket-item.component.html',
  styleUrl: './ticket-item.component.scss',
})
export class TicketItem {
  @Input() ticket!: Ticket;
  readonly dialog = inject(MatDialog);

  onEdit() {
    this.dialog.open(TicketForm, {
      data: { ticket: this.ticket },
    });
  }
}
