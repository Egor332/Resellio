import { Box, Typography, Paper, Button } from '@mui/material'
import ErrorIcon from '@mui/icons-material/Error'
import { useNavigate } from 'react-router-dom'
import { Navigation } from '../../assets/constants/navigation'

function PaymentFailure() {
  const navigate = useNavigate()

  const handleGoToCart = () => {
    navigate(Navigation.CUSTOMERS_CART)
  }

  const handleGoToHome = () => {
    navigate(Navigation.CUSTOMERS)
  }

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%', p: 3 }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 500, textAlign: 'center' }}>
        <ErrorIcon 
          sx={{ 
            fontSize: 80, 
            color: 'error.main', 
            mb: 2 
          }} 
        />
        
        <Typography variant="h4" component="h1" gutterBottom color="error.main">
          Payment Failed
        </Typography>
        
        <Typography variant="body1" sx={{ mb: 3 }}>
          Unfortunately, your payment could not be processed. Please check your payment details and try again.
        </Typography>
        
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            color="primary"
            onClick={handleGoToCart}
          >
            Return to Cart
          </Button>
          
          <Button
            variant="outlined"
            color="primary"
            onClick={handleGoToHome}
          >
            Back to Home
          </Button>
        </Box>
      </Paper>
    </Box>
  )
}

export default PaymentFailure
