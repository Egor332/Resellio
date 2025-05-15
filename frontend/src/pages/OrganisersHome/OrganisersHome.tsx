import { Box, Typography } from '@mui/material'
import {EventList} from "../../components/EventList/EventList.tsx";

function OrganisersHome() {
  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
        <Typography variant="h4" component="h1" gutterBottom>
            Organiser Homepage
        </Typography>
        <EventList/>
    </Box>
  )
}

export default OrganisersHome
