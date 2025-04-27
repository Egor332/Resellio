import React from 'react';
import { Card, CardContent, Typography, Box, Button } from '@mui/material';
import {EventBaseDto} from "../../dtos/EventBaseDto.tsx";
import {EventExtendedDto} from "../../dtos/EventExtendedDto.tsx";
import {useSelector} from "react-redux";

const selectCurrentUserRole = (state: any): string | null => {
    return state.auth.user?.role ?? null;
};

export const EventCard: React.FC<{ event: EventBaseDto | EventExtendedDto }> = ({event}) => {
    const role = useSelector(selectCurrentUserRole);
    const isOrganiser = role === 'Organiser';
    
    return (
        <Card variant="outlined" sx={{ mb: 2 }}>
            <CardContent>
                {/* Name */}
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <Typography variant="h6" align="center">{event.name}</Typography>
                </Box>

                {/* Dates */}
                <Typography variant="body2" color="textSecondary">
                    {new Date(event.start).toLocaleString()} – {new Date(event.end).toLocaleString()}
                </Typography>

                {/* Description */}
                {event.description && (
                    <Typography variant="body1" mt={1}>
                        {event.description}
                    </Typography>
                )}

                {/* Organizer */}
                <Typography variant="caption" display="block" mt={1}>
                    Organizer: {event.organiserName}
                </Typography>

                {/* Tickets balance – display if got the extended event version */}
                {('ticketsSold' in event && 'ticketsTotal' in event) && (
                    <Typography variant="body2" mt={1}>
                        Sold: {event.ticketsSold} / {event.ticketsTotal}
                    </Typography>
                )}

                {/* Button */}
                <Box mt={2}>
                    {isOrganiser ? (
                        <Button variant="outlined" color="primary">
                            Edit event
                        </Button>
                    ) : (
                        <Button variant="contained" color="primary">        
                            Buy ticket
                        </Button>
                    )}
                </Box>
            </CardContent>
        </Card>
    );
};
