export interface Ticket {
  id: string;
  title: string;
  description: string;
  statusId: string;
  statusName: string;
}

export interface CreateTicketDto {
  title: string;
  description: string;
  statusId: string;
}

export interface UpdateTicketStatusDto {
  id: string;
  title: string;
  description: string;
  statusId: string;
}
