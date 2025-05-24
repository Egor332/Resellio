import React from "react";
import {EventDto} from "../../dtos/EventDto.ts";
import {Box, Grid, Typography} from "@mui/material";
import {EventCard} from "../EventCard/EventCard.tsx";

const EventGrid: React.FC<{ events: EventDto[] }> = ({ events }) => {
    return (
        (events.length > 0 ?  
            <Grid container spacing={3} justifyContent="center">
                {events.map((event) => (
                    <Grid size={{xs: 12, sm: 6, md: 4}} key={event.id}>
                        <EventCard event={event} />
                    </Grid>
                ))}
            </Grid> 
            :
            <Box mt={4}>
                <Typography variant="h6">No events match your search criteria.</Typography>
            </Box>
        )
    );
};

export default EventGrid;