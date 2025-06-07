import { Box, Typography, Button, Chip } from '@mui/material';
import { TicketDto } from '../../dtos/TicketDto';
import { useSelector } from 'react-redux';
import { RootState } from "../../store/store.ts";
import { useState } from 'react';
import ResellDialog from '../ResellDialog/ResellDialog';
import { apiRequest } from '../../services/httpClient.ts';
import { API_ENDPOINTS } from '../../assets/constants/api';

const TicketCard: React.FC<{ 
    ticket: TicketDto; 
    onTicketUpdate: () => void; 
}> = ({ ticket, onTicketUpdate }) => {
    const user = useSelector((state: RootState) => state.auth.user);
    const [openDialog, setOpenDialog] = useState(false);


    const handleResellClick = () => {
        setOpenDialog(true);
    };

    const handleDialogClose = () => {
        setOpenDialog(false);
    };

    const handleConfirmResell = async (price: string) => {
        const resellData = {
            ticketId: ticket.id,
            price: price,
            currency: ticket.currentPrice.currencyCode
        }
        await apiRequest(API_ENDPOINTS.RESELL_TICKET, resellData)
        onTicketUpdate()
    };

    const handleStopReselling = async () => {
        await apiRequest(API_ENDPOINTS.STOP_RESELLING, { ticketId: ticket.id });
        onTicketUpdate()
    };

    return (
        <>
            <Box sx={{ mb: 2, p: 2, border: '1px solid #e0e0e0', borderRadius: '8px' }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
                    <Typography variant="h6">{ticket.eventName}</Typography>
                    {user?.confirmedSeller ?
                        (ticket.isOnSale ? 
                            <Button variant="contained" color="error" onClick={handleStopReselling}>
                                Stop Reselling
                            </Button>
                            :
                            <Button variant="contained" color="primary" onClick={handleResellClick}>
                                Resell
                            </Button>
                        )
                        :
                        <Chip 
                            label="Register as a seller to resell" 
                            color="warning" 
                            size="small"
                        />
                    }
                </Box>
                <Typography variant="body1">
                    {ticket.ticketTypeDescription}
                </Typography>
                <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'left' }}>
                    TicketId: {ticket.id}
                </Typography>
                <Typography variant="h6" sx={{ color: '#2e7d32', mt: 1, textAlign: 'left' }}>
                    {ticket.currentPrice.amount} {ticket.currentPrice.currencyCode}
                </Typography>
            </Box>

            <ResellDialog 
                open={openDialog}
                onClose={handleDialogClose}
                ticket={ticket}
                onConfirm={handleConfirmResell}
            />
        </>
    );
}

export default TicketCard;