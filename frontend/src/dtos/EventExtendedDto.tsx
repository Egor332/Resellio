import {EventBaseDto} from "./EventBaseDto.tsx";

export interface EventExtendedDto extends EventBaseDto {
    ticketsSold: number;
    ticketsTotal: number;
}