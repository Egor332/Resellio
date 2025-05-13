export interface TicketType {
  description: string;
  maxCount: number;
  price: number;
  currency: string;
  availableFrom: Date;
}

export interface EventDto {
  name: string;
  description: string;
  start: Date | string;
  end: Date | string;
  ticketTypes: TicketType[];
  image: File | string | null;
}
