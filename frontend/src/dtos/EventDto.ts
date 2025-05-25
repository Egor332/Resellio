export interface TicketType {
    id?: number;
    description: string;
    maxCount: number;
    price: number;
    currency: string;
    availableFrom: Date;
}

export interface EventDto {
    id?: number;
    name: string;
    description: string;
    start: Date | string;
    end: Date | string;
    ticketTypes: TicketType[];
    image: File | string | null;
}

export interface EventDtoFetchResponse {
    id: number;
    name: string;
    description: string;
    start: string;
    end: string;
    imageUri: string;
    organiserId: number;
}