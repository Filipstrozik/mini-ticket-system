import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatBadgeModule } from '@angular/material/badge';
import { Ticket } from '../../models/ticket';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { TicketForm } from '../ticket-form/ticket-form.component';
import { TicketService } from '../../services/ticket.service';

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

  readonly ticketService = inject(TicketService);

  onEdit() {
    this.dialog.open(TicketForm, {
      data: { ticket: this.ticket },
    });
  }

  onDelete() {
    this.ticketService.deleteTicket(this.ticket.id).subscribe({
      next: () => {
        this.ticketService.notifyTicketsChanged();
        console.log('Ticket deleted successfully');
      },
      error: (err) => {
        console.error('Error deleting ticket:', err);
      },
    });
  }
}
