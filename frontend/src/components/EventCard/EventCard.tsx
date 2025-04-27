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
        <Card variant="outlined" sx={{ height: '100%', display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}>
            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                {/* Name */}
                <Typography variant="h6" align="center" gutterBottom>{event.name}</Typography>
                

                {/* Dates */}
                <Typography variant="body2" color="textSecondary" mt={2} align="center">
                    {new Date(event.start).toLocaleString()} – {new Date(event.end).toLocaleString()}
                </Typography>

                {/*/!* Description *!/*/}
                {/*{event.description && (*/}
                {/*    <Typography variant="body1" mt={1} align="center">*/}
                {/*        {event.description}*/}
                {/*    </Typography>*/}
                {/*)}*/}

                {/* Organizer */}
                <Typography variant="caption" display="block" mt={2} align="center">
                    Organizer: {event.organiserName}
                </Typography>

                {/* Tickets balance – display if got the extended event version */}
                {('ticketsSold' in event && 'ticketsTotal' in event) && (
                    <Typography  variant="body2" mt={1} align="center">
                        Sold: {event.ticketsSold} / {event.ticketsTotal}
                    </Typography>
                )}

                {/* Button */}
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
