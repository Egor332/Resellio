import { Box, Typography } from '@mui/material'
import {EventBrowser} from "../../components/EventBrowser/EventBrowser.tsx";

function CustomersHome() {
  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Browse Events
      </Typography>
      <EventBrowser showOrganiserNameFilter={true} />
    </Box>
  )
}

export default CustomersHome
