import { Box, Typography, Paper, Button } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { Navigation } from '../../assets/constants/navigation'

interface PaymentResultPageProps {
  icon: React.ReactNode
  title: string
  message: string
  titleColor: string
  primaryButton: {
    text: string
    path: string
  }
  secondaryButton: {
    text: string
    path: string
  }
}

function PaymentResultPage({
  icon,
  title,
  message,
  titleColor,
  primaryButton,
  secondaryButton
}: PaymentResultPageProps) {
  const navigate = useNavigate()

  const handlePrimaryAction = () => {
    navigate(primaryButton.path)
  }

  const handleSecondaryAction = () => {
    navigate(secondaryButton.path)
  }

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%', p: 3 }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 500, textAlign: 'center' }}>
        {icon}
        
        <Typography variant="h4" component="h1" gutterBottom color={titleColor}>
          {title}
        </Typography>
        
        <Typography variant="body1" sx={{ mb: 3 }}>
          {message}
        </Typography>
        
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            color="primary"
            onClick={handlePrimaryAction}
          >
            {primaryButton.text}
          </Button>
          
          <Button
            variant="outlined"
            color="primary"
            onClick={handleSecondaryAction}
          >
            {secondaryButton.text}
          </Button>
        </Box>
      </Paper>
    </Box>
  )
}

export default PaymentResultPage
