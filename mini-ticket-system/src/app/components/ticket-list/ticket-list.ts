import { Component, OnInit, OnDestroy } from '@angular/core';
import { TicketService } from '../../services/ticket';
import { Ticket } from '../../models/ticket';
import { Subscription } from 'rxjs';
import { TicketItem } from '../ticket-item/ticket-item';
import { TicketFilter } from '../ticket-filter/ticket-filter'; // <-- import
import { MatDividerModule } from '@angular/material/divider';
import { CreateTicket } from '../create-ticket/create-ticket';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [TicketItem, TicketFilter, MatDividerModule, CreateTicket], // <-- add TicketFilter
  templateUrl: './ticket-list.html',
  styleUrl: './ticket-list.scss',
})
export class TicketList implements OnInit, OnDestroy {
  tickets: Ticket[] = [];
  filteredTickets: Ticket[] = [];
  currentStatusId: string | null = null;
  private sub?: Subscription;

  constructor(private ticketService: TicketService) {}

  ngOnInit() {
    this.fetchTickets();
    this.sub = this.ticketService.ticketsChanged$.subscribe(() => {
      this.fetchTickets();
    });
  }

  fetchTickets() {
    this.ticketService.getTickets().subscribe((tickets) => {
      this.tickets = tickets;
      this.filteredTickets = tickets;
      if (this.currentStatusId) {
        this.fiterTickets();
      } else {
        this.filteredTickets = this.tickets;
      }
    });
  }

  onFilterChange(statusId: string | null) {
    if (statusId === null) {
      this.currentStatusId = null;
      this.filteredTickets = this.tickets;
    } else {
      this.currentStatusId = statusId;
      this.fiterTickets();
    }
  }

  fiterTickets() {
    this.filteredTickets = this.tickets.filter(
      (t) => t.statusId === this.currentStatusId
    );
  }

  ngOnDestroy() {
    this.sub?.unsubscribe();
  }

  trackById(index: number, ticket: Ticket) {
    return ticket.id;
  }
}
