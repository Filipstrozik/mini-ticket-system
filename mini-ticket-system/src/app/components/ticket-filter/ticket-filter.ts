import { Component, Output, EventEmitter } from '@angular/core';
import { TicketService } from '../../services/ticket';
import { Ticket } from '../../models/ticket';
import { TicketStatus } from '../../models/ticket-status';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule, MatSelectChange } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-ticket-filter',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './ticket-filter.html',
  styleUrl: './ticket-filter.scss',
})
export class TicketFilter {
  @Output() filterChange = new EventEmitter<string | null>();
  statuses: TicketStatus[] = [];

  constructor(private ticketService: TicketService) {
    this.ticketService.getStatuses().subscribe((statuses) => {
      this.statuses = statuses;
    });
  }

  selectedStatusId: string | null = null;

  onStatusChange(event: MatSelectChange) {
    const value = event.value;
    this.selectedStatusId = value === '' ? null : String(value);
    this.filterChange.emit(this.selectedStatusId);
  }

  clearFilter(event: MouseEvent) {
    this.selectedStatusId = null;
    this.filterChange.emit(null);
    event.stopPropagation(); // Prevent the click from propagating!!
  }
}
