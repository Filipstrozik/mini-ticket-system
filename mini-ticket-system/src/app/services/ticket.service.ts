import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Ticket, UpdateTicketStatusDto } from '../models/ticket';
import { HttpClient } from '@angular/common/http';
import { TicketStatus } from '../models/ticket-status';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  private readonly apiUrl = environment.apiUrl;

  private ticketsChangedSubject = new Subject<void>();
  ticketsChanged$ = this.ticketsChangedSubject.asObservable();

  constructor(private http: HttpClient) {}

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.apiUrl}/tickets`);
  }

  createTicket(ticket: Ticket): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.apiUrl}/tickets`, ticket);
  }

  updateTicket(id: string, ticket: UpdateTicketStatusDto): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.apiUrl}/tickets/${id}`, ticket);
  }

  getStatuses(): Observable<TicketStatus[]> {
    return this.http.get<TicketStatus[]>(`${this.apiUrl}/statuses`);
  }

  notifyTicketsChanged() {
    this.ticketsChangedSubject.next();
  }

  deleteTicket(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/tickets/${id}`);
  }
}
