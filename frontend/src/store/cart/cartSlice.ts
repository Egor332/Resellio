import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit'
import { EventDtoFetchResponse } from '../../dtos/EventDto'
import { apiRequest } from '../../services/httpClient'
import { getApiEndpoint } from '../../assets/constants/api'
import { Navigation } from '../../assets/constants/navigation'


interface CartState {
    events: EventDtoFetchResponse[]
}

const initialState: CartState = {
    events: [],
}

export const add = createAsyncThunk(
    'cart/add',
    async (event: EventDtoFetchResponse, { rejectWithValue }) => {
        try {
            await apiRequest(getApiEndpoint(Navigation.LOCK_TICKET))
        } catch (error: any) {
            return rejectWithValue(error.message)
        }
    }
)

const cartSlice = createSlice({
    name: 'cart',
    initialState,
    reducers: {
        addToCart: (state, action: PayloadAction<EventDtoFetchResponse>) => {
            state.events.push(action.payload)
        },
        removeFromCart: (state, action: PayloadAction<string>) => {
            state.events = state.events.filter(event => event.id !== action.payload)
        },
        clearCart: (state) => {
            state.events = []
        },
    },
})