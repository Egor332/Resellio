import React, { useState } from 'react';
import {
    Box,
    List,
    ListItem,
    ListItemText,
    Typography,
    Paper,
    Divider,
    CircularProgress,
    Button,
    IconButton
} from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { TicketDto, TicketTypeDto } from '../../dtos/TicketDto';
import EventPagination from '../EventBrowser/EventPagination';
import { useSelector, useDispatch } from "react-redux";
import { Role } from "../../store/auth/authSlice.ts"
import { RootState, AppDispatch } from "../../store/store.ts";
import CartActionDialog from '../CartActionDialog/CartActionDialog';
import { add } from '../../store/cart/cartSlice';
import useBanner from '../../hooks/useBanner';

interface TicketListProps {
    ticketType: TicketTypeDto;
    tickets: TicketDto[];
    loading: boolean;
    totalPages: number;
    currentPage: number;
    handlePageChange: (event: React.ChangeEvent<unknown>, page: number) => void;
    onBack: () => void;
    onDialogClose: () => void;
}

const TicketList: React.FC<TicketListProps> = ({
    ticketType,
    tickets,
    loading,
    totalPages,
    currentPage,
    handlePageChange,
    onBack,
    onDialogClose
}) => {
    const role = useSelector((state: RootState) => state.auth.user?.role);
    const isCustomer = role === Role.Customer;
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const dispatch = useDispatch<AppDispatch>();
    const banner = useBanner()

    const handleAddToCart = async (ticket: TicketDto) => {
        try {
            await dispatch(add(ticket)).unwrap();
            setIsDialogOpen(true);
        } catch (error: any) {
            banner.showError(error || "Failed to add ticket to cart");
        }
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
    };

    return (
        <Box width="100%">
            <Box display="flex" alignItems="center" mb={2}>
                <IconButton onClick={onBack} sx={{ mr: 1 }}>
                    <ArrowBackIcon />
                </IconButton>
                <Typography variant="h6">
                    Tickets for {ticketType.description}
                </Typography>
            </Box>
            
            {loading ? (
                <Box display="flex" justifyContent="center" my={4}>
                    <CircularProgress size={40} thickness={6} />
                </Box>
            ) : (
                <Paper variant="outlined">
                    <List>
                        {tickets.length > 0 ? (
                            tickets.map((ticket, index) => (
                                <React.Fragment key={ticket.id}>
                                    <ListItem alignItems="flex-start">
                                        <Box width="100%" display="flex" justifyContent="space-between" alignItems="center">
                                            <ListItemText
                                                primary={`Ticket ID: ${ticket.id.substring(0, 8)}...`}
                                                secondary={
                                                    ticket.isHoldByOrganiser ? "From organiser" : "From other customer"
                                                }
                                            />
                                            <Box display="flex" alignItems="center" gap={2}>
                                                <Typography variant="subtitle1" fontWeight="bold">
                                                    {ticket.currentPrice.amount.toFixed(2)} {ticket.currentPrice.currencyCode}
                                                </Typography>
                                                {isCustomer && 
                                                    <Button 
                                                        variant="contained" 
                                                        color="primary" 
                                                        size="small"
                                                        onClick={() => handleAddToCart(ticket)}
                                                    >
                                                        Add to cart
                                                    </Button>
                                                }
                                            </Box>
                                        </Box>
                                    </ListItem>
                                    {index < tickets.length - 1 && <Divider />}
                                </React.Fragment>
                            ))
                        ) : (
                            <ListItem>
                                <ListItemText primary="No tickets available for this type" />
                            </ListItem>
                        )}
                    </List>
                </Paper>
            )}
            
            <Box mt={2} display="flex" justifyContent="center">
                <EventPagination 
                    totalPages={totalPages} 
                    currentPage={currentPage} 
                    handlePageChange={handlePageChange} 
                />
            </Box>

            <CartActionDialog 
                open={isDialogOpen} 
                onClose={handleCloseDialog}
                parentDialogClose={onDialogClose}
            />
        </Box>
    );
};

export default TicketList;
