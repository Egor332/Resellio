import React, { useState } from 'react';
import {Card, CardContent, Typography, Box, Button, CardMedia} from '@mui/material';
import {EventDtoFetchResponse} from "../../dtos/EventDto.ts";
import TicketBrowser from '../TicketBrowser/TicketBrowser.tsx';

export const EventCard: React.FC<{ event: EventDtoFetchResponse }> = ({event}) => {
    const [ticketBrowserOpen, setTicketBrowserOpen] = useState(false);

    const handleOpenTicketBrowser = () => {
        setTicketBrowserOpen(true);
    };

    const handleCloseTicketBrowser = () => {
        setTicketBrowserOpen(false);
    };
    
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
                image={event.imageUri}
                alt={event.name}
                sx={{ objectFit: 'cover' }}
            />

            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                <Typography variant="h5" align="center" gutterBottom noWrap>
                    {event.name}
                </Typography>

                <Typography variant="h6" align="center" gutterBottom noWrap>
                    {event.description}
                </Typography>

                <Typography variant="body2" color="textSecondary" align="center" noWrap>
                    {new Date(event.start).toLocaleString()} – {new Date(event.end).toLocaleString()}
                </Typography>

                <Box mt="auto" display="flex" justifyContent="center" pt={2}>
                    <Button 
                        variant="contained" 
                        color="primary" 
                        size="small"
                        onClick={handleOpenTicketBrowser}
                    >
                        View tickets
                    </Button>
                </Box>
            </CardContent>

            <TicketBrowser
                open={ticketBrowserOpen}
                onClose={handleCloseTicketBrowser}
                event={event}
            />
        </Card>
    );
};