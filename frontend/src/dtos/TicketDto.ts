export interface Price {
  amount: number;
  currencyCode: string;
}

export interface TicketTypeDto {
    typeId: number;
    description: string;
    availableFrom: string;
    amountOfTickets: number;
    basePrice: Price;
    eventId: number;
}

export interface TicketDto {
    id: string;
    isOnSale: boolean;
    currentPrice: Price;
    eventName: string;
    eventDescription: string;
    ticketTypeDescription: string;
    isHoldByOrganiser: boolean;
    eventId: number;
    ticketTypeId: number;
    sellerId: number;
}

export interface TicketTypeFetchResponse {
    items: TicketTypeDto[];
    pageNumber: number;
    totalAmount: number;    
}

export interface TicketFetchResponse {
    items: TicketDto[];
    pageNumber: number;
    totalAmount: number;
}

export interface TicketLockResponse {
    ticketId: string;
    expirationTime: string;
    success: boolean;
    message: string;
    errorCode: number;
}