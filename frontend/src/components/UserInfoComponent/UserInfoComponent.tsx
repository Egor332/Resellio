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
                <Typography variant="h6"><strong>First name:</strong> {userInfo.firstName}</Typography>
                <Typography variant="h6"><strong>Last name:</strong> {userInfo.lastName}</Typography>
                <Typography variant="h6"><strong>Email:</strong> {userInfo.email}</Typography>
                <Typography variant="h6">
                    <strong>With us since:</strong> {userInfo.createdDate ? new Date(userInfo.createdDate).toLocaleDateString() : 'N/A'}
                </Typography>
            </Box>
        
            {userInfo.organiserName && <Typography variant="h6"><strong>Company name:</strong> {userInfo.organiserName}</Typography>}
            <Typography variant="h6">
                <strong>Confirmed seller:</strong> {userInfo?.confirmedSeller ? 'YES' : 'NO'}
            </Typography>
        </Paper>
    );
};
export default UserInfoComponent;