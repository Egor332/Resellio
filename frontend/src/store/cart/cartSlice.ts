import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import { apiRequest } from '../../services/httpClient'
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api'
import { TicketDto, TicketLockResponse } from '../../dtos/TicketDto'
import cartService from '../../services/cartService'


interface GroupedTickets {
    [sellerId: number]: TicketDto[]
}

interface CartState {
    groupedTickets: GroupedTickets
    loading: boolean
    error: string | null
    cartExpirationTime: string | null
}

const initialState: CartState = {
    groupedTickets: {},
    loading: false,
    error: null,
    cartExpirationTime: null
}

export const fetchCartInfo = createAsyncThunk(
    'cart/fetchCartInfo',
    async (_, { rejectWithValue }) => {
        try {
            const response = await cartService.getCartInfo();
            
            if (!response || !response.isCartExist) {
                return { tickets: [], expirationTime: null };
            }
            
            return { 
                tickets: response.ticketsInCart, 
                expirationTime: response.cartExpirationTime 
            };
        } catch (error: any) {
            return rejectWithValue(error.message || 'Failed to fetch cart info');
        }
    }
)

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
                expirationTime: response.expirationTime
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
            state.cartExpirationTime = null
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
                state.cartExpirationTime = action.payload.expirationTime
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
            
            .addCase(fetchCartInfo.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(fetchCartInfo.fulfilled, (state, action) => {
                state.loading = false
                state.groupedTickets = {}
                state.cartExpirationTime = action.payload.expirationTime
                
                const tickets = action.payload.tickets as TicketDto[];
                if (tickets && tickets.length > 0) {
                    tickets.forEach(ticket => {
                        if (!state.groupedTickets[ticket.sellerId]) {
                            state.groupedTickets[ticket.sellerId] = [];
                        }
                        state.groupedTickets[ticket.sellerId].push(ticket);
                    });
                }
            })
            .addCase(fetchCartInfo.rejected, (state, action) => {
                state.loading = false
                state.error = action.payload as string || 'Failed to fetch cart info'
            })
    },
})

export const { clearCart } = cartSlice.actions
export default cartSlice.reducer