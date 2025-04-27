import { Box, Typography, Paper } from '@mui/material'

function OrganisersProfile() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Organiser Profile
        </Typography>
        <Box sx={{ mt: 3 }}>
          <Typography variant="body1">
            This is your organiser profile page. Here you can manage your
            organiser information and settings.
          </Typography>
        </Box>
      </Paper>
    </Box>
  )
}

export default OrganisersProfile
