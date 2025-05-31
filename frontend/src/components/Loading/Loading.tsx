import {
    Box,
    CircularProgress,
} from '@mui/material';

export default function Loading() {
    return (
        <Box display="flex" justifyContent="center" my={4}>
            <CircularProgress 
                size={60} 
                thickness={12} 
                sx={{ color: 'primary' }} 
            />
        </Box>
    );
}