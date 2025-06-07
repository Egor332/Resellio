import { Box, Typography, Button, Chip } from '@mui/material';
import { TicketDto } from '../../dtos/TicketDto';
import { useSelector } from 'react-redux';
import { RootState, AppDispatch } from "../../store/store.ts";

const TicketCard: React.FC<{ ticket: TicketDto }> = ({ticket}) => {
    const user = useSelector((state: RootState) => state.auth.user);

    return (
        <Box sx={{ mb: 2, p: 2, border: '1px solid #e0e0e0', borderRadius: '8px' }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
                <Typography variant="h6">{ticket.eventName}</Typography>
                {user?.confirmedSeller ?
                    (ticket.isOnSale ? 
                        <Chip 
                            label="Ticket is set for resale"
                            color="success"
                            size="small"
                        />
                        :
                        <Button variant="contained" color="primary">
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
    );
}

export default TicketCard;