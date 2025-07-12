import { Component, Inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TicketStatus } from '../../models/ticket-status';
import { TicketService } from '../../services/ticket';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-ticket-form',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatButtonModule,
  ],
  templateUrl: './ticket-form.html',
  styleUrl: './ticket-form.scss',
})
export class TicketForm {
  form: FormGroup;
  statuses: TicketStatus[] = [];
  isEditMode = false;

  constructor(
    private formBuilder: FormBuilder,
    private ticketService: TicketService,
    private dialogRef: MatDialogRef<TicketForm>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (data && data.ticket) {
      this.isEditMode = true;
      this.form = this.formBuilder.group({
        id: [data.ticket.id],
        title: [data.ticket.title, Validators.required],
        description: [data.ticket.description, Validators.required],
        statusId: [data.ticket.statusId, Validators.required],
      });
    } else {
      this.form = this.formBuilder.group({
        title: ['', Validators.required],
        description: ['', Validators.required],
        statusId: ['', Validators.required],
      });
    }
  }

  ngOnInit() {
    this.ticketService.getStatuses().subscribe((statuses) => {
      this.statuses = statuses;
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const ticketData = this.form.value;
      if (this.isEditMode && this.data && this.data.ticket) {
        this.ticketService
          .updateTicket(this.form.value.id, ticketData)
          .subscribe({
            next: (ticket) => {
              this.form.reset();
              this.ticketService.notifyTicketsChanged();
              this.dialogRef.close();
            },
            error: (error) => {
              console.error('Error updating ticket:', error);
            },
          });
      } else {
        this.ticketService.createTicket(ticketData).subscribe({
          next: (ticket) => {
            this.form.reset();
            this.ticketService.notifyTicketsChanged();
            this.dialogRef.close();
          },
          error: (error) => {
            console.error('Error creating ticket:', error);
          },
        });
      }
    } else {
      console.warn('Form is invalid');
    }
  }
}
