import { Component, inject, signal } from '@angular/core';
//import { RouterOutlet } from '@angular/router';
import { TicketList } from './components/ticket-list/ticket-list.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  imports: [TicketList, MatToolbarModule, MatSnackBarModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('mini-ticket-system');
}
