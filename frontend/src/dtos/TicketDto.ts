export interface TicketTypeDto {
    typeId: number;
    description: string;
    availableFrom: string;
    amountOfTickets: number;
    basePrice: {
        amount: number
        currencyCode: string
    }
    eventId: number;
}

export interface TicketDto {
    id: string;
    isOnSale: boolean;
    currentPrice: {
        amount: number;
        currencyCode: string;
    };
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
