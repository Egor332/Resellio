import { Box, Typography, Paper } from '@mui/material'
import {useUserInfo} from "../../hooks/useUserInfo.ts";
import UserInfoComponent from "../../components/UserInfoComponent/UserInfoComponent.tsx";

function OrganisersProfile() {
  const { userInfo } = useUserInfo();
  
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Organiser Profile
        </Typography>
        <Box sx={{ mt: 3 }}>
          <UserInfoComponent userInfo={userInfo}/>
        </Box>
      </Paper>
    </Box>
  )
}

export default OrganisersProfile
