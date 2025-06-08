import React from 'react';
import {
    Typography,
    Box,
    Paper,
    Container,
    Button,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions
} from '@mui/material';
import {UserInfoDto} from "../../models/dtos/UserInfoDto.ts";
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";
import useBanner from "../../hooks/useBanner.ts";

type Props = {
    userInfo: UserInfoDto | null;
    onConfirmed: () => void;
};


const UserInfoComponent: React.FC<Props> = ({ userInfo, onConfirmed }) => {
    const banner = useBanner()
    
    const [dialogOpen, setDialogOpen] = React.useState(false);
    const openConfirmDialog = () => setDialogOpen(true);
    const closeConfirmDialog = () => setDialogOpen(false);



    const handleConfirmSeller = async () => {
        try{
            const response = await apiRequest(getApiEndpoint(API_ENDPOINTS.CONFIRM_SELLER))
            const redirectUrl = response?.redirectStripeUrl;

            if (redirectUrl) {
                onConfirmed()
                window.location.href = redirectUrl;
            } else {
                banner.showError("Couldn't confirm the seller: missing redirect URL");
            }
        } catch (error) {
            console.error("Error trying to confirm the user: ", error);
            banner.showError("Couldn't confirm the seller")
        } finally {
            closeConfirmDialog();
        }
    }
    
    if (!userInfo) {
        return (
            <Container>
                <Typography variant="h6" color="textSecondary">
                    No user information available.
                </Typography>
            </Container>
        );
    }

    return (
        <>
            <Paper sx={{ p: 3 }}>
                <Box>
                    <Typography variant="h6"><strong>First name:</strong> {userInfo.firstName}</Typography>
                    <Typography variant="h6"><strong>Last name:</strong> {userInfo.lastName}</Typography>
                    <Typography variant="h6"><strong>Email:</strong> {userInfo.email}</Typography>
                    <Typography variant="h6">
                        <strong>With us since:</strong> {userInfo.createdDate ? new Date(userInfo.createdDate).toLocaleDateString() : 'N/A'}
                    </Typography>
                </Box>
            
                {userInfo.organiserName && <Typography variant="h6"><strong>Company name:</strong> {userInfo.organiserName}</Typography>}
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                    <Typography variant="h6">
                        <strong>Confirmed seller:</strong> {userInfo?.confirmedSeller ? 'YES' : 'NO'}
                    </Typography>
                    {!userInfo.confirmedSeller && (
                        <Button
                            size="small"
                            variant="outlined"
                            onClick={openConfirmDialog}
                        >
                            Confirm
                        </Button>
                    )}
                </Box>
            </Paper>
            <Dialog open={dialogOpen} onClose={closeConfirmDialog}>
                <DialogTitle>Confirm seller status</DialogTitle>
                <DialogContent>
                    <Typography>
                        Confirming yourself as a seller means that you will be able to post and manage listings.
                        This will be performed automatically — you will be confirmed if you have a Stripe account set up.
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={closeConfirmDialog}>Cancel</Button>
                    <Button onClick={handleConfirmSeller} variant="contained" color="primary">
                        Confirm
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};
export default UserInfoComponent;