import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import {checkAuthStatus, loginUser, logoutUser} from "../utils/services/authService.ts";
import {useNavigate} from "react-router-dom";

export const fetchAuthStatus = createAsyncThunk(
    'auth/fetchAuthStatus',
    async () => {
        const response = await checkAuthStatus();
        console.log(response);
        return response.isAuthenticated;
    }
);

export const logoutAsync = createAsyncThunk(
    'auth/logout',
    async () => {
        await logoutUser();
    }
);

interface LoginParams {
    email: string;
    password: string;
}

export const loginAsync = createAsyncThunk<void, LoginParams>(
    'auth/login',
    async ({ email, password }) => {
        await loginUser({ email, password });
    }
);


interface AuthState {
    isAuthenticated: boolean;
    loading: boolean;
}

const initialState: AuthState = {
    isAuthenticated: false,
    loading: true,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout(state) {
            state.isAuthenticated = false;
        },
        login(state) {
            state.isAuthenticated = true;
        },
        resetAuthState(state) {
            state.isAuthenticated = false;
            state.loading = false;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchAuthStatus.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchAuthStatus.fulfilled, (state, action) => {
                state.isAuthenticated = action.payload;
                state.loading = false;
            })
            .addCase(fetchAuthStatus.rejected, (state) => {
                state.isAuthenticated = false;
                state.loading = false;
            })
            .addCase(loginAsync.fulfilled, (state) => {
                state.isAuthenticated = true;
                state.loading = false;
            })
            .addCase(logoutAsync.fulfilled, (state) => {
                state.isAuthenticated = false;
                state.loading = false;
            });
    },
});

export const { logout, login, resetAuthState } = authSlice.actions;
export default authSlice.reducer;
