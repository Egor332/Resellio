import {Box, Checkbox, FormControlLabel, Grid, MenuItem, Select, TextField} from "@mui/material";
import React from "react";

const EventFilters: React.FC<{
    searchQuery: string;
    setSearchQuery: (value: string) => void;
    showFutureOnly: boolean;
    setShowFutureOnly: (value: boolean) => void;
    organiserNamePart: string;
    setOrganiserNamePart: (value: string) => void;
    startsAfter: string;
    setStartsAfter: (value: string) => void;
    endsBefore: string;
    setEndsBefore: (value: string) => void;
    itemsPerPage: number;
    setItemsPerPage: (value: number) => void;
    setCurrentPage: (value: number) => void;
    showOrganiserNameFilter: boolean;
}> = ({
          searchQuery,
          setSearchQuery,
          showFutureOnly,
          setShowFutureOnly,
          organiserNamePart,
          setOrganiserNamePart,
          startsAfter,
          setStartsAfter,
          endsBefore,
          setEndsBefore,
          itemsPerPage,
          setItemsPerPage,
          setCurrentPage,
          showOrganiserNameFilter,
      }) => {
    const handleFilterChange = <T,>(setter: (value: T) => void) => (value: T) => {
        setter(value);
        setCurrentPage(1);
    };

    return (
        <Box width="100%" maxWidth="1000px" px={2}>
            <Grid container spacing={2} alignItems="center">
                <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                    <TextField
                        fullWidth
                        label="Search events"
                        variant="outlined"
                        value={searchQuery}
                        onChange={(e) => handleFilterChange(setSearchQuery)(e.target.value)}
                    />
                </Grid>
                <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                    <FormControlLabel
                        control={
                            <Checkbox
                                checked={showFutureOnly}
                                onChange={(e) => handleFilterChange(setShowFutureOnly)(e.target.checked)}
                                color="primary"
                            />
                        }
                        label="Hide past events"
                    />
                </Grid>
                {
                    showOrganiserNameFilter && 
                    <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                        <TextField
                            fullWidth
                            label="Organiser Name"
                            variant="outlined"
                            value={organiserNamePart}
                            onChange={(e) => handleFilterChange(setOrganiserNamePart)(e.target.value)}
                        />
                    </Grid>
                }
                <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                    <TextField
                        fullWidth
                        label="Starts After"
                        type="date"
                        InputLabelProps={{ shrink: true }}
                        value={startsAfter}
                        onChange={(e) => handleFilterChange(setStartsAfter)(e.target.value)}
                    />
                </Grid>
                <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                    <TextField
                        fullWidth
                        label="Ends Before"
                        type="date"
                        InputLabelProps={{ shrink: true }}
                        value={endsBefore}
                        onChange={(e) => handleFilterChange(setEndsBefore)(e.target.value)}
                    />
                </Grid>
                <Grid size={{xs: 12, sm: 6, md: 4, lg: 3}}>
                    <Box display="flex" alignItems="center" gap={1}>
                        <span>Items per page:</span>
                        <Select
                            value={itemsPerPage}
                            onChange={(e) => handleFilterChange(setItemsPerPage)(Number(e.target.value))}
                        >
                            <MenuItem value={3}>3</MenuItem>
                            <MenuItem value={5}>5</MenuItem>
                            <MenuItem value={10}>10</MenuItem>
                        </Select>
                    </Box>
                </Grid>
            </Grid>
        </Box>
    );
};

export default EventFilters;