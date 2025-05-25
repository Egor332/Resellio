import {Pagination, Stack} from "@mui/material";
import React from "react";

const EventPagination: React.FC<{
    totalPages: number;
    currentPage: number;
    handlePageChange: (event: React.ChangeEvent<unknown>, page: number) => void;
}> = ({ totalPages, currentPage, handlePageChange }) => {
    return (
        <Stack mt={4} alignItems="center">
            <Pagination
                count={totalPages}
                page={currentPage}
                onChange={handlePageChange}
                color="primary"
            />
        </Stack>
    );
};

export default EventPagination;