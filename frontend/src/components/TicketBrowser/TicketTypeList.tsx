import React from 'react';
import {
    Box,
    List,
    ListItem,
    ListItemText,
    Typography,
    Paper,
    Divider,
    CircularProgress,
    Button
} from '@mui/material';
import { TicketTypeDto } from '../../dtos/TicketDto';
import EventPagination from '../EventBrowser/EventPagination';

interface TicketTypeListProps {
    ticketTypes: TicketTypeDto[];
    loading: boolean;
    totalPages: number;
    currentPage: number;
    handlePageChange: (event: React.ChangeEvent<unknown>, page: number) => void;
    onTypeSelect: (ticketType: TicketTypeDto) => void;
}

const TicketTypeList: React.FC<TicketTypeListProps> = ({
    ticketTypes,
    loading,
    totalPages,
    currentPage,
    handlePageChange,
    onTypeSelect
}) => {
    return (
        <Box width="100%">
            <Typography variant="h6" mb={2}>Available Ticket Types</Typography>
            
            {loading ? (
                <Box display="flex" justifyContent="center" my={4}>
                    <CircularProgress size={40} thickness={6} />
                </Box>
            ) : (
                <Paper variant="outlined">
                    <List>
                        {ticketTypes.length > 0 ? (
                            ticketTypes.map((type, index) => (
                                <React.Fragment key={type.typeId}>
                                    <ListItem alignItems="flex-start">
                                        <Box width="100%">
                                            <ListItemText
                                                primary={type.description}
                                                secondary={
                                                    <React.Fragment>
                                                        <Typography component="span" variant="body2" color="text.primary">
                                                            From {type.basePrice.amount.toFixed(2)} {type.basePrice.currencyCode}
                                                        </Typography>
                                                    </React.Fragment>
                                                }
                                            />
                                            <Box display="flex" justifyContent="flex-end" mt={1}>
                                                <Button 
                                                    variant="outlined" 
                                                    color="primary" 
                                                    size="small"
                                                    onClick={() => onTypeSelect(type)}
                                                >
                                                    View Available Tickets
                                                </Button>
                                            </Box>
                                        </Box>
                                    </ListItem>
                                    {index < ticketTypes.length - 1 && <Divider />}
                                </React.Fragment>
                            ))
                        ) : (
                            <ListItem>
                                <ListItemText primary="No ticket types available" />
                            </ListItem>
                        )}
                    </List>
                </Paper>
            )}
            
            <Box mt={2} display="flex" justifyContent="center">
                <EventPagination 
                    totalPages={totalPages} 
                    currentPage={currentPage} 
                    handlePageChange={handlePageChange} 
                />
            </Box>
        </Box>
    );
};

export default TicketTypeList;
