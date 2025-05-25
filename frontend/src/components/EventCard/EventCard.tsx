import React from 'react';
import {Card, CardContent, Typography, Box, Button, CardMedia} from '@mui/material';
import {EventDto} from "../../dtos/EventDto.ts";
import {useSelector} from "react-redux";

const selectCurrentUserRole = (state: any): string | null => 
    state.auth.user?.role ?? null;

const defaultImage = "https://via.placeholder.com/400x200?text=Event";

export const EventCard: React.FC<{ event: EventDto }> = ({event}) => {
    const role = useSelector(selectCurrentUserRole);
    const isOrganiser = role === 'Organiser';
    
    return (
        <Card
            variant="outlined"
            sx={{
                height: '100%',
                width: '100%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
            }}
        >
            <CardMedia
                component="img"
                height="180"
                image={defaultImage} // TODO add event.image here
                alt={event.name}
                sx={{ objectFit: 'cover' }}
            />

            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                <Typography variant="h6" align="center" gutterBottom noWrap>
                    {event.name}
                </Typography>

                <Typography variant="body2" color="textSecondary" align="center" noWrap>
                    {new Date(event.start).toLocaleString()} – {new Date(event.end).toLocaleString()}
                </Typography>

                {Array.isArray(event.ticketTypes) && event.ticketTypes.length > 0 && (
                    <Box mt={1}>
                        {event.ticketTypes.map((ticket) => (
                            <Box key={ticket.id} mb={0.5}>
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