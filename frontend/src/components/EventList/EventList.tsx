import {EventBaseDto} from "../../dtos/EventBaseDto.tsx";
import {EventExtendedDto} from "../../dtos/EventExtendedDto.tsx";
import React, {useState} from "react";
import {EventCard} from "../EventCard/EventCard.tsx";
import {Box, Grid, Pagination, Stack, TextField} from '@mui/material';

const ITEMS_PER_PAGE = 6;

export const EventList: React.FC<{ events: (EventBaseDto | EventExtendedDto)[] }> = ({events}) => {
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    
    const filteredEvents = events.filter(event =>
        event.name.toLowerCase().includes(searchQuery.toLowerCase())
    );
    
    const pageCount = Math.ceil(filteredEvents.length / ITEMS_PER_PAGE);
    const paginatedEvents = filteredEvents.slice(
        (currentPage - 1) * ITEMS_PER_PAGE,
        currentPage * ITEMS_PER_PAGE
    );
    
    const handlePageChange = (_: any,  page: number) => {
      setCurrentPage(page);  
    };
    
    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchQuery(e.target.value);
        setCurrentPage(1);
    };
    
    if (events.length === 0){
        return <p>No events available</p>
    }

    return (
        <Box display='flex' flexDirection='column' gap={4}>
            {/* Search */}
            <Box mb={2}>
                <TextField
                    label="Search events"
                    variant="outlined"
                    fullWidth
                    value={searchQuery}
                    onChange={handleSearchChange}
                />
            </Box>

            {/* Event Grid */}
            <Grid container spacing={3}>
                {paginatedEvents.map((event) => (
                    <Grid item xs={12} sm={6} md={4} key={event.id}>
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