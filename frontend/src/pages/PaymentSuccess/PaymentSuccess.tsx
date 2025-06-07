import { Box, Typography, Paper, Button } from '@mui/material'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import { useNavigate } from 'react-router-dom'
import { Navigation } from '../../assets/constants/navigation'

function PaymentSuccess() {
  const navigate = useNavigate()

  const handleGoToTickets = () => {
    navigate(Navigation.CUSTOMERS_MY_TICKETS)
  }

  const handleGoToHome = () => {
    navigate(Navigation.CUSTOMERS)
  }

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%', p: 3 }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 500, textAlign: 'center' }}>
        <CheckCircleIcon 
          sx={{ 
            fontSize: 80, 
            color: 'success.main', 
            mb: 2 
          }} 
        />
        
        <Typography variant="h4" component="h1" gutterBottom color="success.main">
          Payment Successful!
        </Typography>
        
        <Typography variant="body1" sx={{ mb: 3 }}>
          Your payment has been processed successfully. You can now view your tickets in the "My Tickets" section.
        </Typography>
        
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            color="primary"
            onClick={handleGoToTickets}
          >
            View My Tickets
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

export default PaymentSuccess
