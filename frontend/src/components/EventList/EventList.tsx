import {EventDto} from "../../dtos/EventDto.ts";
import React, {useEffect, useState} from "react";
import {EventCard} from "../EventCard/EventCard.tsx";
import {
    Box,
    Checkbox,
    CircularProgress,
    FormControlLabel,
    Grid,
    Pagination,
    Stack,
    TextField,
    Select,
    MenuItem,
    SelectChangeEvent
} from '@mui/material';
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";

export const EventList: React.FC = () => {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [organiserNamePart, setOrganiserNamePart] = useState('');
    const [startsAfter, setStartsAfter] = useState('');
    const [endsBefore, setEndsBefore] = useState('');
    const [showFutureOnly, setShowFutureOnly] = useState(false); // future events are those which have not ended by now

    const [loading, setLoading] = useState(false);

    const [itemsPerPage, setItemsPerPage] = useState(3);
    const [totalPages, setTotalPages] = useState(1);

    const fetchEvents = async () => {
        setLoading(true);
        try {
            const now = new Date().toISOString();

            const rawParams = {
                "Filter.NamePart": searchQuery || undefined,
                "Filter.StartsAfter": showFutureOnly ? now : startsAfter || undefined,
                "Filter.EndsBefore": endsBefore || undefined,
                "Filter.OrganiserNamePart": organiserNamePart || undefined,
                "Pagination.Page": currentPage,
                "Pagination.PageSize": itemsPerPage,
            };
            const filteredParams = Object.entries(rawParams)
                .filter(([, v]) => v !== undefined && v !== '')
                .map(([k, v]) => [k, String(v)]); // gwarantuje, że każda wartość to string
            
            const queryString = new URLSearchParams(filteredParams).toString();
            const fullUrl = `${getApiEndpoint(API_ENDPOINTS.GET_EVENTS).url}?${queryString}`;

            const response = await apiRequest({ ...API_ENDPOINTS.GET_EVENTS, url: fullUrl });
            
            setEvents(response?.items ?? []);
            setTotalPages(Math.ceil(response.totalAmount / itemsPerPage));
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
    }, [searchQuery, showFutureOnly, currentPage, itemsPerPage, organiserNamePart, startsAfter, endsBefore]);


    const handlePageChange = (_: React.ChangeEvent<unknown>,  page: number) => {
      setCurrentPage(page);  
    };

    const handleItemsPerPageChange = (e: SelectChangeEvent<number>) => {
        setItemsPerPage(Number(e.target.value));
        setCurrentPage(1); // Reset to the first page
    };

    if (loading) {
        return <CircularProgress />;
    }
    
    if (events.length === 0){
        return <p>No events available</p>
    }

    return (
        <Box display="flex" alignItems="center" flexDirection="column" gap={4}>
            {/* Search + Filter */}
            <Box width="100%" maxWidth="1000px" px={2}>
                <Grid container spacing={2} alignItems="center">
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <TextField
                            fullWidth
                            label="Search events"
                            variant="outlined"
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                    </Grid>
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={showFutureOnly}
                                    onChange={(e) => setShowFutureOnly(e.target.checked)}
                                    color="primary"
                                />
                            }
                            label="Hide past events"
                        />
                    </Grid>
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <TextField
                            fullWidth
                            label="Organiser Name"
                            variant="outlined"
                            value={organiserNamePart}
                            onChange={(e) => setOrganiserNamePart(e.target.value)}
                        />
                    </Grid>
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <TextField
                            fullWidth
                            label="Starts After"
                            type="date"
                            InputLabelProps={{ shrink: true }}
                            value={startsAfter}
                            onChange={(e) => setStartsAfter(e.target.value)}
                        />
                    </Grid>
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <TextField
                            fullWidth
                            label="Ends Before"
                            type="date"
                            InputLabelProps={{ shrink: true }}
                            value={endsBefore}
                            onChange={(e) => setEndsBefore(e.target.value)}
                        />
                    </Grid>
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <Box display="flex" alignItems="center" gap={1}>
                            <span>Items per page:</span>
                            <Select
                                value={itemsPerPage}
                                onChange={handleItemsPerPageChange}
                            >
                                <MenuItem value={3}>3</MenuItem>
                                <MenuItem value={5}>5</MenuItem>
                                <MenuItem value={10}>10</MenuItem>
                            </Select>
                        </Box>
                    </Grid>
                </Grid>
            </Box>

            {/* Event Grid */}
            <Grid container spacing={3} justifyContent="center">
                {events.map((event) => (
                    <Grid size={{xs: 12, sm: 6, md: 4}} key={event.id}>
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