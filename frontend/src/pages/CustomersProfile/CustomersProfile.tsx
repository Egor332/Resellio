import { Box, Typography, Paper } from '@mui/material'
import UserInfoComponent from "../../components/UserInfoComponent/UserInfoComponent.tsx";
import {useUserInfo} from "../../hooks/useUserInfo.ts";

function CustomersProfile() {
  const { userInfo } = useUserInfo();
  
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Your Profile
        </Typography>
        <Box sx={{ mt: 3 }}>
          <UserInfoComponent userInfo={userInfo}/>
        </Box>
      </Paper>
    </Box>
  )
}

export default CustomersProfile
