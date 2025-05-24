import { Box, Typography } from '@mui/material'
import {EventBrowser} from "../../components/EventBrowser/EventBrowser.tsx";

function OrganisersHome() {
  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
        <Typography variant="h4" component="h1" gutterBottom>
            Organiser Homepage
        </Typography>
        <EventBrowser/>
    </Box>
  )
}

export default OrganisersHome
