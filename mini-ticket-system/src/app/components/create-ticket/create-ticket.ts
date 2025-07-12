import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TicketForm } from '../ticket-form/ticket-form';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-create-ticket',
  imports: [MatButtonModule, MatButtonModule],
  templateUrl: './create-ticket.html',
  styleUrl: './create-ticket.scss',
})
export class CreateTicket {
  readonly dialog = inject(MatDialog);

  openDialog() {
    this.dialog.open(TicketForm);
  }
}
