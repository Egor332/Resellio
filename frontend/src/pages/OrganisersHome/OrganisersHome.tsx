import { Box, Typography } from '@mui/material'
import {EventBrowser} from "../../components/EventBrowser/EventBrowser.tsx";
import { useSelector } from 'react-redux';
import { RootState } from '../../store/store'; 
import { User } from '../../store/auth/authSlice';
import { useEffect } from 'react';
import useBanner from '../../hooks/useBanner';
import { useNavigate } from 'react-router-dom';
import { Navigation } from '../../assets/constants/navigation';

function OrganisersHome() {
  const user: User = useSelector((state: RootState) => state.auth.user);
  const organiserName: string = user?.organiserName ?? "";
  const banner = useBanner();
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) {
      banner.showError('User information not available. Please log in again.');
      navigate(Navigation.LOGIN);
      return;
    }
    
    if (organiserName == "") {
      banner.showError('Organiser name is not available. Please contact support.');
      navigate(Navigation.LOGIN);
    }
  }, [user, organiserName, banner, navigate]);


  return (
    <Box sx={{ textAlign: 'center', width: '100%' }}>
        <Typography variant="h4" component="h1" gutterBottom>
            Your Events
        </Typography>
        <EventBrowser 
          showOrganiserNameFilter={false}
          organiserName={organiserName}
        />
    </Box>
  )
}

export default OrganisersHome
