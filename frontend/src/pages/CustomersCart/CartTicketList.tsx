import { 
  List,
  ListItem,
  ListItemText,
  Divider,
  Typography,
  Box,
  Button,
  CircularProgress
} from '@mui/material';
import { TicketDto } from '../../dtos/TicketDto';

interface CartTicketListProps {
  tickets: TicketDto[];
  onRemove: (ticket: TicketDto) => void;
  removingId: string | null;
}

const CartTicketList: React.FC<CartTicketListProps> = ({
  tickets,
  onRemove,
  removingId
}) => {
  return (
    <List disablePadding>
      {tickets.map((ticket, index) => (
        <Box key={ticket.id}>
          <ListItem alignItems="flex-start" sx={{ py: 2 }}>
            <Box width="100%" display="flex" justifyContent="space-between" alignItems="center">
              <Box>
                <ListItemText
                  primary={
                    <Typography variant="subtitle1" component="span">
                      Ticket ID: {ticket.id.substring(0, 8)}...
                    </Typography>
                  }
                  secondary={
                    <>
                      <Typography variant="body2" component="span" display="block">
                        Event name: {ticket.eventName || 'Event name not available'}
                      </Typography>
                      <Typography variant="body2" component="span" color="text.secondary" display="block">
                        Type: {ticket.ticketTypeDescription || 'Standard'}
                      </Typography>
                    </>
                  }
                />
              </Box>

              <Box display="flex" flexDirection="column" alignItems="flex-end" gap={1}>
                <Typography variant="subtitle1" fontWeight="bold">
                  {ticket.currentPrice.amount.toFixed(2)} {ticket.currentPrice.currencyCode}
                </Typography>
                
                <Button
                  variant="outlined"
                  color="error"
                  size="small"
                  onClick={() => onRemove(ticket)}
                  disabled={removingId === ticket.id}
                  startIcon={removingId === ticket.id ? <CircularProgress size={16} /> : null}
                >
                  {removingId === ticket.id ? 'Removing...' : 'Remove'}
                </Button>
              </Box>
            </Box>
          </ListItem>
          {index < tickets.length - 1 && <Divider />}
        </Box>
      ))}
    </List>
  );
};

export default CartTicketList;
