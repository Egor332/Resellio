import {EventBaseDto} from "../../dtos/EventBaseDto.tsx";
import {EventExtendedDto} from "../../dtos/EventExtendedDto.tsx";
import React from "react";
import {EventCard} from "../EventCard/EventCard.tsx";
import { Box } from '@mui/material';

export const EventList: React.FC<{ events: (EventBaseDto | EventExtendedDto)[] }> = ({events}) => {

    if (events.length === 0){
        return <p>No events available</p>
    }

    return (
        <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
            {events.map((event) => (
                <Box sx={{ width: { xs: '100%', sm: '45%', md: '30%' } }} key={event.id}>
                    <EventCard event={event} />
                </Box>
            ))}
        </Box>
    );
};