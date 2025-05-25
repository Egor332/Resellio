import {EventDto} from "../../dtos/EventDto.ts";
import React, {useEffect, useState} from "react";
import {
    Box,
    CircularProgress,
    Button,
} from '@mui/material';
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";
import EventGrid from "./EventGrid.tsx";
import EventPagination from "./EventPagination.tsx";
import EventFilters from "./EventFilters.tsx";

export const EventBrowser: React.FC<{
    showOrganiserNameFilter: boolean
    organiserName?: string
}> = ({showOrganiserNameFilter, organiserName = ""}) => {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [organiserNamePart, setOrganiserNamePart] = useState(organiserName);
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

            const params = new URLSearchParams({
                "Pagination.Page": currentPage.toString(),
                "Pagination.PageSize": itemsPerPage.toString()
            });

            if (searchQuery) 
                params.append("Filter.NamePart", searchQuery);
            if (showFutureOnly || startsAfter) 
                params.append("Filter.StartsAfter", showFutureOnly ? now : startsAfter);
            if (endsBefore) 
                params.append("Filter.EndsBefore", endsBefore);
            if (organiserNamePart) 
                params.append("Filter.OrganiserNamePart", organiserNamePart);
        
            const response = await apiRequest(getApiEndpoint(API_ENDPOINTS.GET_EVENTS, params));
            
            setEvents(response?.items || []);
            setTotalPages(Math.ceil((response?.totalAmount || 0) / itemsPerPage));
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

    // Fetch on mount, then only after Apply button click or page change
    useEffect(() => {
        fetchEvents();
    }, [currentPage]);

    const handlePageChange = (_: React.ChangeEvent<unknown>,  page: number) => {
      setCurrentPage(page);  
    };

    return (
        <Box display="flex" alignItems="center" flexDirection="column" gap={4}>

            <EventFilters
                searchQuery={searchQuery}
                setSearchQuery={setSearchQuery}
                showFutureOnly={showFutureOnly}
                setShowFutureOnly={setShowFutureOnly}
                organiserNamePart={organiserNamePart}
                setOrganiserNamePart={setOrganiserNamePart}
                startsAfter={startsAfter}
                setStartsAfter={setStartsAfter}
                endsBefore={endsBefore}
                setEndsBefore={setEndsBefore}
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                setCurrentPage={setCurrentPage}
                showOrganiserNameFilter={showOrganiserNameFilter}
            />

            <Button
                type="submit"
                variant="contained"
                color="primary"
                sx={{ 
                    mt: 2,
                    width: { xs: '100%', md: '33%' },
                    alignSelf: 'center'
                }}
                disabled={loading}
                onClick={() => {
                    fetchEvents();
                }}
            >
                Apply
            </Button>

            {loading ? (
                <Box display="flex" justifyContent="center" my={4}>
                    <CircularProgress 
                        size={60} 
                        thickness={12} 
                        sx={{ color: 'primary' }} 
                    />
                </Box>
            ) : (
                <EventGrid events={events}/>
            )}

            <EventPagination totalPages={totalPages} currentPage={currentPage} handlePageChange={handlePageChange}/>
            
        </Box>
    );
};