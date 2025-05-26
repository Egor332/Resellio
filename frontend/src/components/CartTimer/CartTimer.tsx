import { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Typography } from '@mui/material'
import { RootState, AppDispatch } from '../../store/store';
import { fetchCartInfo } from '../../store/cart/cartSlice';

const SECONDS_PER_MINUTE = 60;
const LOW_TIME_THRESHOLD_SECONDS = 120; 

const CartTimer = () => {
  const cartExpirationTime = useSelector((state: RootState) => state.cart.cartExpirationTime);
  const [secondsLeft, setSecondsLeft] = useState(0);

  const dispatch = useDispatch<AppDispatch>()

  useEffect(() => {
    const calculateTimeRemaining = () => {
      if (!cartExpirationTime) return 0;
      
      const expirationTime = new Date(cartExpirationTime).getTime();
      const currentTime = new Date().getTime();
      const timeLeftMs = expirationTime - currentTime;

      if (timeLeftMs < 1000) {
          dispatch(fetchCartInfo());
      }
      
      return Math.max(0, Math.floor(timeLeftMs / 1000));
    };
    
    setSecondsLeft(calculateTimeRemaining());
    
    const interval = setInterval(() => {
      setSecondsLeft(calculateTimeRemaining());
    }, 1000);
    
    return () => clearInterval(interval);
  }, [cartExpirationTime]);

  const formatTime = () => {
    const minutes = Math.floor(secondsLeft / SECONDS_PER_MINUTE);
    const seconds = secondsLeft % SECONDS_PER_MINUTE;
    return `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
  };

  return (
    <>
      {secondsLeft > 0 && (
        <Typography 
          variant="body2" 
          sx={{ 
            color: secondsLeft < LOW_TIME_THRESHOLD_SECONDS ? 'error.main' : '',
            fontSize: '1rem',
          }}
        >
          {formatTime()}
        </Typography>
      )}
    </>
  );
};

export default CartTimer;
