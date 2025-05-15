import { Box, Typography } from '@mui/material'
import {EventList} from "../../components/EventList/EventList.tsx";

function CustomersHome() {
  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Customer Homepage
      </Typography>
        <EventList/>
    </Box>
  )
}

export default CustomersHome
