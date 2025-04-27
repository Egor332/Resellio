import { Box, Typography } from '@mui/material'
import { mockedEvents } from '../../assets/MockEventList'
import { EventList } from '../../components/EventList/EventList'

function OrganisersHome() {
  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
      <Typography variant="h6" component="h2" gutterBottom sx={{ mt: 3, mb: 2 }}>
        Your Events
      </Typography>
      <EventList events={mockedEvents} />
    </Box>
  )
}

export default OrganisersHome
