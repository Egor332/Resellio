import React from 'react';
import {Card, CardContent, Typography, Box, Button, CardMedia} from '@mui/material';
import {EventDto} from "../../dtos/EventDto.ts";
import {useSelector} from "react-redux";

const selectCurrentUserRole = (state: any): string | null => {
    return state.auth.user?.role ?? null;
};

// placeholder image
const defaultImage = "https://via.placeholder.com/400x200?text=Event";

export const EventCard: React.FC<{ event: EventDto }> = ({event}) => {
    const role = useSelector(selectCurrentUserRole);
    const isOrganiser = role === 'Organiser';
    
    return (
        <Card
            variant="outlined"
            sx={{
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
                width: 300 // stała szerokość dla lepszego rozmieszczenia
            }}
        >
            {/* Obrazek */}
            <CardMedia
                component="img"
                height="180"
                image={defaultImage} // TODO add event.image here
                alt={event.name}
                sx={{ objectFit: 'cover' }}
            />

            {/* Treść */}
            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                {/* Nazwa */}
                <Typography variant="h6" align="center" gutterBottom noWrap>
                    {event.name}
                </Typography>

                {/* Daty */}
                <Typography variant="body2" color="textSecondary" align="center" noWrap>
                    {new Date(event.start).toLocaleString()} – {new Date(event.end).toLocaleString()}
                </Typography>

                {Array.isArray(event.ticketTypes) && event.ticketTypes.length > 0 && (
                    <Box mt={1}>
                        {event.ticketTypes.map((ticket, index) => (
                            <Box key={index} mb={0.5}>
                                <Typography variant="body2" align="center">
                                    {ticket.description}: {ticket.price.toFixed(2)} {ticket.currency}
                                </Typography>
                                <Typography variant="caption" color="textSecondary" align="center" display="block">
                                    Available from: {new Date(ticket.availableFrom).toLocaleDateString()}
                                </Typography>
                                <Typography variant="caption" color="textSecondary" align="center" display="block">
                                    Max: {ticket.maxCount} tickets
                                </Typography>
                            </Box>
                        ))}
                    </Box>
                )}


                        {/* Przycisk */}
                <Box mt="auto" display="flex" justifyContent="center" pt={2}>
                    {isOrganiser ? (
                        <Button variant="outlined" color="primary" size="small">
                            Edit event
                        </Button>
                    ) : (
                        <Button variant="contained" color="primary" size="small">
                            Buy ticket
                        </Button>
                    )}
                </Box>
            </CardContent>
        </Card>
    );
};
