import { useEffect, useState } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {
  Box,
  Typography,
  Paper,
  CircularProgress,
  Button,
} from '@mui/material'
import { AppDispatch, RootState } from '../../store/store'
import { fetchCartInfo, remove } from '../../store/cart/cartSlice'
import useBanner from '../../hooks/useBanner'
import CartTicketList from './CartTicketList'
import { TicketDto } from '../../dtos/TicketDto'
import {useNavigate} from "react-router-dom";
import {Navigation} from '../../assets/constants/navigation'

function CustomersCart() {
  const dispatch = useDispatch<AppDispatch>()
  const banner = useBanner()
  const navigate = useNavigate()
  const [isRemoving, setIsRemoving] = useState<string | null>(null)

  const { groupedTickets, loading, error } = useSelector(
    (state: RootState) => state.cart
  )
  const sellerIds = Object.keys(groupedTickets).map(Number)
  const hasItems = sellerIds.length > 0

  useEffect(() => {
    dispatch(fetchCartInfo())
  }, [dispatch])

  const handleRemoveFromCart = async (ticket: TicketDto) => {
    try {
      setIsRemoving(ticket.id)
      await dispatch(remove(ticket.id)).unwrap()
      banner.showSuccess('Item removed from cart')
    } catch (error: any) {
      banner.showError(error.message || 'Failed to remove item from cart')
    } finally {
      setIsRemoving(null)
    }
  }
  
  const handleProceedToCheckout = (sellerId: number) => {
    navigate(Navigation.CUSTOMERS_CHECKOUT_FOR_ORGANISER.replace(':sellerId', sellerId.toString()))
  }

  const calculatePriceForSeller = (sellerId: number) => {
    let sum = 0
    groupedTickets[sellerId].forEach((ticket: TicketDto) => {
      sum += ticket.currentPrice.amount
    })
    return sum.toFixed(2)
  }

  if (loading && !hasItems) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          height: '300px',
        }}
      >
        <CircularProgress />
      </Box>
    )
  }

  if (error) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
        <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 800 }}>
          <Typography variant="h6" color="error" align="center">
            {error}
          </Typography>
          <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center' }}>
            <Button
              variant="contained"
              onClick={() => dispatch(fetchCartInfo())}
            >
              Retry
            </Button>
          </Box>
        </Paper>
      </Box>
    )
  }

  if (!hasItems) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
        <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 800 }}>
          <Typography variant="h4" component="h1" gutterBottom align="center">
            Your Cart
          </Typography>
          <Typography variant="body1" align="center" sx={{ mt: 3 }}>
            Your cart is empty. Add some tickets to get started.
          </Typography>
        </Paper>
      </Box>
    )
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        width: '100%',
        maxWidth: 1000,
        mx: 'auto',
        px: 2,
      }}
    >
      <Typography variant="h4" component="h1" gutterBottom sx={{ mb: 3 }}>
        Your Cart
      </Typography>

      {sellerIds.map((sellerId) => (
        <Paper
          key={sellerId}
          elevation={3}
          sx={{ mb: 4, overflow: 'hidden' }}
        >
          <Box sx={{ bgcolor: 'primary.main', color: 'white', py: 1, px: 2 }}>
            <Typography variant="h6">Seller ID: {sellerId}</Typography>
          </Box>

          <CartTicketList
            tickets={groupedTickets[sellerId]}
            onRemove={handleRemoveFromCart}
            removingId={isRemoving}
          />

          <Box sx={{ 
            display: 'flex', 
            justifyContent: 'space-between', 
            alignItems: 'center',
            p: 2,
            borderTop: 1,
            borderColor: 'divider'
          }}>
            <Typography variant="h5">
              Total price: {calculatePriceForSeller(sellerId)} PLN
            </Typography>
            
            <Button
              variant="contained"
              color="primary"
              size="large"
              onClick={() => handleProceedToCheckout(sellerId)}
            >
              Proceed to Checkout
            </Button>
          </Box>

        </Paper>
      ))}

    </Box>
  )
}

export default CustomersCart
