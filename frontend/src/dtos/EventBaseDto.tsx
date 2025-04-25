export interface EventBaseDto {
    id: number;
    name: string;
    description?: string;
    start: string;
    end: string;
    organiserName: string;
    organiserId: number;
}