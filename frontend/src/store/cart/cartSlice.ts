import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import { apiRequest } from '../../services/httpClient'
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api'
import { TicketDto, TicketLockResponse } from '../../dtos/TicketDto'


interface GroupedTickets {
    [sellerId: number]: TicketDto[]
}

interface CartState {
    groupedTickets: GroupedTickets
    loading: boolean
    error: string | null
}

const initialState: CartState = {
    groupedTickets: {},
    loading: false,
    error: null
}

export const add = createAsyncThunk(
    'cart/add',
    async (ticket: TicketDto, { rejectWithValue }) => {
        try {
            const response = await apiRequest(
                getApiEndpoint(API_ENDPOINTS.LOCK_TICKET), 
                { ticketId: ticket.id }
            );
            
            if (!response || !response.success) {
                return rejectWithValue(response?.message || 'Failed to lock ticket');
            }
            
            return {
                ...ticket,
                lockExpirationTime: response.expirationTime
            };
        } catch (error: any) {
            return rejectWithValue(error.message);
        }
    }
)

export const remove = createAsyncThunk(
    'cart/remove',
    async (ticketId: string, { rejectWithValue }) => {
        try {
            const response = await apiRequest(
                getApiEndpoint(API_ENDPOINTS.UNLOCK_TICKET), 
                { ticketId }
            );
            
            if (!response || !response.success) {
                return rejectWithValue(response?.message || 'Failed to unlock ticket');
            }
            
            return ticketId;
        } catch (error: any) {
            return rejectWithValue(error.message);
        }
    }
)

const cartSlice = createSlice({
    name: 'cart',
    initialState,
    reducers: {
        clearCart: (state) => {
            state.groupedTickets = {}
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(add.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(add.fulfilled, (state, action) => {
                state.loading = false
                const ticket = action.payload as TicketDto;
                if (!state.groupedTickets[ticket.sellerId]) {
                    state.groupedTickets[ticket.sellerId] = [];
                }
                state.groupedTickets[ticket.sellerId].push(ticket);
            })
            .addCase(add.rejected, (state, action) => {
                state.loading = false
                state.error = action.payload as string || 'Failed to add ticket to cart'
            })
            
            .addCase(remove.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(remove.fulfilled, (state, action) => {
                state.loading = false
                const ticketId = action.meta.arg;
                
                Object.keys(state.groupedTickets).forEach((sellerIdKey) => {
                    const sellerId = Number(sellerIdKey);
                    state.groupedTickets[sellerId] = state.groupedTickets[sellerId].filter(
                        ticket => ticket.id !== ticketId
                    );
                    
                    if (state.groupedTickets[sellerId].length === 0) {
                        delete state.groupedTickets[sellerId];
                    }
                });
            })
            .addCase(remove.rejected, (state, action) => {
                state.loading = false
                state.error = action.payload as string || 'Failed to remove ticket from cart'
            })
    },
})

export const { clearCart } = cartSlice.actions
export default cartSlice.reducer