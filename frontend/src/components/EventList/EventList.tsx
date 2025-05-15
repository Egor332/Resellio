import {EventDto} from "../../dtos/EventDto.ts";
import React, {useEffect, useState} from "react";
import {EventCard} from "../EventCard/EventCard.tsx";
import {Box, Checkbox, CircularProgress, FormControlLabel, Grid, Pagination, Stack, TextField} from '@mui/material';
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";

export const EventList: React.FC = () => {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    // future events are those which have not ended by now
    const [showFutureOnly, setShowFutureOnly] = useState(false);
    const [totalPages, setTotalPages] = useState(1);

    const [loading, setLoading] = useState(false);

    const ITEMS_PER_PAGE = 3;

    const fetchEvents = async () => {
        setLoading(true);
        try {
            const now = new Date().toISOString();

            const params = {
                "Filter.NamePart": searchQuery || undefined,
                "Filter.StartsAfter": showFutureOnly ? now : undefined,
                "Pagination.Page": currentPage,
                "Pagination.PageSize": ITEMS_PER_PAGE,
            };

            const response = await apiRequest(getApiEndpoint(API_ENDPOINTS.GET_EVENTS), params);

            setEvents(response.Items);
            setTotalPages(response.TotalPages || Math.ceil(response.TotalAmount / ITEMS_PER_PAGE));
        } 
        catch (error) {
            if (error instanceof Error) {
                console.error("Error fetching events:", error.message);
            } else {
                console.error("Error fetching events:", error);
            }
        }
        finally {
            setLoading(false);
        }
    };

    // fetch whenever one of the dependencies changes
    useEffect(() => {
        fetchEvents();
    }, [searchQuery, showFutureOnly, currentPage]);
    
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

    if (loading) {
        return <CircularProgress />;
    }
    
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
                {events.map((event) => (
                    <Grid size={{ xs: 12, sm: 6, md: 3 }} key={event.id}>
                        <EventCard event={event} />
                    </Grid>
                ))}
            </Grid>

            {/* Pagination */}
            <Stack mt={4} alignItems="center">
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    color="primary"
                />
            </Stack>
        </Box>
    );
};