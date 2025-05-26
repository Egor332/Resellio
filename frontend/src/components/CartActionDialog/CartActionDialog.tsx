import {
  Dialog,
  DialogTitle,
  DialogContent,
  Button,
  Box,
  Typography,
} from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { Navigation } from '../../assets/constants/navigation'

interface CartActionDialogProps {
  open: boolean
  onClose: () => void
  parentDialogClose?: () => void
}

const CartActionDialog = ({ open, onClose, parentDialogClose }: CartActionDialogProps) => {
  const navigate = useNavigate()

  const handleGoToCart = () => {
    navigate(Navigation.CUSTOMERS_CART)
    onClose()
    if (parentDialogClose) parentDialogClose()
  }

  const handleContinueShopping = () => {
    onClose()
    if (parentDialogClose) parentDialogClose()
  }

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle align="center">Item added to Cart!</DialogTitle>
      <DialogContent>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: 2,
            my: 2,
          }}
        >
          <Typography variant="body1" align="center">
            What would you like to do next?
          </Typography>
          
          <Box sx={{ display: 'flex', justifyContent: 'space-around', mt: 2 }}>
            <Button 
              variant="contained" 
              color="primary"
              onClick={handleGoToCart}
            >
              View Cart
            </Button>
            
            <Button 
              variant="outlined" 
              color="primary"
              onClick={handleContinueShopping}
            >
              Continue Shopping
            </Button>
          </Box>
        </Box>
      </DialogContent>
    </Dialog>
  )
}

export default CartActionDialog
