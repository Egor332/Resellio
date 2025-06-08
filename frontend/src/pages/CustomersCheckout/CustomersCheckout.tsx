import {useParams} from "react-router";
import CartTicketList from "../CustomersCart/CartTicketList.tsx";
import {useSelector} from "react-redux";
import {RootState} from "../../store/store.ts";
import {Box, Button, Divider, Paper, Typography} from "@mui/material";
import {apiRequest} from "../../services/httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../../assets/constants/api.ts";
import { loadStripe } from "@stripe/stripe-js";

const CustomersCheckout = () => {
    const {sellerId} = useParams<{sellerId: string}>()
    const sid = Number(sellerId)
    
    const groupedTickets = useSelector((state: RootState) => state.cart.groupedTickets);
    const tickets = groupedTickets[sid] || [];

    const total = tickets.reduce((sum, t) => sum + t.currentPrice.amount, 0);

    async function handlePayment() {
        try {
            const response = await apiRequest(getApiEndpoint(API_ENDPOINTS.CREATE_CHECKOUT_SESSION),{ sellerId: sid })
            const { publishableKey, sessionId } = response
            
            const stripe = await loadStripe(publishableKey);
            
            if (stripe){
                await stripe.redirectToCheckout({sessionId})
            } else {
                throw new Error("Stripe did not load")
            }
        } catch (error: any) {
            if (error instanceof Error) {
                console.error("Error creating stripe checkout session:", error.message);
            } else {
                console.error("Error creating stripe checkout session:", error);
            }
        }
    }

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                width: "100%",
                maxWidth: 600,
                mx: "auto",
                px: 2,
                pt: 5,
            }}
        >
            <Paper
                elevation={4}
                sx={{
                    p: 4,
                    width: "100%",
                    bgcolor: "background.paper",
                    borderRadius: 3,
                    boxShadow: 6,
                }}
            >
                <Typography
                    variant="h4"
                    component="h1"
                    gutterBottom
                    sx={{ mb: 2, textAlign: "center" }}
                >
                    Checkout
                </Typography>
                <Typography
                    variant="subtitle1"
                    color="text.secondary"
                    sx={{ mb: 3, textAlign: "center" }}
                >
                    Organizer ID: <b>{sellerId}</b>
                </Typography>
                
                <CartTicketList
                    tickets={tickets}
                    showRemoveButton={false}
                />
                <Divider sx={{ my: 3 }} />
                <Box
                    sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        mb: 1,
                        px: 1,
                    }}
                >
                    <Typography variant="h6">
                        Total:
                    </Typography>
                    <Typography variant="h5" fontWeight="bold" color="primary">
                        {total.toFixed(2)} PLN
                    </Typography>
                </Box>
                <Button
                    variant="contained"
                    color="primary"
                    size="large"
                    fullWidth
                    sx={{ mt: 2, py: 1.5, fontSize: 18 }}
                    onClick={handlePayment}
                >
                    Pay now via Stripe
                </Button>
            </Paper>
        </Box>
    );
};

export default CustomersCheckout;