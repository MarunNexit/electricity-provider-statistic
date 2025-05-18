import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        mode: 'dark', // Set the mode to dark
        primary: {
            main: '#00796b', // Custom primary color
        },
        secondary: {
            main: '#c2185b', // Custom secondary color
        },
        text: {
            primary: '#e0e0e0', // Light text for dark theme
            secondary: '#b0b0b0', // Light secondary text for dark theme
        },
        background: {
            default: '#000', // Dark background color
            paper: '#1d1d1d', // Darker background for paper components
        },
        divider: '#333', // Divider color for dark mode
    },
    typography: {
        body1: {
            fontWeight: 500, // Default font weight for body1 text
        },
    },
    components: {
        MuiListItemButton: {
            styleOverrides: {
                root: {
                    color: '#b0b0b0',
                    '&.Mui-selected': {
                        color: 'white',
                        backgroundColor: 'rgba(0, 191, 174, 0.1)',
                    },
                    '&:hover': {
                        color: 'white',
                    },
                },
            },
        },
        MuiListItemIcon: {
            styleOverrides: {
                root: {
                    color: '#e0e0e0', // Set default color for icons (light color)
                    '&.Mui-selected': {
                        color: '#ffffff',
                    },
                    '&:hover': {
                        color: 'white',
                    },
                },
            },
        },
    },
});

export default theme;