import {EventDto} from "../../dtos/EventDto.ts";
import React, {useState} from "react";
import {EventCard} from "../EventCard/EventCard.tsx";
import {Box, Checkbox, FormControlLabel, Grid, Pagination, Stack, TextField} from '@mui/material';

const ITEMS_PER_PAGE = 3;

export const EventList: React.FC<{ events: (EventDto)[] }> = ({events}) => {
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    // future events are those which have not ended by now
    const [showFutureOnly, setShowFutureOnly] = useState(false);
    
    const now = new Date();
    
    const filteredEvents = events.filter(event => {
            const matchesSearch = event.name.toLowerCase().includes(searchQuery.toLowerCase());
            const isFutureEvent = new Date(event.end) > now;
            return matchesSearch && (!showFutureOnly || isFutureEvent);
    });
    
    const pageCount = Math.ceil(filteredEvents.length / ITEMS_PER_PAGE);
    const paginatedEvents = filteredEvents.slice(
        (currentPage - 1) * ITEMS_PER_PAGE,
        currentPage * ITEMS_PER_PAGE
    );
    
    const handlePageChange = (_: React.ChangeEvent<unknown>,  page: number) => {
      setCurrentPage(page);  
    };
    
    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchQuery(e.target.value);
        setCurrentPage(1);
    };

    const handleShowFutureOnlyChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setShowFutureOnly(e.target.checked);
        setCurrentPage(1);
    };
    
    if (events.length === 0){
        return <p>No events available</p>
    }

    return (
        <Box display='flex' alignItems="center" flexDirection='column' gap={4}>
            {/* Search + Filter */}
            <Box display="flex" flexDirection={{ xs: 'column', sm: 'row' }} alignItems={{ sm: 'center' }} gap={2} mb={2}>
                <TextField
                    label="Search events"
                    variant="outlined"
                    value={searchQuery}
                    onChange={handleSearchChange}
                />
                <FormControlLabel
                    control={
                        <Checkbox
                            checked={showFutureOnly}
                            onChange={handleShowFutureOnlyChange}
                            color="primary"
                        />
                    }
                    label="Hide past events"
                />
            </Box>


            {/* Event Grid */}
            <Grid container spacing={3} justifyContent="center">
                {paginatedEvents.map((event) => (
                    <Grid size={{ xs: 12, sm: 6, md: 3 }} key={event.name}>
                        <EventCard event={event} />
                    </Grid>
                ))}
            </Grid>
            
            {/* Stronicowanie */}
            <Stack mt={4} alignItems="center">
                <Pagination
                    count={pageCount}
                    page={currentPage}
                    onChange={handlePageChange}
                    color="primary"
                />
            </Stack>
        </Box>
    );
};