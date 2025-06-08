import {
    Box,
    Typography,
    Button,
    Chip,
    DialogContent,
    CircularProgress,
    IconButton,
    DialogTitle,
    Dialog
} from '@mui/material';
import { TicketDto } from '../../dtos/TicketDto';
import { useSelector } from 'react-redux';
import { RootState } from "../../store/store.ts";
import { useState } from 'react';
import ResellDialog from '../ResellDialog/ResellDialog';
import { apiRequest } from '../../services/httpClient.ts';
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api';
import CloseIcon from "@mui/icons-material/Close";
import QrCode2Icon from '@mui/icons-material/QrCode2';

const TicketCard: React.FC<{ 
    ticket: TicketDto; 
    onTicketUpdate: () => void; 
}> = ({ ticket, onTicketUpdate }) => {
    const user = useSelector((state: RootState) => state.auth.user);
    const [openDialog, setOpenDialog] = useState(false);

    const [qrDialogOpen, setQrDialogOpen] = useState(false);
    const [qrLoading, setQrLoading] = useState(false);
    const [qrImage, setQrImage] = useState<string | null>(null);
    const [qrError, setQrError] = useState<string | null>(null);

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
        await apiRequest(getApiEndpoint(API_ENDPOINTS.RESELL_TICKET), resellData)
        onTicketUpdate()
    };

    const handleStopReselling = async () => {
        await apiRequest(getApiEndpoint(API_ENDPOINTS.STOP_RESELLING), { ticketId: ticket.id });
        onTicketUpdate()
    };

    const handleShowQr = async () => {
        setQrDialogOpen(true);
        setQrLoading(true);
        setQrImage(null);
        setQrError(null);
        
        try {
            const urlParams = new URLSearchParams({ ticketId: ticket.id })
            const response = await apiRequest(getApiEndpoint(API_ENDPOINTS.GET_QR_CODE, urlParams));

            const imageSrc = `data:image/png;base64,${response.qrCodeImage}`;
            
            if (response?.qrCodeImage) {
                setQrImage(imageSrc);
            } else {
                setQrError("QR code fetching failed");
            }
        } catch (err) {
            setQrError("QR code fetchin failed");
        } finally {
            setQrLoading(false);
        }
    }

    const handleQrDialogClose = () => {
        setQrDialogOpen(false);
        setQrImage(null);
        setQrError(null);
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
                <Box sx={{ display: 'flex', justifyContent: 'flex-end', mb: 1 }}>
                    <Button
                        variant="outlined"
                        color="secondary"
                        startIcon={<QrCode2Icon />}
                        onClick={handleShowQr}
                        disabled={ticket.isOnSale}
                        sx={{
                            fontWeight: 600,
                            textTransform: 'none',
                            borderRadius: 2,
                            px: 2
                        }}
                    >
                        Show QR Code
                    </Button>
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

            <Dialog open={qrDialogOpen} onClose={handleQrDialogClose} maxWidth="xs" fullWidth>
                <DialogTitle sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', pb: 1 }}>
                    QR Code for your ticket
                    <IconButton onClick={handleQrDialogClose} size="small">
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: 240 }}>
                    {qrLoading && <CircularProgress />}
                    {qrError && <Typography color="error">{qrError}</Typography>}
                    {qrImage && (
                        <img
                            src={qrImage}
                            alt="Kod QR"
                            style={{ width: 220, height: 220, margin: '16px 0' }}
                        />
                    )}
                </DialogContent>
            </Dialog>
            
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