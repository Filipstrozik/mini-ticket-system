import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TicketForm } from '../ticket-form/ticket-form.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-create-ticket',
  imports: [MatButtonModule, MatButtonModule],
  templateUrl: './create-ticket.component.html',
  styleUrl: './create-ticket.component.scss',
})
export class CreateTicket {
  readonly dialog = inject(MatDialog);

  openDialog() {
    this.dialog.open(TicketForm);
  }
}
