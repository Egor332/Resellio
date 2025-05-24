import {EventDto} from "../../dtos/EventDto.ts";
import React, {useEffect, useState} from "react";
import {
    Box,
    CircularProgress,
} from '@mui/material';
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";
import EventGrid from "./EventGrid.tsx";
import EventPagination from "./EventPagination.tsx";
import EventFilters from "./EventFilters.tsx";

export const EventBrowser: React.FC = () => {
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

    if (loading) {
        return <CircularProgress />;
    }

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
            />

            <EventGrid events={events}/>

            <EventPagination totalPages={totalPages} currentPage={currentPage} handlePageChange={handlePageChange}/>
            
        </Box>
    );
};