import { Box, Typography } from '@mui/material'
import { useState, useEffect } from 'react';
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api';
import { TicketFetchResponse, TicketDto } from '../../dtos/TicketDto';
import { apiRequest } from '../../services/httpClient';
import EventPagination from '../../components/EventBrowser/EventPagination';
import Loading from "../../components/Loading/Loading.tsx";
import TicketCard from '../../components/TicketCard/TicketCard.tsx';
import { useSelector } from 'react-redux';
import { RootState } from "../../store/store.ts";


function CustomersMyTickets() {
    const [loadingTickets, setLoadingTickets] = useState(false);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [currentPage, setCurrentPage] = useState(1);
    const [tickets, setTickets] = useState<TicketDto[]>([]);
    const [totalPages, setTotalPages] = useState(1);
    const user = useSelector((state: RootState) => state.auth.user);

    const handlePageChange = (_: React.ChangeEvent<unknown>,  page: number) => {
        setCurrentPage(page);  
    };

    const fetchTickets = async () => {
        setLoadingTickets(true);
        try {
            const params = new URLSearchParams({
                "Pagination.Page": currentPage.toString(),
                "Pagination.PageSize": itemsPerPage.toString(),
            });

            const response: TicketFetchResponse = 
                await apiRequest(getApiEndpoint(API_ENDPOINTS.MY_TICKETS, params));
            
            setTickets(response?.items || []);
            setTotalPages(Math.ceil((response?.totalAmount || 0) / itemsPerPage));
        } catch (error) {
            console.error("Error fetching tickets:", error);
        } finally {
            setLoadingTickets(false);
        }
    };

    useEffect(() => {
        fetchTickets();
    }, [currentPage, itemsPerPage, user?.confirmedSeller]);

  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
        <Typography variant="h4" component="h1" gutterBottom>
            Your Tickets
        </Typography>

        {loadingTickets ? 
            <Loading />
            :
            <>
                {tickets.length > 0 ? (
                    <Box>
                        {tickets.map((ticket) => (
                            <TicketCard key={ticket.id} ticket={ticket} onTicketUpdate={fetchTickets} />
                        ))}
                    </Box>
                ) : (
                    <Typography variant="body1">No tickets found.</Typography>
                )}
            </>
        }
        
        <EventPagination totalPages={totalPages} currentPage={currentPage} handlePageChange={handlePageChange}/>
    </Box>
  )
}
export default CustomersMyTickets;