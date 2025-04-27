import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import authService from '../../services/authService.ts'

export type Role = 'Customer' | 'Admin' | 'Organiser'

export interface Customer {
  email: string
  firstName: string
  lastName: string
  role: Role
}

export interface Organiser {
  email: string
  firstName: string
  lastName: string
  organiserName: string
  role: Role
}

interface AuthState {
  user: Customer | Organiser | null
  token: string | null
  isAuthenticated: boolean
  loading: boolean
  error: string | null
}

export interface LoginCredentials {
  email: string
  password: string
}

export interface CustomerRegisterData {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface OrganiserRegisterData {
  firstName: string
  lastName: string
  organiserName: string
  email: string
  password: string
}

const initialState: AuthState = {
  user: null,
  token: localStorage.getItem('auth_token') || null,
  isAuthenticated: !!localStorage.getItem('auth_token'),
  loading: false,
  error: null,
}

const createEmptyUserByRole = (role: Role): Customer | Organiser => {
  if (role === 'Organiser') {
    return {
      email: '',
      firstName: '',
      lastName: '',
      organiserName: '',
      role: role,
    } as Organiser
  } else if (role === 'Customer') {
    return {
      email: '',
      firstName: '',
      lastName: '',
      role: role,
    } as Customer
  } else {
    throw new Error('Invalid role')
  }
}

export const loginUser = createAsyncThunk(
  'auth/login',
  async (credentials: LoginCredentials, { rejectWithValue }) => {
    try {
      return await authService.login(credentials)
    } catch (error: any) {
      return rejectWithValue(error.message)
    }
  }
)

export const registerCustomer = createAsyncThunk(
  'auth/registerCustomer',
  async (customerData: CustomerRegisterData, { rejectWithValue }) => {
    try {
      return await authService.registerCustomer(customerData)
    } catch (error: any) {
      return rejectWithValue(error.message)
    }
  }
)

export const registerOrganiser = createAsyncThunk(
  'auth/registerOrganiser',
  async (organiserData: OrganiserRegisterData, { rejectWithValue }) => {
    try {
      return await authService.registerOrganiser(organiserData)
    } catch (error: any) {
      return rejectWithValue(error.message)
    }
  }
)

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout: (state) => {
      state.user = null
      state.token = null
      state.isAuthenticated = false
      localStorage.removeItem('auth_token')
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true
        state.error = null
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.loading = false
        state.isAuthenticated = true

        state.user = createEmptyUserByRole(action.payload.userRole)

        state.token = action.payload.token
        localStorage.setItem('auth_token', action.payload.token)
      })
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false
        state.error = action.payload as string
        state.isAuthenticated = false
      })

      .addCase(registerCustomer.pending, (state) => {
        state.loading = true
        state.error = null
      })
      .addCase(registerCustomer.fulfilled, (state) => {
        state.loading = false
      })
      .addCase(registerCustomer.rejected, (state, action) => {
        state.loading = false
        state.error = action.payload as string
      })

      .addCase(registerOrganiser.pending, (state) => {
        state.loading = true
        state.error = null
      })
      .addCase(registerOrganiser.fulfilled, (state) => {
        state.loading = false
      })
      .addCase(registerOrganiser.rejected, (state, action) => {
        state.loading = false
        state.error = action.payload as string
      })
  },
})

export const { logout } = authSlice.actions
export default authSlice.reducer
