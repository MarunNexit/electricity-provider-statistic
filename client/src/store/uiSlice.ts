import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UIState {
    sidebarCollapsed: boolean;
}

const initialState: UIState = {
    sidebarCollapsed: false,
};

const uiSlice = createSlice({
    name: 'ui',
    initialState,
    reducers: {
        toggleSidebar(state) {
            state.sidebarCollapsed = !state.sidebarCollapsed;
        },
        setSidebarCollapsed(state, action: PayloadAction<boolean>) {
            state.sidebarCollapsed = action.payload;
        },
        resetSidebarCollapsed(state) {
            state.sidebarCollapsed = false;
        },
    },
});

export const { toggleSidebar, setSidebarCollapsed, resetSidebarCollapsed } = uiSlice.actions;
export default uiSlice.reducer;
