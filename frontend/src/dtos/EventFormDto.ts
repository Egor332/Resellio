export interface EventFormDto {
  name: string;
  description: string;
  start: string;
  end: string;
  ticketsTotal: number;
  // Id is optional because it won't be available for new events
  id?: number;
}
