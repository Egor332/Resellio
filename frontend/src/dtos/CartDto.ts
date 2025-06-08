import { TicketDto } from './TicketDto';

export interface CartInfoResponse {
  isCartExist: boolean;
  ticketsInCart: TicketDto[];
  cartExpirationTime: string;
}