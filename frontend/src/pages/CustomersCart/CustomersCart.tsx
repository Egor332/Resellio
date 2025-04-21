import { Box, Typography, Paper } from '@mui/material'

function CustomersCart() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Your Cart
        </Typography>
        <Box sx={{ mt: 3 }}>
          <Typography variant="body1">
            This is your shopping cart. Here you can view the items you've
            added, adjust quantities, and proceed to checkout.
          </Typography>
        </Box>
      </Paper>
    </Box>
  )
}

export default CustomersCart
