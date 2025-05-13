import React from 'react';
import { Typography, Box, Paper, Container } from '@mui/material';
import {UserInfoDto} from "../../models/dtos/UserInfoDto.ts";

const UserInfoComponent: React.FC<{ userInfo: UserInfoDto | null }> = ({ userInfo }) => {

    if (!userInfo) {
        return (
            <Container>
                <Typography variant="h6" color="textSecondary">
                    No user information available.
                </Typography>
            </Container>
        );
    }

    return (
        <Paper sx={{ p: 3 }}>
            <Box>
                <Typography variant="h6">First name: {userInfo.firstName}</Typography>
                <Typography variant="h6">Last name: {userInfo.lastName}</Typography>
                <Typography variant="body1">Email: {userInfo.email}</Typography>
                <Typography variant="body1">
                    With us since: {userInfo.createdDate ? (userInfo.createdDate instanceof Date ? userInfo.createdDate.toLocaleDateString() : userInfo.createdDate) : 'N/A'}
                </Typography>
            </Box>
        
            {/* Render Organiser information */}
            {userInfo.organiserName && <Box sx={{ mt: 2 }}>Company name: {userInfo.organiserName}</Box>}
            <Typography variant="body1">
                {userInfo?.confirmedSeller ? 'is a confirmed seller' : 'seller certification pending...'}
            </Typography>
        </Paper>
    );
};
export default UserInfoComponent;