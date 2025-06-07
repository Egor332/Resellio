import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, Typography } from '@mui/material';
import { useState } from 'react';
import { TicketDto } from '../../dtos/TicketDto';

interface ResellDialogProps {
    open: boolean;
    onClose: () => void;
    ticket: TicketDto;
    onConfirm: (price: string) => void;
}

const ResellDialog: React.FC<ResellDialogProps> = ({ open, onClose, ticket, onConfirm }) => {
    const [resellPrice, setResellPrice] = useState('');

    const handleClose = () => {
        setResellPrice('');
        onClose();
    };

    const handleConfirm = () => {
        onConfirm(resellPrice);
        handleClose();
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Resell Ticket</DialogTitle>
            <DialogContent>
                <Typography variant="body2" sx={{ mb: 2 }}>
                    Set your resale price for: {ticket.eventName}
                </Typography>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Price"
                    type="number"
                    fullWidth
                    variant="outlined"
                    value={resellPrice}
                    onChange={(e) => setResellPrice(e.target.value)}
                    placeholder={`Current: ${ticket.currentPrice.amount} ${ticket.currentPrice.currencyCode}`}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>
                    Cancel
                </Button>
                <Button 
                    onClick={handleConfirm} 
                    variant="contained"
                    disabled={!resellPrice}
                >
                    Confirm Resell
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ResellDialog;