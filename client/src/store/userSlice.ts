// store/userSlice.ts
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import {getMyUserData} from "../utils/services/userService.ts";

export const fetchUserProfile = createAsyncThunk(
    'user/fetchUserProfile',
    async () => {
        const response = await getMyUserData();
        return response;
    }
);

interface UserProfile {
    id: string;
    name: string;
    email: string;
    avatarUrl: string;
}

interface UserState {
    profile: UserProfile | null;
    loading: boolean;
    error: string | null;
}

const initialState: UserState = {
    profile: null,
    loading: false,
    error: null,
};

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        clearUserProfile(state) {
            state.profile = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchUserProfile.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchUserProfile.fulfilled, (state, action) => {
                state.profile = action.payload;
                state.loading = false;
            })
            .addCase(fetchUserProfile.rejected, (state) => {
                state.profile = null;
                state.loading = false;
                state.error = 'Failed to fetch profile';
            });
    },
});

export const { clearUserProfile } = userSlice.actions;
export default userSlice.reducer;
