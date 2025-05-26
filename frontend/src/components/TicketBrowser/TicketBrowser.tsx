import React, { useState, useEffect } from 'react';
import {
    Dialog,
    DialogContent,
    DialogTitle,
    IconButton,
    Typography,
    Box
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { EventDtoFetchResponse } from '../../dtos/EventDto';
import { TicketTypeDto, TicketDto, TicketTypeFetchResponse, TicketFetchResponse } from '../../dtos/TicketDto';
import { apiRequest } from '../../services/httpClient';
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api';
import TicketTypeList from './TicketTypeList';
import TicketList from './TicketList';

interface TicketBrowserProps {
    open: boolean;
    onClose: () => void;
    event: EventDtoFetchResponse;
}

const TicketBrowser: React.FC<TicketBrowserProps> = ({ open, onClose, event }) => {
    const [ticketTypes, setTicketTypes] = useState<TicketTypeDto[]>([]);
    const [tickets, setTickets] = useState<TicketDto[]>([]);
    const [selectedTicketType, setSelectedTicketType] = useState<TicketTypeDto | null>(null);
    const [loadingTicketTypes, setLoadingTicketTypes] = useState(false);
    const [loadingTickets, setLoadingTickets] = useState(false);
    
    const [currentTypePage, setCurrentTypePage] = useState(1);
    const [totalTypePages, setTotalTypePages] = useState(1);
    const [itemsPerTypePage] = useState(5);
    
    const [currentTicketPage, setCurrentTicketPage] = useState(1);
    const [totalTicketPages, setTotalTicketPages] = useState(1);
    const [itemsPerTicketPage] = useState(10);

    const fetchTicketTypes = async () => {
        setLoadingTicketTypes(true);
        try {
            const params = new URLSearchParams({
                "Filter.EventId": event.id.toString(),
                "Pagination.Page": currentTypePage.toString(),
                "Pagination.PageSize": itemsPerTypePage.toString()
            });

            const response: TicketTypeFetchResponse = 
                await apiRequest(getApiEndpoint(API_ENDPOINTS.GET_TICKET_TYPES, params));
            
            setTicketTypes(response?.items || []);
            setTotalTypePages(Math.ceil((response?.totalAmount || 0) / itemsPerTypePage));
        } catch (error) {
            console.error("Error fetching ticket types:", error);
        } finally {
            setLoadingTicketTypes(false);
        }
    };

    const fetchTickets = async (ticketTypeId: string) => {
        setLoadingTickets(true);
        try {
            const params = new URLSearchParams({
                "TicketTypeId": ticketTypeId,
                "Pagination.Page": currentTicketPage.toString(),
                "Pagination.PageSize": itemsPerTicketPage.toString(),
            });

            const response: TicketFetchResponse = 
                await apiRequest(getApiEndpoint(API_ENDPOINTS.GET_TICKETS, params));
            
            setTickets(response?.items || []);
            setTotalTicketPages(Math.ceil((response?.totalAmount || 0) / itemsPerTicketPage));
        } catch (error) {
            console.error("Error fetching tickets:", error);
        } finally {
            setLoadingTickets(false);
        }
    };

    useEffect(() => {
        if (open) {
            fetchTicketTypes();
        }
    }, [open, currentTypePage]);

    useEffect(() => {
        if (selectedTicketType) {
            fetchTickets(selectedTicketType.typeId.toString());
        }
    }, [selectedTicketType, currentTicketPage]);

    const handleTicketTypeSelect = (ticketType: TicketTypeDto) => {
        setSelectedTicketType(ticketType);
        setCurrentTicketPage(1);
    };

    const handleTicketTypePageChange = (_: React.ChangeEvent<unknown>, page: number) => {
        setCurrentTypePage(page);
    };

    const handleTicketPageChange = (_: React.ChangeEvent<unknown>, page: number) => {
        setCurrentTicketPage(page);
    };

    const handleBack = () => {
        setSelectedTicketType(null);
    };

    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth="md"
            fullWidth
            aria-labelledby="ticket-dialog-title"
        >
            <DialogTitle id="ticket-dialog-title">
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <Typography variant="h6" component="div">
                        {event.name}
                    </Typography>
                    <IconButton onClick={onClose} aria-label="close">
                        <CloseIcon />
                    </IconButton>
                </Box>
            </DialogTitle>
            <DialogContent dividers>
                {selectedTicketType ? (
                    <TicketList
                        ticketType={selectedTicketType}
                        tickets={tickets}
                        loading={loadingTickets}
                        totalPages={totalTicketPages}
                        currentPage={currentTicketPage}
                        handlePageChange={handleTicketPageChange}
                        onBack={handleBack}
                        onDialogClose={onClose}
                    />
                ) : (
                    <TicketTypeList
                        ticketTypes={ticketTypes}
                        loading={loadingTicketTypes}
                        totalPages={totalTypePages}
                        currentPage={currentTypePage}
                        handlePageChange={handleTicketTypePageChange}
                        onTypeSelect={handleTicketTypeSelect}
                    />
                )}
            </DialogContent>
        </Dialog>
    );
};

export default TicketBrowser;
