import { Box, Typography, Paper } from '@mui/material'

function CustomersProfile() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Your Profile
        </Typography>
        <Box sx={{ mt: 3 }}>
          <Typography variant="body1">
            This is your customer profile page. Here you can view and update
            your personal information.
          </Typography>
        </Box>
      </Paper>
    </Box>
  )
}

export default CustomersProfile
